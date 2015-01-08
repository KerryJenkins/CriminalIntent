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

        public const string EXTRA_PHOTO_FILENAME = "com.arcbtech.android.criminalintent.photo_filename";

        private Camera _camera;
        private SurfaceView _surfaceView;
        private View _progressContainer;
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_crime_camera, container, false);

            _progressContainer = v.FindViewById(Resource.Id.crime_camera_progressContainer);
            _progressContainer.Visibility = ViewStates.Invisible;

            var takePictureButton = v.FindViewById<Button>(Resource.Id.crime_camera_takePictureButton);

            takePictureButton.Click += takePictureButton_Click;

            _surfaceView = v.FindViewById<SurfaceView>(Resource.Id.crime_camera_surfaceView);

            var holder = _surfaceView.Holder;
            holder.SetType(SurfaceType.PushBuffers);

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
            if (_camera != null)
            {
                _camera.TakePicture(this as Camera.IShutterCallback, null, this as Camera.IPictureCallback);
            }
        }
     }

    public partial class CrimeCameraFragment :  ISurfaceHolderCallback
    {

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            if (_camera == null) return;

            var parameters = _camera.GetParameters();
            Android.Hardware.Camera.Size s = GetBestSupportedSize(parameters.SupportedPreviewSizes, width, height);
            parameters.SetPreviewSize(s.Width, s.Height);
            s = GetBestSupportedSize(parameters.SupportedPictureSizes, width, height);
            parameters.SetPictureSize(s.Width, s.Height);
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

    public partial class CrimeCameraFragment : Camera.IShutterCallback
    {

        public void OnShutter()
        {
            _progressContainer.Visibility = ViewStates.Visible;
        }
    }

    public partial class CrimeCameraFragment : Camera.IPictureCallback
    {

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            string fullFilename = null;
            var success = false;
            try
            {
                var filename = Guid.NewGuid().ToString() + ".jpg";
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                fullFilename = System.IO.Path.Combine(documentsPath, filename);

                System.IO.File.WriteAllBytes(fullFilename, data);
                success = true;
            }
            finally
            {
                if (success)
                {
                    var i = new Intent();
                    i.PutExtra(EXTRA_PHOTO_FILENAME, fullFilename);
                    Activity.SetResult(Android.App.Result.Ok, i);
                }
                else
                {
                    Activity.SetResult(Android.App.Result.Canceled);
                }

                Activity.Finish();
            }

        }
    }
}