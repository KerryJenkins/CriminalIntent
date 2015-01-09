using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class PictureUtils
    {
        public static BitmapDrawable GetScaledDrawable(Activity activity, string filename)
        {
            Display display = activity.WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            var destWidth = size.X;
            var destHeight = size.Y;
            //var destWidth = display.Width;
            //var destHeight = display.Height;

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true; 
            BitmapFactory.DecodeFile(filename, options);

            var srcWidth = options.OutWidth;
            var srcHeight = options.OutHeight;

            int inSampleSize = 1;

            if (srcHeight > destHeight || srcWidth > destWidth)
            {
                if (srcWidth > srcHeight)
                {
                    inSampleSize = (int)Math.Round(((double)srcHeight / (double)destHeight));
                }
                else
                {
                    inSampleSize = (int)Math.Round(((double)srcWidth / (double)destWidth));
                }
            }

            options = new BitmapFactory.Options();
            options.InSampleSize = inSampleSize;
            var bitmap = BitmapFactory.DecodeFile(filename, options);
            return new BitmapDrawable(activity.Resources, bitmap);
        }

        public static void CleanImageView(ImageView imageView)
        {
            if (!(imageView.Drawable is BitmapDrawable)) return;

            BitmapDrawable b = (BitmapDrawable)imageView.Drawable;
            b.Bitmap.Recycle();
            imageView.SetImageDrawable(null);
        }
    }
}