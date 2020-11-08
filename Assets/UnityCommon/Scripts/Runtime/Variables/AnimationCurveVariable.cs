using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/AnimationCurve Variable", fileName = "New AnimationCurve Variable")]
	public class AnimationCurveVariable : Variable<AnimationCurve>
	{

		public override bool CanBeBoundToPlayerPrefs() => false;


	}

	[Serializable]
	public class AnimationCurveReference : Reference<AnimationCurve, AnimationCurveVariable>
	{

	}



}
