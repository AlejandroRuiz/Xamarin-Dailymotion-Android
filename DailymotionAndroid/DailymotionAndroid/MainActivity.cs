using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using DailymotionAndroid.SDK;

namespace DailymotionAndroid
{
	[Activity (
		Label = "Dailymotion Android",
		MainLauncher = true,
		Icon = "@mipmap/icon",
		HardwareAccelerated = true,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize

	)]
	public class MainActivity : Activity
	{
		private DMWebVideoView mVideoView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			mVideoView = FindViewById<DMWebVideoView>(Resource.Id.dmWebVideoView);
			mVideoView.setVideoId("x10iisk", true);
		}

		public override void OnBackPressed()
		{
			//base.OnBackPressed();
			mVideoView.handleBackPress(this);
		}

		protected override void OnPause()
		{
			base.OnPause();

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
			{
				mVideoView.OnPause();
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
			{
				mVideoView.OnResume();
			}
		}
	}
}


