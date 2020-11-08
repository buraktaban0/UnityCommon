using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Variables;
using UnityEngine;

namespace UnityCommon.Runtime.Animators
{


	[CreateAssetMenu(menuName = "Variables/Animator Parameter Hash", fileName = "Hash")]
	public class AnimatorParameterHash : Variable<string>
	{

		[SerializeField] private int hash;
		public int Hash { get => hash; private set => hash = value; }

		private bool isUsedOnce = false;

		private bool boolValue;

		public override bool CanBeBoundToPlayerPrefs() => false;

		public override void OnInspectorChanged()
		{
			base.OnInspectorChanged();

			if (string.IsNullOrEmpty(this.value))
				return;


			Hash = Animator.StringToHash(this.value);


		}


		public void Reset()
		{
			isUsedOnce = false;
		}


		public void SetTrigger(Animator animator)
		{
			animator.SetTrigger(this.hash);
		}

		public void SetBool(Animator animator, bool value)
		{

			if (isUsedOnce)
			{
				if (boolValue == value)
				{
					return;
				}
			}

			animator.SetBool(this.hash, value);

			boolValue = value;

			isUsedOnce = true;
		}


	}

}
