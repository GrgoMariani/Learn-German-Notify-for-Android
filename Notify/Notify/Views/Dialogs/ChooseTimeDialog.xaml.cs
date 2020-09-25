using Notify.Data;
using Notify.Interfaces;
using Notify.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notify.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseTimeDialog : ContentPage
    {
        readonly ItemScheduledTime scheduledTime;

        public ChooseTimeDialog()
        {
            InitializeComponent();
            scheduledTime = new ItemScheduledTime();
            _timePicker.Time = DateTime.Now.TimeOfDay;
        }

        public ChooseTimeDialog(ItemScheduledTime itemScheduledTime)
        {
            InitializeComponent();
            scheduledTime = itemScheduledTime;
            var newTime = new TimeSpan(scheduledTime.Hour, scheduledTime.Minute, 0);
            _timePicker.Time = newTime;
        }


        async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            scheduledTime.Hour = _timePicker.Time.Hours;
            scheduledTime.Minute = _timePicker.Time.Minutes;

            // TODO DELETE
            var alarmService = DependencyService.Get<ISetAlarm>();
            if (scheduledTime.ID.HasValue)
                alarmService.CancelAlarm(scheduledTime.ID.Value);
            int id = alarmService.SetAlarm(scheduledTime.Hour, scheduledTime.Minute);

            scheduledTime.ID = id;

            var scheduledTimeDatabaseController = new ScheduledTimeDatabaseController();
            scheduledTimeDatabaseController.SaveScheduledTime(scheduledTime);

            await Navigation.PopToRootAsync();
        }

    }
}