using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Modules;
using UnityEngine;

namespace UnityCommon.Runtime.UI.Animations
{

	public class ScaleAnim : UIAnimation
	{

		[SerializeField] private AnimationCurve curve = null;

		[SerializeField] private float duration = 1f;

		[SerializeField] private Vector3 outScale = default;

		[SerializeField] private bool initiallyVisible = false;

		[SerializeField]
		[HideInInspector]
		private Vector3 inScale = default;

		[SerializeField, HideInInspector]
		private Transform _t = null;

		private Animation<Vector3> anim;


		private void OnValidate()
		{
			if (Application.isPlaying == false)
			{
				_t = transform;
				inScale = _t.localScale;
			}
		}


		private void Awake()
		{

			if (!initiallyVisible)
			{
				_t.localScale = outScale;
			}

		}


		public override void FadeIn()
		{

			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.localScale = val)
				.From(outScale).To(inScale)
				.For(duration)
				.With(new Interpolator(curve));

			anim.Start();

		}

		public override void FadeOut()
		{

			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.localScale = val)
				.From(inScale).To(outScale)
				.For(duration)
				.With(new Interpolator(curve));

			anim.Start();
		}




	}

}
