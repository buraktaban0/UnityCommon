using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Color Variable", fileName = "New Color Variable")]
	public class ColorVariable : Variable<Color>
	{
		public override string Serialize()
		{
			// Packing Color value with '&'s between r, g and b:
			return string.Format("{0}&{1}&{2}&{3}",
				Value.r.ToString(CultureInfo.InvariantCulture),
				Value.g.ToString(CultureInfo.InvariantCulture),
				Value.b.ToString(CultureInfo.InvariantCulture),
				Value.a.ToString(CultureInfo.InvariantCulture)
				);
		}

		public override void Deserialize(string s)
		{
			var split = s.Split('&');

			value = new Color(
				float.Parse(split[0], CultureInfo.InvariantCulture),
				float.Parse(split[1], CultureInfo.InvariantCulture),
				float.Parse(split[2], CultureInfo.InvariantCulture),
				float.Parse(split[3], CultureInfo.InvariantCulture)
				);
		}

		public void Multiply(Color color)
		{
			Value *= color;
		}

		public void Add(Color color)
		{
			Value += color;
		}


	}

	[Serializable]
	public class ColorReference : Reference<Color, ColorVariable>
	{

	}



}
