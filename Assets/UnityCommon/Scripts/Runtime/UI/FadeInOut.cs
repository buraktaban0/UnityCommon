using System;
using UnityCommon.Modules;
using UnityCommon.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.Runtime.UI
{
	public class FadeInOut : SingletonBehaviour<FadeInOut>
	{
		private Canvas canvas;
		private Image image;

		private void Start()
		{
			DontDestroyOnLoad(gameObject);

			SetupCanvas();
		}

		private void SetupCanvas()
		{
			if (canvas != null)
				return;

			if ((canvas = gameObject.GetComponent<Canvas>()) != null)
			{
				// Assuming it's setup in the editor
				canvas.enabled = false;
				image = canvas.GetComponentInChildren<Image>();
				image.color = Color.clear;
				return;
			}

			canvas = gameObject.AddComponent<Canvas>();
			canvas.sortingOrder = 10000;
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;

			canvas.enabled = true;

			var rootRect = canvas.GetComponent<RectTransform>();
			var imageObj = new GameObject("Image");
			imageObj.transform.parent = transform;
			imageObj.transform.localScale = Vector3.one;
			imageObj.transform.localRotation = Quaternion.identity;
			image = imageObj.AddComponent<Image>();
			var rect = image.GetComponent<RectTransform>();

			rect.anchorMin = new Vector2(0, 0);
			rect.anchorMax = new Vector2(1, 1);
			rect.anchoredPosition = new Vector2(0.5f, 0.5f);
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.sizeDelta = rootRect.rect.size;
			image.color = Color.clear;
		}

		public void DoTransition(Action onOut)                 => DoTransition(onOut, 0.7f);
		public void DoTransition(Action onOut, float duration) => DoTransition(onOut, duration, Color.black);

		public void DoTransition(Action onOut, float duration, Color color, float durationOutPercent = 0.6f)
		{
			if (canvas == null)
			{
				SetupCanvas();
			}

			var c0 = color;
			c0.a = 0f;

			canvas.enabled = true;

			float outDuration = duration * durationOutPercent;
			float inDuration = duration * (1f - durationOutPercent);

			var fadeIn = new Animation<Color>(val => image.color = val)
			             .From(color).To(c0)
			             .For(inDuration)
			             .With(Interpolator.Smooth())
			             .OnCompleted(() => { canvas.enabled = false; });

			var fadeOut = new Animation<Color>(val => image.color = val)
			              .From(c0).To(color)
			              .For(outDuration)
			              .With(Interpolator.Accelerate())
			              .OnCompleted(() =>
			              {
				              onOut?.Invoke();
				              Conditional.WaitFrames(3).Do(() => { fadeIn.Start(); });
			              })
			              .Start();
		}
	}
}
