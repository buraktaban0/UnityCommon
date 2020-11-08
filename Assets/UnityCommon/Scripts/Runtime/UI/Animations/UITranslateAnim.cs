using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Modules;
using UnityEngine;

namespace UnityCommon.Runtime.UI.Animations
{
	public class UITranslateAnim : UIAnimation
	{
		[SerializeField]
		private AnimationCurve curve = null;

		[SerializeField]
		private float duration = 1f;

		[SerializeField]
		private Vector3 offset = default;

		[SerializeField]
		private bool initiallyVisible = false;

		[SerializeField]
		[HideInInspector]
		private Vector3 inPos = default;

		[SerializeField, HideInInspector]
		private RectTransform _t = null;

		private Animation<Vector3> anim = null;


		private Conditional cond;

		private void OnValidate()
		{
			if (Application.isPlaying == false)
			{
				_t = GetComponent<RectTransform>();
				inPos = _t.anchoredPosition;
			}
		}


		private void Awake()
		{
			if (!initiallyVisible)
			{
				_t.anchoredPosition = inPos + offset;
			}
		}


		public void Cancel()
		{
			cond?.Cancel();
		}


		public override void FadeIn()
		{
			if (delay.Value < 0)
			{
				if (anim != null)
					anim.Stop();

				anim = new Animation<Vector3>(val => _t.anchoredPosition = val)
				       .From(_t.anchoredPosition).To(inPos)
				       .For(duration)
				       .With(new Interpolator(curve));

				anim.Start();
			}
			else
			{
				cond?.Cancel();
				cond = Conditional.Wait(delay.Value).Do(() =>
				{
					if (anim != null)
						anim.Stop();

					anim = new Animation<Vector3>(val => _t.anchoredPosition = val)
					       .From(_t.anchoredPosition).To(inPos)
					       .For(duration)
					       .With(new Interpolator(curve));

					anim.Start();
				});
			}
		}

		public override void FadeOut()
		{
			cond?.Cancel();

			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.anchoredPosition = val)
			       .From(_t.anchoredPosition).To(inPos + offset)
			       .For(duration)
			       .With(new Interpolator(curve));

			anim.Start();
		}
	}
}
