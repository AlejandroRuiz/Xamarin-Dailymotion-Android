using System;
using Android.App;
using Android.Media;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace DailymotionAndroid.SDK
{
	public class CustomWebChromeClient : WebChromeClient, MediaPlayer.IOnCompletionListener
	{
		public void OnCompletion(MediaPlayer mp)
		{
			VideoView.hideVideoView();
		}

		public DMWebVideoView VideoView;

		public CustomWebChromeClient(DMWebVideoView videoView)
		{
			VideoView = videoView;
		}

		public override View VideoLoadingProgressView
		{
			get
			{
				ProgressBar pb = new ProgressBar(VideoView.Context);
				pb.Indeterminate = true;
				return pb;
			}
		}

		public override void OnShowCustomView(View view, WebChromeClient.ICustomViewCallback callback)
		{
			base.OnShowCustomView(view, callback);
			((Activity)VideoView.Context).VolumeControlStream = Stream.Music;
			VideoView.mIsFullscreen = true;
			VideoView.mViewCallback = callback;
			if (view is FrameLayout)
			{
				FrameLayout frame = (FrameLayout)view;
				if (frame.FocusedChild is VideoView)
				{//We are in 2.3
					VideoView video = (VideoView)frame.FocusedChild;
					frame.RemoveView(video);

					VideoView.setupVideoLayout(video);

					VideoView.mCustomVideoView = video;
					VideoView.mCustomVideoView.SetOnCompletionListener(this);


				}
				else {//Handle 4.x
					VideoView.setupVideoLayout(view);

				}
			}
		}

		public override void OnShowCustomView(View view, Android.Content.PM.ScreenOrientation requestedOrientation, WebChromeClient.ICustomViewCallback callback)
		{
			OnShowCustomView(view, callback);
		}

		public override void OnHideCustomView()
		{
			VideoView.hideVideoView();
		}
	}
}

