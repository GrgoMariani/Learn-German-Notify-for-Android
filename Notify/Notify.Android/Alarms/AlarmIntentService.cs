using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Notify.Droid.Interfaces;
using Notify.Models;
using System;
using System.Linq;

namespace Notify.Droid.Alarms
{
    [Service(Name = "com.grimar2008.notify_de.AlarmIntentService", Exported = false, Enabled = true, DirectBootAware = true, Process = ":remote")]
    public class AlarmIntentService : Service
    {
        public event EventHandler NotificationReceived;

        Context context;
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";
        bool channelInitialized;


        public override void OnCreate()
        {
            Android.Util.Log.Verbose("notifyFilter", $"Created"); // adb logcat -s notifyFilter
            base.OnCreate();
        }

        public override IBinder OnBind(Intent intent)
        {
            Android.Util.Log.Verbose("notifyFilter", $"Binded"); // adb logcat -s notifyFilter
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            context = ApplicationContext;

            Bundle bundle = intent.Extras;
            var id = bundle.GetInt("id");

            Android.Util.Log.Verbose("notifyFilter", $"I am here {id}"); // adb logcat -s notifyFilter

            ScheduleNotification(id);
            
            return StartCommandResult.Sticky;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);
        }

        public override void OnDestroy()
        {
            Android.Util.Log.Verbose("notifyFilter", $"Destroyed"); // adb logcat -s notifyFilter
            base.OnDestroy();
        }



        public int ScheduleNotification(int id)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            Intent intent = new Intent(context, typeof(MainActivity));

            var sqliteController = new AndroidSQLite();
            var dbPath = sqliteController.GetPlatformDBPath("translations.db3");
            var connection = sqliteController.GetConnection(dbPath);
            var randomTranslation = connection.Query<ItemTranslation>("SELECT * FROM ItemTranslation ORDER BY RANDOM() LIMIT 1").First();

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, id, intent, PendingIntentFlags.OneShot);

            var notification = AndroidNotificationManager.BuildNotification(context, pendingIntent, randomTranslation.German, randomTranslation.English);

            StartForeground(id, notification);

            ReceiveNotification(randomTranslation.German, randomTranslation.English);

            // reschedule alarm here
            var alarmStorage = new AlarmStorage(context);
            var alarm = alarmStorage.GetAlarmById(id);
            var util = new AlarmUtil(context);
            if (alarm != null)
            {
                util.ScheduleAlarm(alarm);
                Android.Util.Log.Verbose("notifyFilter", $"Rescheduled {alarm.Id}"); // adb logcat -s notifyFilter
            }

            StopForeground(false);

            return id;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
            // Save result to history db
            var itemTranslation = new ItemTranslation()
            {
                German = title,
                English = message
            };

            var sqliteController = new AndroidSQLite();
            var dbPath = sqliteController.GetPlatformDBPath("history.db3");
            var connection = sqliteController.GetConnection(dbPath);
            connection.CreateTable<ItemTranslation>();
            connection.Insert(itemTranslation);
        }

        void CreateNotificationChannel()
        {
            var manager = context.GetSystemService(NotificationService).JavaCast<NotificationManager>(); ;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
    }

}

