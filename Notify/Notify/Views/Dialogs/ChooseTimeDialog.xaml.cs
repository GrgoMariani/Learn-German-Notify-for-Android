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
        public int pickerIndex = 0;

        public ChooseTimeDialog()
        {
            InitializeComponent();
            scheduledTime = new ItemScheduledTime();
            _timePicker.Time = DateTime.Now.TimeOfDay;
            Title = "New Alarm";
            BindingContext = this;

            var settingsDatabaseController = new SettingsDatabaseController();
            var curr_settings = settingsDatabaseController.GetSettings();
            if (curr_settings == null)
            {
                curr_settings = new ItemSettings()
                {
                    IsDatabaseSetUp = true,
                    LastDifficultyChosen = "Other"
                };
                settingsDatabaseController.SaveSettings(curr_settings);
            }
            switch(curr_settings.LastDifficultyChosen)
            {
                case "A1": pickerIndex = 0; break;
                case "A2": pickerIndex = 1; break;
                case "B1": pickerIndex = 2; break;
                case "B2": pickerIndex = 3; break;
                default: pickerIndex = 4; break;
            }
            _difficultyPicker.SelectedIndex = pickerIndex;
        }

        async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            scheduledTime.Hour = _timePicker.Time.Hours;
            scheduledTime.Minute = _timePicker.Time.Minutes;

            var settingsDatabaseController = new SettingsDatabaseController();
            var curr_settings = settingsDatabaseController.GetSettings();
            switch (pickerIndex)
            {
                case 0: curr_settings.LastDifficultyChosen = "A1"; break;
                case 1: curr_settings.LastDifficultyChosen = "A2"; break;
                case 2: curr_settings.LastDifficultyChosen = "B1"; break;
                case 3: curr_settings.LastDifficultyChosen = "B2"; break;
                case 4: curr_settings.LastDifficultyChosen = "Other"; break;
            }
            settingsDatabaseController.SaveSettings(curr_settings);
            scheduledTime.Difficulty = curr_settings.LastDifficultyChosen;
            // TODO DELETE
            var alarmService = DependencyService.Get<ISetAlarm>();
            if (scheduledTime.ID.HasValue)
                alarmService.CancelAlarm(scheduledTime.ID.Value);
            int id = alarmService.SetAlarm(scheduledTime.Hour, scheduledTime.Minute, scheduledTime.Difficulty);

            scheduledTime.ID = id;

            var scheduledTimeDatabaseController = new ScheduledTimeDatabaseController();
            scheduledTimeDatabaseController.SaveScheduledTime(scheduledTime);

            await Navigation.PopToRootAsync();
        }


    }
}