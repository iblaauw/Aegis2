using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aegis
{
	public class PrefabCache
	{
		private static PrefabCache instance = new PrefabCache();
		
		public static PrefabCache Instance { get { return instance; } }
		
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
				cache[prefabName] = prefab;
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
	}
}