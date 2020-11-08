using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Singletons;
using UnityEngine;

namespace UnityCommon.Modules
{
	public abstract class Module<T> : SingletonBehaviour<T> where T : Module<T>
	{
		private void Awake()
		{
			if (!SetupInstance())
				return;
		}

		public abstract void OnEnable();

		public abstract void OnDisable();

		public abstract void Update();
		public abstract void LateUpdate();
	}
}
