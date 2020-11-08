using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	public static class WeightedRandom
	{
		public static T Get<T>(T[] values, float[] weights)
		{
			if (values.Length != weights.Length)
				throw new Exception("Length of values and weights should be equal.");

			float sum = 0.0f;


			for (int i = 0; i < weights.Length; i++)
			{
				float w = weights[i];
				weights[i] += sum;
				sum += w;
			}

			float rand = UnityEngine.Random.Range(0f, sum);

			for (int i = 0; i < weights.Length; i++)
			{
				if (rand < weights[i])
				{
					return values[i];
				}
			}

			throw new Exception(
				"Could not select weighted random value: " + rand + " - " + sum + " - " + weights.Last());
		}
	}
}
