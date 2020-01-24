using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Util;
using System.Threading.Tasks;

namespace final_app.Droid
{
    [Activity(Theme ="@style/Logo.Splash", Icon = "@drawable/icon", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : AppCompatActivity   // First page that shows when the application is opening
    {
        static readonly string TAG = "X:" + typeof(SplashScreen).Name;
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistableBundle)
        {
            base.OnCreate(savedInstanceState, persistableBundle);
            Log.Debug(TAG, "SplashScreen created and running");
        }

        protected override void OnResume()  // Shows logo on splash screen at open
        {
            base.OnResume();
            Task Splashscreen = new Task(() => { ShowLogo(); });
            Splashscreen.Start();
        }

        public override void OnBackPressed() { }    // Does nothing

        async void ShowLogo()   // Shows logo on splash screen
        {
            Log.Debug(TAG, "Startup Screen shown.");
            await Task.Delay(8000);
            Log.Debug(TAG, "Delay done starting application.");
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}