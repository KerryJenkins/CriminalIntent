using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    [Android.App.Activity(Label = "@string/ApplicationName", 
        Name = "dtc.nin.ukjenks.criminalintent.CrimeCameraActivity", 
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class CrimeCameraActivity : SingleFragmentActivity
    {
        protected override Fragment CreateFragment()
        {
            return new CrimeCameraFragment();
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.AddFlags(WindowManagerFlags.Fullscreen);

            base.OnCreate(bundle);
        }
    }
}