using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace nityCommon.Runtime.Utility
{

	public class UnityCallbackEvents : MonoBehaviour
	{

		public UnityEvent onAwake;
		public UnityEvent onEnable;
		public UnityEvent onDisable;
		public UnityEvent onUpdate;
		public UnityEvent onDestroy;

		private void Awake()
		{
			onAwake.Invoke();
		}

		private void OnEnable()
		{
			onEnable.Invoke();
		}

		private void OnDisable()
		{
			onDisable.Invoke();
		}

		private void Update()
		{
			onUpdate.Invoke();
		}

		private void OnDestroy()
		{
			onDestroy.Invoke();
		}

	}

}
