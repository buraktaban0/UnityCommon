using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityCommon.Variables
{
	[CreateAssetMenu(menuName = "Variables/Animator Variable", fileName = "New Animator Variable")]
	public class AnimatorVariable : Variable<Animator>
	{
		public override bool CanBeBoundToPlayerPrefs() => false;
	}

	[Serializable]
	public class AnimatorReference : Reference<Animator, AnimatorVariable>
	{
	}
}
