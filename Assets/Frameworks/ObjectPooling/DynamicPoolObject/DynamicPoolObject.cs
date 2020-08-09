using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPoolObject : MonoBehaviour {

	private DynamicPoolManager _dynamicPoolManager;

	private string objectId = "";

	public string poolObjectId{ 
		get{
			if (string.IsNullOrEmpty (objectId)) {
				objectId = gameObject.name.Replace ("(Clone)", "");
			}
			return objectId;
		}
		set{
			objectId = value;
		}
	}

	public void DespawnPoolObject(){
		_dynamicPoolManager.DespawnObject (this);
	}

    public class Pool : MemoryPool<DynamicPoolObject>
    {
        public Pool(MemoryPoolSettings _memoryPoolSettings, FactoryBase<DynamicPoolObject> _factory) : base(_memoryPoolSettings, _factory)
        {
        }

		protected override void OnCreated (DynamicPoolObject item)
		{

		}

		protected override void OnSpawned (DynamicPoolObject item)
		{
            base.OnSpawned(item);
			if (item.GetComponent<DynamicPoolObjectDespawner> () != null) {
				item.GetComponent<DynamicPoolObjectDespawner> ().Init ();
			}
		}

		protected override void OnDespawned (DynamicPoolObject item)
		{

		}

	}

    public class Factory : FactoryBase<DynamicPoolObject>
    {
		protected DynamicPoolManager _dpm;

		public Factory(
			GameObject prefab,
            DynamicPoolManager dpm) : base(prefab)
		{
			_dpm = dpm;
		}

        public override void OnCreated(DynamicPoolObject obj)
        {
            obj.poolObjectId = _prefab.name;
            obj._dynamicPoolManager = this._dpm;
        }

        public override void OnDespawned(DynamicPoolObject obj)
        {

        }
    }
}