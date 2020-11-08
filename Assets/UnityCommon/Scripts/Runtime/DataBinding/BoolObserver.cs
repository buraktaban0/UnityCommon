using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommon.DataBinding
{
	public class BoolObserver : DataObserver<BoolReference, BoolVariable, BoolEvent, bool>
	{
		[SerializeField]
		private bool negate = false;

		[SerializeField]
		private UnityEvent onTrue = null;

		[SerializeField]
		private UnityEvent onFalse = null;


		protected override void OnDataModified(bool val)
		{
			val = negate ? !val : val;

			base.OnDataModified(val);

			if (val)
			{
				onTrue.Invoke();
			}
			else
			{
				onFalse.Invoke();
			}
		}
	}
}
