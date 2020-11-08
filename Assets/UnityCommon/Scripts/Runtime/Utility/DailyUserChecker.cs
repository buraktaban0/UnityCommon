using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{

	[DefaultExecutionOrder(-1500)]
	public class DailyUserChecker : MonoBehaviour
	{

		public bool overrideData = false;

		public int repeatingDays = 0;
		public bool newDay = false;

		public int RepeatingDays
		{
			get
			{
				return PlayerPrefs.GetInt("__REPEATING_DAYS__", -1);
			}
			private set
			{
				PlayerPrefs.SetInt("__REPEATING_DAYS__", value);
				PlayerPrefs.Save();
			}
		}


		public bool NewDay { get; private set; }


		public void Awake()
		{
			if (overrideData)
			{
				Debug.Log("Override");
				RepeatingDays = repeatingDays;
				NewDay = newDay;
				return;
			}

			var datum = new DateTime(2020, 2, 2);


			int today = (DateTime.Now - datum).Days;

			int lastDay = PlayerPrefs.GetInt("__LAST_DAY__", -1);


			if (lastDay < 0)
			{
				PlayerPrefs.SetInt("__LAST_DAY__", today);
				RepeatingDays = 1;
				NewDay = true;
				return;
			}

			if (lastDay == today)
			{
				NewDay = false;
				return;
			}

			if (lastDay + 1 == today)
			{
				RepeatingDays += 1;
				NewDay = true;

			}
			else if (lastDay + 1 < today)
			{
				// missed days;
				RepeatingDays = 0;
				NewDay = true;
			}

			PlayerPrefs.SetInt("__LAST_DAY__", today);
			PlayerPrefs.Save();

		}


	}

}
