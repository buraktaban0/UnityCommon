using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Variables;
using UnityEngine;

namespace UnityCommon.Events
{
	public class GameEventRaiser : MonoBehaviour
	{

		public GameEvent gameEvent;

		public FloatReference delay;

		public void Raise()
		{
			if (delay.Value < Mathf.Epsilon)
			{
				RaiseNow();
			}
			else
			{
				StartCoroutine(WaitAndRaise());
			}
		}


		public void RaiseNow()
		{
			gameEvent.Invoke(this);
		}


		private IEnumerator WaitAndRaise()
		{
			yield return new WaitForSecondsRealtime(delay.Value);

			RaiseNow();
		}


	}
}
