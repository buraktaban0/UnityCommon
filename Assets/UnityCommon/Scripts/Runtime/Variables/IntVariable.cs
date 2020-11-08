using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Int Variable", fileName = "New Int Variable")]
	public class IntVariable : Variable<int>
	{

		public override string Serialize()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		public override void Deserialize(string s)
		{
			value = int.Parse(s, CultureInfo.InvariantCulture);
		}



		public void Add(int value)
		{
			Value += value;
		}
		public void Add(IntVariable var)
		{
			Value += var.Value;
		}

		public void Multiply(int value)
		{
			Value *= value;
		}
		public void Multiply(IntVariable var)
		{
			Value *= var.Value;
		}


		public void Negate() { Value = -Value; }

	}

	[Serializable]
	public class IntReference : Reference<int, IntVariable>
	{

	}



}
