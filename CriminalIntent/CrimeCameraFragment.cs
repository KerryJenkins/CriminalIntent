using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class CrimeCameraFragment : Android.Support.V4.App.Fragment
    {
        private const string TAG = "CrimeCameraFragment";

        private Camera _camera;
        private SurfaceView _surfaceView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_crime_camera, container, false);

            var takePictureButton = v.FindViewById<Button>(Resource.Id.crime_camera_takePictureButton);

            takePictureButton.Click += takePictureButton_Click;

            _surfaceView = v.FindViewById<SurfaceView>(Resource.Id.crime_camera_surfaceView);

            return v;
        }

        void takePictureButton_Click(object sender, EventArgs e)
        {
            Activity.Finish();
        }
     }
}