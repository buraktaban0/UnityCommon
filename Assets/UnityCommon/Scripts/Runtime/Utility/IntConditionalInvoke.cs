using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.DataBinding;
using UnityCommon.Variables;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	public class IntConditionalInvoke : MonoBehaviour
	{
		public enum Condition
		{
			LessThan,
			GreaterThan
		}


		[SerializeField]
		private Condition condition = Condition.GreaterThan;

		[SerializeField]
		private IntReference threshold = default;

		[SerializeField]
		private bool continuous = false;

		[SerializeField]
		private bool invokeElse = false;


		[SerializeField]
		private IntEvent action = null;

		[SerializeField]
		private IntEvent elseAction = null;


		private bool shouldInvoke = true;

		private float lastVal;

		private void OnInvoked()
		{
			if (continuous)
			{
			}
			else
			{
				shouldInvoke = false;
			}
		}


		public void OnValueChanged(float value)
		{
			OnValueChanged((float) value);
		}

		public void OnValueChanged(int value)
		{
			lastVal = value;

			if (!shouldInvoke)
				return;

			var b = (condition == Condition.LessThan && value < threshold.Value) ||
			        (condition == Condition.GreaterThan && value > threshold.Value);

			if (b)
			{
				OnInvoked();
				action.Invoke(value);
			}
			else if (invokeElse)
			{
				Debug.Log("Else " + gameObject.name);
				OnInvoked();
				elseAction.Invoke(value);
			}
		}
	}
}
