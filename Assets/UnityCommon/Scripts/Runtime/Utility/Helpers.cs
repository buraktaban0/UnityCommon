using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.Runtime.Utility
{
	public static class Helpers
	{
		public static string SplitCamelCase(this string input)
		{
			return System.Text.RegularExpressions.Regex
			             .Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}

		public static Color Transparent(this Color c)
		{
			c.a = 0f;
			return c;
		}

		public static Color Opaque(this Color c)
		{
			c.a = 1f;
			return c;
		}

		public static Color WithAlpha(this Color c, float a)
		{
			c.a = a;
			return c;
		}

		public static int RoundTo5(this int x)
		{
			if (x < 0)
				return x;

			int r = x % 5;
			return r >= 3 ? x + (5 - r) : x - r;
		}

		public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
		{
			Canvas.ForceUpdateCanvases();
			Vector2 viewportLocalPosition = instance.viewport.localPosition;
			Vector2 childLocalPosition = child.localPosition;
			Vector2 result = new Vector2(
				0 - (viewportLocalPosition.x + childLocalPosition.x),
				0 - (viewportLocalPosition.y + childLocalPosition.y)
			);
			return result;
		}
	}
}
