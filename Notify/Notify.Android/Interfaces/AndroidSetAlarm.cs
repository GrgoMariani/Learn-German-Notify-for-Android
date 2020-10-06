using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Notify.Droid.Alarms;
using Notify.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(Notify.Droid.Interfaces.AndroidSetAlarm))]
namespace Notify.Droid.Interfaces
{
    public class AndroidSetAlarm : ISetAlarm
    {

        public AndroidSetAlarm() { }


        public int SetAlarm(int hour, int minute, string difficulty)
        {
            var context = Android.App.Application.Context;

            var util = new AlarmUtil(context);
            var alarmStorage = new AlarmStorage(context);
            var alarm = alarmStorage.SaveAlarm(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, difficulty);
            return util.ScheduleAlarm(alarm);
        }

        public void CancelAlarm(int id)
        {
            var alarmStorage = new AlarmStorage(Android.App.Application.Context);
            var toBeDeleted = new Alarm()
            {
                Id = id
            };
            alarmStorage.DeleteAlarm(toBeDeleted);
        }
    }
}