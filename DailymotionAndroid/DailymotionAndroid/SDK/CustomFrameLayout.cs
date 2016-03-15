using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DailymotionAndroid.SDK
{
	public class CustomFrameLayout : FrameLayout
	{
		DMWebVideoView VideoView;

		public CustomFrameLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
		}

		public CustomFrameLayout(Context context, IAttributeSet attrs) : base(context, attrs)
		{

		}

		public CustomFrameLayout(Context context, DMWebVideoView videoView) : base(context)
		{
			VideoView = videoView;
		}

		public override bool DispatchKeyEvent(KeyEvent e)
		{
			if (e.KeyCode == Keycode.Back && e.Action == KeyEventActions.Up)
			{
				VideoView.hideVideoView();
				return true;
			}
			return base.DispatchKeyEvent(e);
		}
	}
}

