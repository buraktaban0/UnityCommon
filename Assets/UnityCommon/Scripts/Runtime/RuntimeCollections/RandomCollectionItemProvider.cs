using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.RuntimeCollections;
using UnityEngine;

namespace UnityCommon.Runtime.RuntimeCollections
{
	[CreateAssetMenu(menuName = "Collection Item Provider/Random")]
	public class RandomCollectionItemProvider : ScriptableObject
	{
		private static readonly System.Random rand = new System.Random();

		public RuntimeCollection collection;

		private int currentIndex = 0;

		[HideInInspector]
		[SerializeField]
		private List<int> indices;


		private bool initialized = false;

		private void Init()
		{
			initialized = true;

			currentIndex = 0;

			indices = Enumerable.Range(0, collection.Count).ToList();

			ShuffleIndices();
		}


		private void ShuffleIndices()
		{
			int n = indices.Count;
			while (n > 1)
			{
				n--;
				int k = rand.Next(n + 1);
				int value = indices[k];
				indices[k] = indices[n];
				indices[n] = value;
			}
		}


		private void OnEnable()
		{
			Init();
		}

		private void OnDisable()
		{
			Init();
		}


		public T Get<T>()
		{
			if (!initialized)
				Init();

			var obj = (T) collection.GetAsObject(indices[currentIndex]);
			currentIndex = (currentIndex + 1) % indices.Count;

			if (currentIndex == 0)
			{
				ShuffleIndices();
			}

			return obj;
		}
	}
}
