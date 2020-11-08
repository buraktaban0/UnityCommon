using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCommon.Runtime.Utility
{

	public class SceneLoader : MonoBehaviour
	{

		public int sceneToLoad;

		public void Load()
		{
			SceneManager.LoadScene(sceneToLoad);
		}

	}

}
