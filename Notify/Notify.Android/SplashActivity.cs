using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Notify.Droid;

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

        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
}