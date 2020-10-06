using Notify.Data;
using Notify.Interfaces;
using Notify.Models;
using Notify.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScheduleNotificationsPage : ContentPage
    {
        public ObservableCollection<ItemScheduledTime> ScheduledTimes { get; private set; }

        

        public ScheduleNotificationsPage()
        {
            ScheduledTimes = new ObservableCollection<ItemScheduledTime>();

            InitializeComponent();

            Title = "Alarms";

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var scheduledTimeDatabaseController = new ScheduledTimeDatabaseController();
            var scheduledTimes = scheduledTimeDatabaseController.GetAllScheduledTimes();

            ScheduledTimes.Clear();
            foreach (var scheduledTime in scheduledTimes)
            {
                ScheduledTimes.Add(scheduledTime);
            }
        }


        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var itemToDelete = (ItemScheduledTime)mi.CommandParameter;
            var scheduledTimeDatabaseController = new ScheduledTimeDatabaseController();
            scheduledTimeDatabaseController.DeleteScheduledTime(itemToDelete);
            var alarmService = DependencyService.Get<ISetAlarm>();
            if (itemToDelete.ID.HasValue)
                alarmService.CancelAlarm(itemToDelete.ID.Value);
            ScheduledTimes.Remove(itemToDelete);
        }

       

        async void OnScheduleTimeClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseTimeDialog());
        }

        
    }
}