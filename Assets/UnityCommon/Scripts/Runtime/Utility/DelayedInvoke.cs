using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Modules;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommon.Runtime.Utility
{
	public class DelayedInvoke : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent onExecute = null;

		[SerializeField]
		private FloatReference delay = default;

		[SerializeField]
		private bool onAwake = true;

		private void Awake()
		{
			if (onAwake)
			{
				Invoke();
			}
		}

		public void Invoke()
		{
			if (delay.Value < 0.001f)
			{
				onExecute.Invoke();
			}
			else
			{
				if (gameObject.activeSelf)
				{
					StartCoroutine(WaitAndExecute());
				}
				else
				{
					Conditional.Wait(delay.Value).Do(() => { onExecute?.Invoke(); });
				}
			}
		}


		private IEnumerator WaitAndExecute()
		{
			yield return new WaitForSeconds(delay.Value);


			onExecute.Invoke();
		}
	}
}
