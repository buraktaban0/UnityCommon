using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEditor;
using UnityEngine;

namespace UnityCommon.Events.Editor
{
	[CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
	public class GameEventEditor : UnityEditor.Editor
	{

		GameEvent gameEvent;

		bool historyFoldout = true;

		private void OnEnable()
		{
			gameEvent = (GameEvent)target;
		}


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.Space(20);

			if (GUILayout.Button("Raise"))
			{
				gameEvent.Invoke(this);
			}

			GUILayout.Space(40);


			if (gameEvent.RaiseHistory != null && gameEvent.RaiseHistory.Count > 0)
			{

				if (GUILayout.Button("Clear History"))
				{
					gameEvent.RaiseHistory.Clear();
					return;
				}

				historyFoldout = EditorGUILayout.Foldout(historyFoldout, "Raise History");

				if (historyFoldout)
				{
					EditorGUI.indentLevel++;
					foreach (var r in gameEvent.RaiseHistory)
					{
						r.foldout = EditorGUILayout.Foldout(r.foldout, r.raiserName);
						//r.foldout = true;
						if (r.foldout)
						{
							EditorGUI.indentLevel++;
							if (r.listeners.Count > 0)
								foreach (var l in r.listeners)
								{
									EditorGUILayout.LabelField(l);
								}
							else
								EditorGUILayout.LabelField("No listeners");
							EditorGUI.indentLevel--;
						}
					}
					EditorGUI.indentLevel--;
				}
			}

			EditorUtility.SetDirty(this);
			EditorUtility.SetDirty(target);
			this.Repaint();

		}

	}
}
