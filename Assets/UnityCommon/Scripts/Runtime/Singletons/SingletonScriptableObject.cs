using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Singletons
{
	public abstract class SingletonScriptableObject : ScriptableObject
	{
		public static T GetInstance<T>() where T : ScriptableObject
		{
			return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
		}

	}

	public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
	{

		private static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

					
					if (instance == null)
					{
						throw new MissingReferenceException($"No instance of type {typeof(T)} is present in the project.");
					}
					

					//DontDestroyOnLoad(instance);
				}

				return instance;

			}
			protected set
			{
				instance = value;
			}
		}

		public static T Find(string name)
		{
			return Resources.FindObjectsOfTypeAll<T>().Where(obj => obj.name == name).FirstOrDefault();
		}

	}
}
