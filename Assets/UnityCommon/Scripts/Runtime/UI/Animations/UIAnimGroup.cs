using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.UI.Animations
{
	public class UIAnimGroup : MonoBehaviour
	{
		[SerializeField]
		private bool onStart = false;

		public List<UIAnimation> animations;

#if UNITY_EDITOR
		private void OnValidate()
		{
			animations = new List<UIAnimation>();

			CacheChildren(transform);
		}

		private void CacheChildren(Transform t)
		{
			var anim = t.GetComponent<UIAnimation>();
			if (anim)
			{
				animations.Add(anim);
			}

			for (int i = 0; i < t.childCount; i++)
			{
				var child = t.GetChild(i);

				if (!child.GetComponent<UIAnimGroup>())
				{
					CacheChildren(child);
				}
			}
		}

#endif

		private void OnEnable()
		{
			if (onStart)
			{
				FadeIn();
			}
		}

		public void Fade(bool e)
		{
			if (e)
			{
				FadeIn();
			}
			else
			{
				FadeOut();
			}
		}

		public void FadeIn()
		{
			for (int i = 0; i < animations.Count; i++)
			{
				animations[i].FadeIn();
			}
		}

		public void FadeOut()
		{
			for (int i = 0; i < animations.Count; i++)
			{
				animations[i].FadeOut();
			}
		}
	}
}
