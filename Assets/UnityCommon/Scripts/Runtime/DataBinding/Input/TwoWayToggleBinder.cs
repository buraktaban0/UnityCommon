using UnityCommon.DataBinding;
using UnityEngine.UI;

namespace UnityCommon.Runtime.DataBinding
{
	public class TwoWayToggleBinder : BoolObserver
	{
		
		public Toggle toggle;

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();

			toggle = GetComponent<Toggle>();
		}
#endif

		protected override void Start()
		{
			base.Start();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			toggle.onValueChanged.AddListener(OnToggleInput);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			toggle.onValueChanged.RemoveListener(OnToggleInput);
		}


		private void OnToggleInput(bool val)
		{
			data.Value = val;
		}

		protected override void OnDataModified(bool val)
		{
			base.OnDataModified(val);

			toggle.SetIsOnWithoutNotify(val);
		}
	}
}
