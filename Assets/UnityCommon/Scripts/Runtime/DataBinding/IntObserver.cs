using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Variables;

namespace UnityCommon.DataBinding
{
	public class IntObserver : DataObserver<IntReference, IntVariable, IntEvent, int>
	{
		public int offset = 0;

		protected override void OnDataModified(int val)
		{
			base.OnDataModified(val + offset);
		}
	}
}
