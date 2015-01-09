using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Android.Graphics.Drawables;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class CrimeFragment : Android.Support.V4.App.Fragment
    {
        private const string TAG = "CrimeFragment";
        private const string DIALOG_IMAGE = "image";

        private Crime _crime;
        private EditText _titleField;
        private Button _dateButton;
        private CheckBox _solvedCheckBox;
        private ImageButton _photoButton;
        private ImageView _photoView;

        public const string EXTRA_CRIME_ID = "com.bignerdranch.android.criminalintent.crime_id";
        public const string DIALOG_DATE = "date";
        public const int REQUEST_DATE = 0;
        public const int REQUEST_PHOTO = 1;

        public static CrimeFragment NewInstance(Guid crimeId)
        {
            Bundle args = new Bundle();
            args.PutString(EXTRA_CRIME_ID, crimeId.ToString());

            var fragment = new CrimeFragment();
            fragment.Arguments = args;

            return fragment;
        }

        public override void OnStart()
        {
            base.OnStart();
            ShowPhoto();
        }

        public override void OnStop()
        {
            base.OnStop();
            PictureUtils.CleanImageView(_photoView);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var crimeId = new Guid(Arguments.GetString(EXTRA_CRIME_ID));

            var crimeLab = CrimeLab.Create(Activity);
            _crime = crimeLab.GetCrime(crimeId);

            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_crime, container, false);

            if (Android.Support.V4.App.NavUtils.GetParentActivityName(Activity) != null)
            {
                Activity.ActionBar.SetDisplayHomeAsUpEnabled(true);
            }

            _titleField = v.FindViewById<EditText>(Resource.Id.crime_title);
            _titleField.Text = _crime.Title;
            _titleField.TextChanged += TitleFieldTextChanged;

            _dateButton = v.FindViewById<Button>(Resource.Id.crime_date);
            UpdateDate();
            _dateButton.Click += dateButton_Click;

            _solvedCheckBox = v.FindViewById<CheckBox>(Resource.Id.crime_solved);
            _solvedCheckBox.Checked = _crime.Solved;
            _solvedCheckBox.CheckedChange += SolvedCheckBoxCheckedChange;

            _photoButton = v.FindViewById<ImageButton>(Resource.Id.crime_imageButton);
            _photoButton.Click += _photoButton_Click;

            _photoView = v.FindViewById<ImageView>(Resource.Id.crime_imageView);
            _photoView.Click += _photoView_Click;
            var pm = Activity.PackageManager;
            var hasACamera = pm.HasSystemFeature(Android.Content.PM.PackageManager.FeatureCamera) ||
                            pm.HasSystemFeature(Android.Content.PM.PackageManager.FeatureCameraFront) ||
                            (Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread &&
                             Camera.NumberOfCameras > 0);
            if (!hasACamera)
            {
                _photoButton.Enabled = false;
            }

            return v;        
        }

        void _photoView_Click(object sender, EventArgs e)
        {
            var p = _crime.Picture;
            if (p == null) return;

            var fm = Activity.SupportFragmentManager;
            ImageFragment.NewInstance(p.Filename).Show(fm, DIALOG_IMAGE);
        }

        void _photoButton_Click(object sender, EventArgs e)
        {
            var i = new Intent(Activity, typeof(CrimeCameraActivity));
            StartActivityForResult(i, REQUEST_PHOTO);
        }

        void ShowPhoto()
        {
            Photo p = _crime.Picture;
            BitmapDrawable b = null;
            if (p != null)
            {
                b = PictureUtils.GetScaledDrawable(Activity, p.Filename);
            }
            _photoView.SetImageDrawable(b);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (Android.Support.V4.App.NavUtils.GetParentActivityName(Activity) != null) {
                        Android.Support.V4.App.NavUtils.NavigateUpFromSameTask(Activity);
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        void dateButton_Click(object sender, EventArgs e)
        {
            var fm = Activity.SupportFragmentManager;
            var dialog = DatePickerFragment.NewInstance(_crime.Date);
            dialog.SetTargetFragment(this, REQUEST_DATE);
            dialog.Show(fm, DIALOG_DATE);
        }

        void SolvedCheckBoxCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            _crime.Solved = e.IsChecked;
        }

        void TitleFieldTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            _crime.Title = e.Text.ToString();
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            //base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode != (int)Android.App.Result.Ok) return;

            if (requestCode == REQUEST_DATE)
            {
                var date = DateTime.Parse(data.GetStringExtra(DatePickerFragment.EXTRA_DATE));
                _crime.Date = date;
                UpdateDate();
            }
            else if (requestCode == REQUEST_PHOTO)
            {
                string fileName = data.GetStringExtra(CrimeCameraFragment.EXTRA_PHOTO_FILENAME);
                if (fileName != null)
                {
                    var p = new Photo(fileName);
                    _crime.Picture = p;
                    ShowPhoto();
                }
            }
        }

        public override void OnPause()
        {
            base.OnPause();
            CrimeLab.Create(Activity).SaveCrimes();
        }

        private void UpdateDate()
        {
            _dateButton.Text = _crime.Date.ToString(CultureInfo.InvariantCulture);
        }


    }
}