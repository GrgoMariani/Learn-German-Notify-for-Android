using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Notify.Droid;
using Notify.Droid.Alarms;

[Activity(Theme = "@style/SplashTheme.Splash", MainLauncher = true, NoHistory = true, Icon = "@mipmap/icon", RoundIcon = "@mipmap/icon_round")]
public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
       // global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
    }

    // Launches the startup task
    protected override void OnResume()
    {
        base.OnResume();
        Task startupWork = new Task(() => { SetupRequiredFilesAndStart(); });
        startupWork.Start();
    }

    void SetupRequiredFilesAndStart()
    {
        InitialSetupHelper initialSetupHelper = new InitialSetupHelper("translations.db3");
        if (!initialSetupHelper.IsSetup())
            initialSetupHelper.CopyDBFromAssets();

        // Cancel and set all alarms
        var context = ApplicationContext;
        var util = new AlarmUtil(context);
        var alarmStorage = new AlarmStorage(context);
        foreach (Alarm alarm in alarmStorage.GetAlarms())
        {
            util.CancelAlarm(alarm);
            util.ScheduleAlarm(alarm);
        }
            

        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
}