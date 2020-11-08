using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.Runtime.UI.Animations
{

	[RequireComponent(typeof(Graphic))]
	public class UIAlphaAnim : UIAnimation
	{

		[SerializeField] private AnimationCurve curve = default;

		[SerializeField] private float duration = 1f;

		[SerializeField] private Color outColor = default;

		[SerializeField] private bool unscaledTime = false;

		[SerializeField] private bool initiallyVisible = false;


		[SerializeField]
		[HideInInspector]
		private Color inColor;

		[SerializeField, HideInInspector]
		private Graphic graphic;

		private Animation<Color> anim;


		private void OnValidate()
		{
			if (Application.isPlaying == false)
			{
				graphic = GetComponent<Graphic>();
				inColor = graphic.color;
			}
		}


		private void Awake()
		{

			if (!initiallyVisible)
			{
				graphic.color = outColor;
			}

		}


		public override void FadeIn()
		{

			if (delay.Value < 0f)
			{
				if (anim != null)
					anim.Stop();

				anim = new Animation<Color>(val => graphic.color = val)
					.From(outColor).To(inColor)
					.For(duration)
					.UnscaledTime(unscaledTime)
					.With(new Interpolator(curve));

				anim.Start();
			}
			else
			{
				Conditional.Wait(delay.Value).Do(() =>
				{
					if (anim != null)
						anim.Stop();

					anim = new Animation<Color>(val => graphic.color = val)
						.From(outColor).To(inColor)
						.For(duration)
						.UnscaledTime(unscaledTime)
						.With(new Interpolator(curve));

					anim.Start();
				});
			}

		}

		public override void FadeOut()
		{

			if (anim != null)
				anim.Stop();

			anim = new Animation<Color>(val => graphic.color = val)
				.From(inColor).To(outColor)
				.For(duration)
				.UnscaledTime(unscaledTime)
				.With(new Interpolator(curve));

			anim.Start();
		}




	}

}
