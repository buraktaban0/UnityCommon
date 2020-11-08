using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCommon.RuntimeCollections
{
	public abstract class RuntimeCollection : ScriptableObject
	{
		public abstract int Count { get; }

		public abstract object GetAsObject(int index);
	}

	public abstract class RuntimeCollection<T> : RuntimeCollection, IEnumerable<T>
	{
		[SerializeField]
		private bool allowDuplicates = false;

		public bool AllowDuplicates
		{
			get => allowDuplicates;
			set => allowDuplicates = value;
		}

		[SerializeField]
		private bool clearOnPlay = false;

		public bool ClearOnPlay
		{
			get => clearOnPlay;
			set => clearOnPlay = value;
		}


		//public GameEvent onModified;

		//public TEvent onItemAdded, onItemRemoved;

		public List<T> items = new List<T>();


		public override int Count
		{
			get => items.Count;
		}

		private void OnDisable()
		{
			if (clearOnPlay)
				items = new List<T>();
		}

		public T this[int i]
		{
			get => items[i];
			set => items[i] = value;
		}


		public override object GetAsObject(int index)
		{
			return items[index];
		}

		public void Clear()
		{
			items.Clear();
		}


		public void AddRange(IEnumerable<T> enumerable)
		{
			items.AddRange(enumerable);
		}

		public void Add(T item)
		{
			if (allowDuplicates || !items.Contains(item))
			{
				items.Add(item);

				/*onModified?.Raise(this);
				onItemAdded?.Raise(this, item);*/
			}
		}

		public T GetRandom()
		{
			return items.OrderBy(item => UnityEngine.Random.Range(0, 99999)).First();
		}

		public void Remove(T item)
		{
			if (items.Contains(item))
			{
				items.Remove(item);

				/*onModified?.Raise(this);
				onItemRemoved?.Raise(this, item);*/
			}
		}


		public void Shuffle()
		{
			int n = Count;
			var rand = new System.Random();
			while (n > 1)
			{
				n--;
				int k = rand.Next(n + 1);
				T value = this[k];
				this[k] = this[n];
				this[n] = value;
			}
		}


		public int IndexOf(T item)
		{
			return items.IndexOf(item);
		}


		public IEnumerator<T> GetEnumerator()
		{
			foreach (var item in items)
			{
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
