using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.Runtime.UI
{

	[RequireComponent(typeof(Text))]
	public class Typewriter : MonoBehaviour
	{

		[SerializeField]
		private bool debug = false;


		[SerializeField]
		private float charDelay = 0.03f;

		[HideInInspector, SerializeField]
		private Text text;


		//private int index = 0;

		private char[] targetChars;

		private StringBuilder sb;

		private WaitForSeconds wait;

		private void OnValidate()
		{
			text = GetComponent<Text>();
		}


		private void Awake()
		{
			sb = new StringBuilder();

			if (debug)
			{
				SetText(text.text);
			}

		}


		private void OnDisable()
		{
			StopAllCoroutines();
		}

		private IEnumerator Write()
		{

			for (int i = 0; i < targetChars.Length; i++)
			{
				yield return wait;

				sb.Append(targetChars[i]);

				text.text = sb.ToString();
			}

		}

		public void SetText(string text)
		{
			if (text == null)
				throw new Exception("Parameter 'text' cannot be null!");

			sb.Clear();
			//index = 0;
			targetChars = text.ToCharArray();

			wait = new WaitForSeconds(charDelay);

			StopAllCoroutines();

			StartCoroutine(Write());

		}



	}

}
