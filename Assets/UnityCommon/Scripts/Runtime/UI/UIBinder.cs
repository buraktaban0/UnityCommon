using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCommon.Events;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityCommon.UI
{

	public abstract class UIBinder<TElement> : MonoBehaviour where TElement : UIBehaviour
	{

		[SerializeField] protected TElement uiElement;



		protected virtual void OnValidate()
		{

			if (uiElement == null)
			{
				uiElement = this.GetComponent<TElement>();
			}

		}

	}
}
