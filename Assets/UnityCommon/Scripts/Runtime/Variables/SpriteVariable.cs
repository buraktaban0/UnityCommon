using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Sprite Variable", fileName = "New Sprite Variable")]
	public class SpriteVariable : Variable<Sprite>
	{

		public override bool CanBeBoundToPlayerPrefs()
		{
			return false;
		}

	}

	[Serializable]
	public class SpriteReference : Reference<Sprite, SpriteVariable>
	{

	}



}
