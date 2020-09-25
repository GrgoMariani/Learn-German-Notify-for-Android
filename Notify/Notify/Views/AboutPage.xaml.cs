using Notify.Interfaces;
using Notify.Models;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notify.Views
{
    public partial class AboutPage : ContentPage
    {
        INotificationManager notificationManager;

        public AboutPage()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                NotificationShown(evtData.Title, evtData.Message);
            };
            Title = "About";
        }



        void OnScheduleClick(object sender, EventArgs e)
        {
            notificationManager.ScheduleNotification(new Random().Next());
        }

        void NotificationShown(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                stackLabel.Text = $"<b>{title}</b><br><br><i>{message}</i>";
            });
        }
    }
}