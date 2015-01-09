using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class ImageFragment : DialogFragment
    {
        public const string EXTRA_IMAGE_PATH = "com.arcbtech.android.criminalintent.image_path";
        private ImageView _imageView;

        public static ImageFragment NewInstance(string imagePath)
        {
            var args = new Bundle();
            args.PutString(EXTRA_IMAGE_PATH, imagePath);

            var fragment = new ImageFragment();
            fragment.Arguments = args;
            fragment.SetStyle(DialogFragment.StyleNoTitle, 0);

            return fragment;
        }
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //return base.OnCreateView(inflater, container, savedInstanceState);

            _imageView = new ImageView(Activity);
            var path = Arguments.GetString(EXTRA_IMAGE_PATH);
            var image = PictureUtils.GetScaledDrawable(Activity, path);
            _imageView.SetImageDrawable(image);

            return _imageView;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            PictureUtils.CleanImageView(_imageView);
        }
    }
}