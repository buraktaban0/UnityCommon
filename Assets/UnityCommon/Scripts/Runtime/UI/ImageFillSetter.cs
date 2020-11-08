using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.UI
{
	[RequireComponent(typeof(UnityEngine.UI.Image))]
	public class ImageFillSetter : UIBinder<Image>
	{
		private static readonly float threshold = 1e-3f;

		public enum Mode
		{
			DefaultFill,
			CustomShader
		}


		[SerializeField]
		private FloatReference maxValue = default;

		[SerializeField]
		private float sharpness = 30f;

		[SerializeField]
		private Mode mode = Mode.DefaultFill;

		[SerializeField]
		private string shaderKeyword = "_Fill";

		private bool ShouldShowKeyword() => mode == Mode.CustomShader;

		private float currentVal;

		private float targetVal = 0f;

		private Material mat;

		private int hash;

		private void Awake()
		{
			targetVal = uiElement.fillAmount;
			currentVal = targetVal;

			if (mode == Mode.CustomShader)
			{
				mat = uiElement.material;
				hash = Shader.PropertyToID(shaderKeyword);
			}
		}


		public void SetValue(float val)
		{
			targetVal = Mathf.Clamp01(val / maxValue.Value);

			this.enabled = true;
		}


		public void SetMaxValue(float val)
		{
			maxValue.Value = val;

			SetValue(val);
		}


		private void Update()
		{
			currentVal = Mathf.Lerp(currentVal, targetVal, Time.deltaTime * sharpness);

			if (mode == Mode.DefaultFill)
			{
				uiElement.fillAmount = currentVal;
			}
			else
			{
				mat.SetFloat(hash, currentVal);
			}

			if (Mathf.Abs(currentVal - targetVal) < threshold)
			{
				this.enabled = false;
			}
		}
	}
}
