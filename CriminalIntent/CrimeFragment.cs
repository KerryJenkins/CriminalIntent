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
    public class CrimeFragment : Fragment
    {
        private Crime crime;
        private EditText titleField;
        private Button dateButton;
        private CheckBox solvedCheckBox;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            crime = new Crime();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_crime, container, false);


            titleField = v.FindViewById<EditText>(Resource.Id.crime_title);
            titleField.TextChanged += TitleFieldTextChanged;

            dateButton = v.FindViewById<Button>(Resource.Id.crime_date);
            dateButton.Text = crime.Date.ToString(CultureInfo.InvariantCulture);
            dateButton.Enabled = false;

            solvedCheckBox = v.FindViewById<CheckBox>(Resource.Id.crime_solved);
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