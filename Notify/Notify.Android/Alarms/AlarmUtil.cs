using System;
using Android.App;
using Android.Content;
using Android.Runtime;

namespace Notify.Droid.Alarms
{
	public class AlarmUtil
	{
		Context context;
		AlarmManager alarmManager;

		public AlarmUtil(Context context)
		{
			this.context = context;
			alarmManager = context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();
		}

		/// <summary>
		/// Schedules an alarm.
		/// </summary>
		/// <param name="alarm">The alarm to be scheduled.</param>
		public int ScheduleAlarm(Alarm alarm)
		{
			var scheduleTime = GetNextAlarmTime(alarm.Hour, alarm.Minute);
			var utcTime = TimeZoneInfo.ConvertTimeToUtc(scheduleTime);
			var epochDif = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
			var notifyTimeInInMilliseconds = utcTime.AddSeconds(-epochDif).Ticks / 10000;

			var intent = new Intent(context, typeof(AlarmIntentService));

			intent.PutExtra("id", alarm.Id);
			intent.PutExtra("year", alarm.Year);
			intent.PutExtra("month", alarm.Month);
			intent.PutExtra("day", alarm.Day);
			intent.PutExtra("hour", alarm.Hour);
			intent.PutExtra("minute", alarm.Minute);
			intent.PutExtra("difficulty", alarm.Difficulty);

			Android.Util.Log.Verbose("notifyFilter", $"register {alarm.Id}"); // adb logcat -s notifyFilter

			PendingIntent pendingIntent;
			
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
			{
				pendingIntent = PendingIntent.GetForegroundService(context, alarm.Id, intent, PendingIntentFlags.UpdateCurrent);
			}
			else
			{
				pendingIntent = PendingIntent.GetService(context, alarm.Id, intent, PendingIntentFlags.UpdateCurrent);
			}

			// Set inexact repeating
			alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, notifyTimeInInMilliseconds, pendingIntent);
			//alarmManager.SetRepeating(AlarmType.RtcWakeup, notifyTimeInInMilliseconds, AlarmManager.IntervalDay, pendingIntent);
			return alarm.Id;
		}

		/// <summary>
		/// Cancels the scheduled alarm.
		/// </summary>
		/// <returns>The alarm to be canceled.</returns>
		/// <param name="alarm">Alarm.</param>
		public void CancelAlarm(Alarm alarm)
		{
			var intent = new Intent(context, typeof(AlarmIntentService));
			PendingIntent pendingIntent;

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
			{
				pendingIntent = PendingIntent.GetForegroundService(context, alarm.Id, intent, PendingIntentFlags.UpdateCurrent);
			}
			else
			{
				pendingIntent = PendingIntent.GetService(context, alarm.Id, intent, PendingIntentFlags.UpdateCurrent);
			}

			alarmManager.Cancel(pendingIntent);
		}

		/// <summary>
		/// Returns the alarm time closest to the hour and minute specified.
		/// </summary>
		/// <returns>The time the alarm should trigger.</returns>
		/// <param name="hour">The hour the alarm should trigger.</param>
		/// <param name="minute">the minute the alarm should trigger.</param>
		public DateTime GetNextAlarmTime(int hour, int minute)
		{
			var now = DateTime.Now;
			var alarmTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
			if (alarmTime < now)
				alarmTime = alarmTime.AddDays(1);

			return alarmTime;
		}
	}

}

