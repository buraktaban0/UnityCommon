using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{

	[RequireComponent(typeof(AudioSource))]
	public class AudioSourceFader : MonoBehaviour
	{

		[SerializeField] private AudioSource source;

		[SerializeField] private float sharpness = 20f;

		private float targetVolume = 0f;

		private float volume;

		private void OnValidate()
		{
			source = GetComponent<AudioSource>();
		}

		private void Awake()
		{
			targetVolume = source.volume;
			volume = targetVolume;

			this.enabled = false;
		}


		public void SetVolume(float val)
		{
			targetVolume = val;
			this.enabled = true;
		}


		private void Update()
		{
			volume = Mathf.Lerp(volume, targetVolume, Time.deltaTime * sharpness);

			if (Mathf.Abs(volume - targetVolume) < 1e-4)
			{
				volume = targetVolume;

				this.enabled = false;
			}

			source.volume = volume;

		}

	}

}
