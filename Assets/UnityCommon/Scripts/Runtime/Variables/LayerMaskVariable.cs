using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/LayerMask Variable", fileName = "New LayerMask Variable")]
	public class LayerMaskVariable : Variable<LayerMask>
	{

		public override bool CanBeBoundToPlayerPrefs() => false;

	}

	[Serializable]
	public class LayerMaskReference : Reference<LayerMask, LayerMaskVariable>
	{

	}



}
