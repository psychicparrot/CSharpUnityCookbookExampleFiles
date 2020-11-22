using UnityEngine;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Common/Timer class")]

	public class TimerClass
	{
		public bool isTimerRunning = false;
		private float timeElapsed = 0.0f;
		private float currentTime = 0.0f;
		private float lastTime = 0.0f; 
		private float timeScaleFactor = 1.0f; // <-- If you need to scale time, change this!
		private string timeString;
		private string hour;
		private string minutes;
		private string seconds;
		private string mills;
		private int aHour;
		private int aMinute;
		private int aSecond;
		private int aMillis;
		private int tmp;

		public void UpdateTimer()
		{
			// calculate the time elapsed since the last Update()
			timeElapsed = Mathf.Abs(Time.realtimeSinceStartup - lastTime);

			// if the timer is running, we add the time elapsed to the current time (advancing the timer)
			if (isTimerRunning)
			{
				currentTime += timeElapsed * timeScaleFactor;
			}

			// store the current time so that we can use it on the next update
			lastTime = Time.realtimeSinceStartup;
		}

		public void StartTimer()
		{
			// set up initial variables to start the timer
			isTimerRunning = true;
			lastTime = Time.realtimeSinceStartup;
		}

		public void StopTimer()
		{
			// stop the timer
			isTimerRunning = false;

			// carry out an update to the timer
			UpdateTimer();
		}

		public void ResetTimer()
		{
			// resetTimer will set the timer back to zero
			timeElapsed = 0.0f;
			currentTime = 0.0f;
			lastTime = Time.realtimeSinceStartup;

			// carry out an update to the timer
			UpdateTimer();
		}

		public string GetFormattedTime()
		{
			// carry out an update to the timer so it is 'up to date'
			UpdateTimer();

			// grab hours
			aHour = (int)currentTime / 3600;
			aHour = aHour % 24;

			// grab minutes
			aMinute = (int)currentTime / 60;
			aMinute = aMinute % 60;

			// grab seconds
			aSecond = (int)currentTime % 60;

			// grab milliseconds
			aMillis = (int)(currentTime * 100) % 100;

			// format strings for individual mm/ss/mills
			tmp = (int)aSecond;
			seconds = tmp.ToString("D2"); // ToString() formats .. in this case, D followed by how many numbers 

			tmp = (int)aMinute;
			minutes = tmp.ToString("D2");

			tmp = (int)aHour;
			hour = tmp.ToString("D2");

			tmp = (int)aMillis;
			mills = tmp.ToString("D2");

			// pull together a formatted string to return
			timeString = hour + ":" + minutes + ":" + seconds + ":" + mills;

			return timeString;
		}

		public int GetTime()
		{
			// remember to call UpdateTimer() before trying to use this function, otherwise the time value will
			// not be up to date
			return (int)(currentTime);
		}
	}
}