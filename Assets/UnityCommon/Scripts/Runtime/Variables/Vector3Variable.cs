using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Vector3 Variable", fileName = "New Vector3 Variable")]
	public class Vector3Variable : Variable<Vector3>
	{
		public override string Serialize()
		{
			// Packing Vector3 value with '&'s between x, y and z:
			return string.Format("{0}&{1}&{2}",
			                     Value.x.ToString(CultureInfo.InvariantCulture),
			                     Value.y.ToString(CultureInfo.InvariantCulture),
			                     Value.z.ToString(CultureInfo.InvariantCulture)
			);
		}

		public override void Deserialize(string s)
		{
			var split = s.Split('&');
			value = new Vector3(
				float.Parse(split[0], CultureInfo.InvariantCulture),
				float.Parse(split[1], CultureInfo.InvariantCulture),
				float.Parse(split[2], CultureInfo.InvariantCulture)
			);
		}
	}

	[Serializable]
	public class Vector3Reference : Reference<Vector3, Vector3Variable>
	{
	}
}
