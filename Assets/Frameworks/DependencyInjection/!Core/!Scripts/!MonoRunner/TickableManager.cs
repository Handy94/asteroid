namespace HandyPackage
{
    using System.Collections.Generic;

    public class TickableManager
    {
        private List<IFixedTickable> _fixedTickables = new List<IFixedTickable>();
        private List<ITickable> _tickables = new List<ITickable>();
        private List<ILateTickable> _lateTickables = new List<ILateTickable>();

        public void RegisterFixedTickable(IFixedTickable fixedTickable)
        {
            if (_fixedTickables.Contains(fixedTickable)) return;
            _fixedTickables.Add(fixedTickable);
        }

        public void RegisterTickable(ITickable tickable)
        {
            if (_tickables.Contains(tickable)) return;
            _tickables.Add(tickable);
        }

        public void RegisterLateTickable(ILateTickable lateTickable)
        {
            if (_lateTickables.Contains(lateTickable)) return;
            _lateTickables.Add(lateTickable);
        }

        public void FixedTick()
        {
            int count = _fixedTickables.Count;
            for (int i = 0; i < count; i++)
            {
                _fixedTickables[i].FixedTick();
            }
        }

        public void Tick()
        {
            int count = _tickables.Count;
            for (int i = 0; i < count; i++)
            {
                _tickables[i].Tick();
            }
        }

        public void LateTick()
        {
            int count = _lateTickables.Count;
            for (int i = 0; i < count; i++)
            {
                _lateTickables[i].LateTick();
            }
        }
    }
}
