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

	public class FloatConditionalInvoke : MonoBehaviour
	{

		public enum Condition
		{
			LessThan,
			GreaterThan
		}


		[SerializeField] private Condition condition = Condition.GreaterThan;

		[SerializeField] private FloatReference threshold = null;

		[SerializeField] private bool continuous = false;
		[SerializeField] private bool invokeElse = false;


		[SerializeField] private FloatEvent action = null;
		[SerializeField] private FloatEvent elseAction = null;


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


		public void OnValueChanged(int value)
		{
			OnValueChanged((float)value);
		}

		public void OnValueChanged(float value)
		{
			lastVal = value;

			if (!shouldInvoke)
				return;

			var b = (condition == Condition.LessThan && value < threshold.Value) || (condition == Condition.GreaterThan && value > threshold.Value);

			if (b)
			{
				OnInvoked();
				action.Invoke(value);
				return;
			}
			else if (invokeElse)
			{
				OnInvoked();
				elseAction.Invoke(value);
			}


		}


	}

}
