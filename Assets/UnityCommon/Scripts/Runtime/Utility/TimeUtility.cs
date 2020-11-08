using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityCommon.Utilities
{

	public static class TimeUtility
	{
		public static long UnixTimestamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

	}

}
