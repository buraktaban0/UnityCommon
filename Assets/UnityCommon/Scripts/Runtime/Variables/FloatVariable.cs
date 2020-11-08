using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Float Variable", fileName = "New Float Variable")]
	public class FloatVariable : Variable<float>
	{

		public override string Serialize()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		public override void Deserialize(string s)
		{
			value = float.Parse(s, CultureInfo.InvariantCulture);
		}


		public void Add(float value)
		{
			Value += value;
		}
		public void Add(FloatVariable var)
		{
			Value += var.Value;
		}

		public void Multiply(float value)
		{
			Value *= value;
		}
		public void Multiply(FloatVariable var)
		{
			Value *= var.Value;
		}


		public void Negate() { Value = -Value; }

	}

	[Serializable]
	public class FloatReference : Reference<float, FloatVariable>
	{

	}



}
