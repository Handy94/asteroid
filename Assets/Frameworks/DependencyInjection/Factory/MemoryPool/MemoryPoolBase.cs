using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UniRx.Async;


namespace HandyPackage
{
    public enum PoolExpandMethod
    {
        ExpandByOne,
        Double
    }

    public abstract class MemoryPoolBase<TValue> : IMemoryPool, IInitializable
    {
        private IFactory<TValue> factory;
        private PoolExpandMethod expandMethod = PoolExpandMethod.ExpandByOne;
        private List<TValue> _activeItems = new List<TValue>();
        private Stack<TValue> _inactiveItems = new Stack<TValue>();

        private int InitialPoolSize { get; set; }

        public int NumInactive => _inactiveItems.Count;
        public int NumActive => _activeItems.Count;
        public int NumTotal => NumActive + NumInactive;

        #region IInitializable Interface Implementation
        public async UniTask Initialize()
        {
            await Resize(InitialPoolSize);
            await UniTask.CompletedTask;
        }
        #endregion

        #region IMemoryPool Interface Implementation
        public async UniTask Resize(int desiredPoolSize)
        {
            while (NumTotal != desiredPoolSize)
            {
                if (NumTotal > desiredPoolSize) OnDestroyed(_inactiveItems.Pop());
                else await AllocThenPush();
            }
        }

        public UniTask Clear()
        {
            return ShrinkBy(NumTotal);
        }

        public UniTask ExpandBy(int numToAdd)
        {
            return Resize(NumTotal + numToAdd);
        }

        public UniTask ShrinkBy(int numToRemove)
        {
            return Resize(NumTotal - numToRemove);
        }
        #endregion

        public MemoryPoolBase<TValue> WithInitialPoolSize(int size)
        {
            InitialPoolSize = size;
            return this;
        }

        public MemoryPoolBase<TValue> WithExpandMethod(PoolExpandMethod expandMethod)
        {
            this.expandMethod = expandMethod;
            return this;
        }

        public MemoryPoolBase<TValue> FromFactory<TFactory>() where TFactory : IFactory<TValue>
        {
            return this.FromFactory(DIResolver.GetObject<TFactory>());
        }

        public MemoryPoolBase<TValue> FromFactory(IFactory<TValue> factory)
        {
            this.factory = factory;
            return this;
        }

        private async UniTask<TValue> AllocNew()
        {
            var item = await factory.Create();
            OnCreated(item);
            return item;
        }

        private async UniTask AllocThenPush()
        {
            _inactiveItems.Push(await AllocNew());
        }

        private async UniTask ExpandPool()
        {
            int currentPoolSize = NumTotal;
            int desiredPoolSize = currentPoolSize;
            switch (expandMethod)
            {
                case PoolExpandMethod.ExpandByOne: desiredPoolSize += 1; break;
                case PoolExpandMethod.Double: desiredPoolSize *= 2; break;
            }
            List<UniTask> tasks = new List<UniTask>();
            for (int i = currentPoolSize; i < desiredPoolSize; i++)
            {
                tasks.Add(AllocThenPush());
            }
            if (tasks.Count > 0)
            {
                await UniTask.WhenAll(tasks);
            }
            await UniTask.CompletedTask;
        }

        public async UniTask<TValue> GetInternal()
        {
            if (_inactiveItems.Count == 0)
            {
                await ExpandPool();
            }

            var item = _inactiveItems.Pop();
            _activeItems.Add(item);
            return item;
        }

        public void Despawn(TValue item)
        {
            if (!_activeItems.Contains(item)) return;

            OnDespawned(item);
            _inactiveItems.Push(item);
            _activeItems.Remove(item);
        }

        protected virtual void OnCreated(TValue item)
        {
            if (item is IPoolableBase) ((IPoolableBase)item).OnCreated();
        }

        protected virtual void OnDespawned(TValue item)
        {
            if (item is IPoolableBase) ((IPoolableBase)item).OnDespawned();
        }

        protected virtual void OnDestroyed(TValue item)
        {
            if (item is IPoolableBase) ((IPoolableBase)item).OnDestroyed();
        }
    }
}


