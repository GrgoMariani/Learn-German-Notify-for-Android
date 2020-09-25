using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Notify.Interfaces;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;
using Android.Graphics;
using Notify.Models;
using Notify.Data;
using AndroidX.Core.App;

[assembly: Dependency(typeof(Notify.Droid.Interfaces.AndroidNotificationManager))]
namespace Notify.Droid.Interfaces
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        bool channelInitialized = false;
        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static Notification BuildNotification(Context context, PendingIntent pendingIntent, string german, string english)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(german)
                .SetContentText(english)
                .SetLargeIcon(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.main_icon))
                .SetSmallIcon(Resource.Drawable.logo_small)
                .SetDefaults((int)NotificationDefaults.Vibrate);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                builder.SetVisibility((int)NotificationVisibility.Public);
            }

            var notification = builder.Build();
            return notification;
        }

        public void Initialize()
        {
            CreateNotificationChannel();
        }

       

        public int ScheduleNotification(int id)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            var context = AndroidApp.Context;

            var translationDBController = new TranslationDatabaseController();
            var randomTranslation = translationDBController.GetRandomTranslation();

            Intent intent = new Intent(context, typeof(MainActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(context, id, intent, PendingIntentFlags.OneShot);

            var notification = BuildNotification(context, pendingIntent, randomTranslation.German, randomTranslation.English);

            manager.Notify(id, notification);

            ReceiveNotification(randomTranslation.German, randomTranslation.English);
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
            var historyDatabaseController = new HistoryDatabaseController();
            historyDatabaseController.SaveToHistory(itemTranslation);
        }

        void CreateNotificationChannel()
        {
            var context = AndroidApp.Context;
            manager = (NotificationManager)context.GetSystemService(AndroidApp.NotificationService);

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