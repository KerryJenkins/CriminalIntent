using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class CrimeFragment : Android.Support.V4.App.Fragment
    {
        private Crime _crime;
        private EditText _titleField;
        private Button _dateButton;
        private CheckBox _solvedCheckBox;

        public const string EXTRA_CRIME_ID = "com.bignerdranch.android.criminalintent.crime_id";
        public const string DIALOG_DATE = "date";
        public const int REQUEST_DATE = 0;

        public static CrimeFragment NewInstance(Guid crimeId)
        {
            Bundle args = new Bundle();
            args.PutString(EXTRA_CRIME_ID, crimeId.ToString());

            var fragment = new CrimeFragment();
            fragment.Arguments = args;

            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var crimeId = new Guid(Arguments.GetString(EXTRA_CRIME_ID));

            var crimeLab = CrimeLab.Create(Activity);
            _crime = crimeLab.GetCrime(crimeId);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_crime, container, false);


            _titleField = v.FindViewById<EditText>(Resource.Id.crime_title);
            _titleField.Text = _crime.Title;
            _titleField.TextChanged += TitleFieldTextChanged;

            _dateButton = v.FindViewById<Button>(Resource.Id.crime_date);
            _dateButton.Text = _crime.Date.ToString(CultureInfo.InvariantCulture);
            _dateButton.Click += dateButton_Click;

            _solvedCheckBox = v.FindViewById<CheckBox>(Resource.Id.crime_solved);
            _solvedCheckBox.Checked = _crime.Solved;
            _solvedCheckBox.CheckedChange += SolvedCheckBoxCheckedChange;

            return v;        
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
    }
}