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
using Android.Util;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public partial class CrimeCameraFragment : Android.Support.V4.App.Fragment
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

            var holder = _surfaceView.Holder;
            //holder.SetType(SurfaceType.PushBuffers);

            holder.AddCallback(this as ISurfaceHolderCallback);

            return v;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Gingerbread)
            {
                _camera = Camera.Open(0);
            }
            else
            {
                _camera = Camera.Open();
            }
        }

        public override void OnPause()
        {
            base.OnPause();

            if (_camera != null)
            {
                _camera.Release();
                _camera = null;
            }
        }

        void takePictureButton_Click(object sender, EventArgs e)
        {
            Activity.Finish();
        }
     }

    public partial class CrimeCameraFragment :  ISurfaceHolderCallback
    {

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            if (_camera != null) return;

            var parameters = _camera.GetParameters();
            Android.Hardware.Camera.Size s = GetBestSupportedSize(parameters.SupportedPreviewSizes, width, height);
            parameters.SetPreviewSize(s.Width, s.Height);
            _camera.SetParameters(parameters);

            try
            {
                _camera.StartPreview();
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "Could not start preview", ex);
                _camera.Release();
                _camera = null;
            }
         }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                if (_camera != null)
                {
                    _camera.SetPreviewDisplay(holder);
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "Error setting up preview display", ex.ToString());
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (_camera != null)
            {
                _camera.StopPreview();
            }
        }

        private Android.Hardware.Camera.Size GetBestSupportedSize(IList<Android.Hardware.Camera.Size> sizes, int width, int height)
        {
            var bestSize = sizes[0];
            int largestArea = bestSize.Width * bestSize.Height;
            foreach (var s in sizes)
            {
                var area = s.Width * s.Height;
                if (area > largestArea)
                {
                    bestSize = s;
                    largestArea = area;
                }

            }

            return bestSize;
        }
    }
}