using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Long Variable", fileName = "New Long Variable")]
	public class LongVariable : Variable<long>
	{

		public override string Serialize()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		public override void Deserialize(string s)
		{
			value = long.Parse(s, CultureInfo.InvariantCulture);
		}

	}

	[Serializable]
	public class LongReference : Reference<long, LongVariable>
	{

	}



}
