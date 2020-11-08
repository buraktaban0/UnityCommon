using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.Rendering
{

	public static class TextureUtility
	{


		public static Texture2D Copy(this Texture2D original)
		{
			var copy = new Texture2D(original.width, original.height, TextureFormat.ARGB32, false);


			copy.SetPixels(original.GetPixels());
			copy.Apply(false);

			return copy;

		}


		public static void ReadRenderTexture(RenderTexture source, Texture2D target)
		{
			var temp = RenderTexture.active;
			RenderTexture.active = source;

			target.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0, false);

			RenderTexture.active = temp;
		}




	}

}
