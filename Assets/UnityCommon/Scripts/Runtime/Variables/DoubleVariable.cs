using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Double Variable", fileName = "New Double Variable")]
	public class DoubleVariable : Variable<double>
	{

		public override string Serialize()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		public override void Deserialize(string s)
		{
			value = double.Parse(s, CultureInfo.InvariantCulture);
		}

	}

	[Serializable]
	public class DoubleReference : Reference<double, DoubleVariable>
	{

	}



}
