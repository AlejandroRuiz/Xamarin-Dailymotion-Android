using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace DailymotionAndroid.SDK
{
	public class DMWebVideoView : WebView
	{
		private WebSettings mWebSettings;
		private CustomWebChromeClient mChromeClient;
		public VideoView mCustomVideoView;
		public WebChromeClient.ICustomViewCallback mViewCallback;

		private const String mEmbedUrl = "http://www.dailymotion.com/embed/video/{0}?html=1&fullscreen={1}&app={2}&api=location";
		private const String mExtraUA = "; DailymotionEmbedSDK 1.0";
		private FrameLayout mVideoLayout;
		public bool mIsFullscreen = false;
		private FrameLayout mRootLayout;
		private bool mAllowAutomaticNativeFullscreen = false;
		private bool mAutoPlay = false;

		public DMWebVideoView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			Init ();
		}

		public DMWebVideoView (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			Init ();
		}

		public DMWebVideoView (Context context) : base (context)
		{
			Init ();
		}

		private void Init ()
		{
			//The topmost layout of the window where the actual VideoView will be added to
			mRootLayout = (FrameLayout)((Activity)Context).Window.DecorView;

			mWebSettings = Settings;
			mWebSettings.JavaScriptEnabled = true;
			mWebSettings.SetPluginState (WebSettings.PluginState.On);
			mWebSettings.UserAgentString = mWebSettings.UserAgentString + mExtraUA;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean) {
				mWebSettings.MediaPlaybackRequiresUserGesture = false;
			}
			mChromeClient = new CustomWebChromeClient(this);
			SetWebChromeClient(mChromeClient);
			SetWebViewClient(new CustomWebViewClient(this));
		}

		public void callPlayerMethod (String method)
		{
			LoadUrl ("javascript:player.api(\"" + method + "\")");
		}
		public void setVideoId (String videoId)
		{
			LoadUrl (String.Format (mEmbedUrl, videoId, mAllowAutomaticNativeFullscreen, Context.PackageName));
		}

		public void setVideoId (String videoId, bool autoPlay)
		{
			mAutoPlay = autoPlay;
			var url = String.Format(mEmbedUrl, videoId, mAllowAutomaticNativeFullscreen, Context.PackageName);
			LoadUrl (url);
		}

		public void hideVideoView ()
		{
			if (isFullscreen ()) {
				if (mCustomVideoView != null) {
					mCustomVideoView.StopPlayback ();
				}
				mRootLayout.RemoveView (mVideoLayout);
				mViewCallback.OnCustomViewHidden ();
				((Activity)Context).VolumeControlStream = (Stream)AudioManager.UseDefaultStreamType;
				mIsFullscreen = false;
			}
		}

		public void setupVideoLayout (View video)
		{

			/**
			 * As we don't want the touch events to be processed by the underlying WebView, we do not set the WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE flag
			 * But then we have to handle directly back press in our View to exit fullscreen.
			 * Otherwise the back button will be handled by the topmost Window, id-est the player controller
			 */
			mVideoLayout = new CustomFrameLayout (Context, this);

			mVideoLayout.Background = new ColorDrawable (Color.Black);
			mVideoLayout.AddView (video);
			FrameLayout.LayoutParams lp = new FrameLayout.LayoutParams (LayoutParams.MatchParent, LayoutParams.MatchParent);
			lp.Gravity = GravityFlags.Center;
			mRootLayout.AddView (mVideoLayout, lp);
			mIsFullscreen = true;
		}

		public bool isFullscreen ()
		{
			return mIsFullscreen;
		}

		public void handleBackPress (Activity activity)
		{
			if (isFullscreen ()) {
				hideVideoView ();
			} else {
				LoadUrl ("");//Hack to stop video
				activity.Finish ();
			}
		}

		public void setAllowAutomaticNativeFullscreen (bool allowAutomaticNativeFullscreen)
		{
			mAllowAutomaticNativeFullscreen = allowAutomaticNativeFullscreen;
		}

		public bool isAutoPlaying ()
		{
			return mAutoPlay;
		}

		public void setAutoPlay (bool autoPlay)
		{
			mAutoPlay = autoPlay;
		}
	}
}