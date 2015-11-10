using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aegis
{
	public class PrefabCache
	{        
		private Dictionary<string, GameObject> cache = new Dictionary<string, GameObject>();
		
		public GameObject Instantiate(string prefabName)
		{
			GameObject prefab;
			if (cache.ContainsKey(prefabName))
			{
				prefab = cache[prefabName];
			}
			else
			{
				//Load it and cache it
				prefab = Resources.Load<GameObject>(prefabName);
                Add(prefab);
			}
			
			return GameObject.Instantiate(prefab);
		}
		
		public T Create<T>(string prefabName)
		{
			GameObject obj = Instantiate(prefabName);
			T script = obj.GetComponent<T>();
			if (script == null)
				throw new MissingComponentException("The prefab " + prefabName + " does not contain the component " + typeof(T).ToString());
			return script;
		}

        public void Add(PrefabEntry prefab)
        {
            if (cache.ContainsKey(prefab.name))
                throw new InvalidOperationException("The prefab name '" + prefab.name + "' already exists");

            cache[prefab.name] = prefab.prefab;
        }

        public void Add(GameObject prefab)
        {
            string name = prefab.name;

            if (cache.ContainsKey(name))
                throw new InvalidOperationException("The prefab name '" + name + "' already exists");

            cache[name] = prefab;
        }

        public void AddAll(IEnumerable<PrefabEntry> prefabs)
        {
            foreach (PrefabEntry entry in prefabs)
            {
                Add(entry);
            }
        }

        public void AddAll(IEnumerable<GameObject> prefabs)
        {
            foreach (GameObject prefab in prefabs)
                Add(prefab);
        }
        
        [Serializable]
        public class PrefabEntry
        {
            public PrefabEntry()
            { }

            public PrefabEntry(string name, GameObject prefab)
            {
                this.name = name;
                this.prefab = prefab;
            }

            public string name;
            public GameObject prefab;
        }
    }
}