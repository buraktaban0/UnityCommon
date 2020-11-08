using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Bool Variable", fileName = "New Bool Variable")]
	public class BoolVariable : Variable<bool>
	{

		public override string Serialize()
		{
			return Value ? "1" : "0";
		}

		public override void Deserialize(string s)
		{
			value = (s == "1");
		}


		public void Negate()
		{
			Value = !Value;
		}

	}

	[Serializable]
	public class BoolReference : Reference<bool, BoolVariable>
	{

	}



}
