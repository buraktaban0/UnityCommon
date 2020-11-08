using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	public class ParticleSystemController : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem[] systems;

		private List<float> defaultEmissions;

		private bool isPlaying;

		public Color color
		{
			get { return systems[0].main.startColor.color; }
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			systems = GetComponentsInChildren<ParticleSystem>();
		}
#endif

		private void Awake()
		{
			isPlaying = systems.First().emission.enabled;

			if (defaultEmissions == null)
				defaultEmissions = systems.Select(s => s.emission.rateOverTimeMultiplier).ToList();
		}

		public void Play()
		{
			isPlaying = true;

			foreach (var system in systems)
			{
				system.Play();
				var emission = system.emission;
				emission.enabled = true;
			}
		}

		public void Pause()
		{
			isPlaying = false;

			foreach (var system in systems)
			{
				system.Play();
				var emission = system.emission;
				emission.enabled = false;
			}
		}

		public void SetEmissionMultiplier(float speed)
		{
			if (defaultEmissions == null)
				defaultEmissions = systems.Select(s => s.emission.rateOverTimeMultiplier).ToList();

			for (var index = 0; index < systems.Length; index++)
			{
				var system = systems[index];
				if (system.isPlaying == false)
					system.Play();

				var emission = system.emission;
				emission.rateOverTimeMultiplier = defaultEmissions[index] * speed;
			}
		}

		public void Stop()
		{
			foreach (var system in systems)
			{
				system.Stop();
			}
		}
	}
}
