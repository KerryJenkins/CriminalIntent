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
        private Crime crime;
        private EditText titleField;
        private Button dateButton;
        private CheckBox solvedCheckBox;

        public const string EXTRA_CRIME_ID = "com.bignerdranch.android.criminalintent.crime_id";

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
            crime = crimeLab.GetCrime(crimeId);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_crime, container, false);


            titleField = v.FindViewById<EditText>(Resource.Id.crime_title);
            titleField.Text = crime.Title;
            titleField.TextChanged += TitleFieldTextChanged;

            dateButton = v.FindViewById<Button>(Resource.Id.crime_date);
            dateButton.Text = crime.Date.ToString(CultureInfo.InvariantCulture);
            dateButton.Enabled = false;

            solvedCheckBox = v.FindViewById<CheckBox>(Resource.Id.crime_solved);
            solvedCheckBox.Checked = crime.Solved;
            solvedCheckBox.CheckedChange += SolvedCheckBoxCheckedChange;

            return v;        
        }

        void SolvedCheckBoxCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            crime.Solved = e.IsChecked;
        }

        void TitleFieldTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            crime.Title = e.Text.ToString();
        }
    }
}