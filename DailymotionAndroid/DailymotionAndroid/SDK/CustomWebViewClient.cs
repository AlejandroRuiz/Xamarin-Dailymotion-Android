using System;
using Android.Webkit;
using DailymotionAndroid.SDK;

namespace DailymotionAndroid
{
	public class CustomWebViewClient : WebViewClient
	{
		public DMWebVideoView VideoView;

		public CustomWebViewClient(DMWebVideoView videoView)
		{
			VideoView = videoView;
		}

		public override bool ShouldOverrideUrlLoading(WebView view, string url)
		{
			Console.WriteLine(url);
			Android.Net.Uri uri = Android.Net.Uri.Parse(url);
			if (uri.Scheme == "dmevent")
			{
				string evt = uri.GetQueryParameter("event");
				if (evt == "apiready")
				{
					if (VideoView.isAutoPlaying())
					{
						VideoView.callPlayerMethod("play");
					}
				}
				return true;
			}
			else {
				return base.ShouldOverrideUrlLoading(view, url);
			}
		}
	}
}

