using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/String Variable", fileName = "New String Variable")]
	public class StringVariable : Variable<string>
	{

		public override string Serialize()
		{
			return Value;
		}

		public override void Deserialize(string s)
		{
			value = s;
		}

	}

	[Serializable]
	public class StringReference : Reference<string, StringVariable>
	{

	}



}
