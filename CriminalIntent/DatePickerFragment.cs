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
    public class DatePickerFragment : DialogFragment, DatePicker.IOnDateChangedListener
    {

        public const string EXTRA_DATE = "com.bignerdranch.android.criminalintent.date";
        private DateTime _date;

        public static DatePickerFragment NewInstance(DateTime date)
        {
            var args = new Bundle();
            args.PutString(EXTRA_DATE, date.ToString());

            var fragment = new DatePickerFragment();
            fragment.Arguments = args;

            return fragment;
        }

         public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreateDialog(savedInstanceState);

            // Create your fragment here
            _date = DateTime.Parse(Arguments.GetString(EXTRA_DATE), null, System.Globalization.DateTimeStyles.AssumeLocal);
            
            View v = Activity.LayoutInflater.Inflate(Resource.Layout.dialog_date, null);
            var datePicker = v.FindViewById<DatePicker>(Resource.Id.dialog_date_datePicker);
            datePicker.Init(_date.Year, _date.Month-1, _date.Day, this);

            return new Android.App.AlertDialog.Builder(Activity)
                .SetView(v)
                .SetTitle(Resource.String.date_picker_title)
                .SetPositiveButton(Android.Resource.String.Ok, DialogClickHandler)
                .Create();
        }

        private void DialogClickHandler(object sender, DialogClickEventArgs e)
        {
            sendResult(Android.App.Result.Ok);
        }

        private void sendResult(Android.App.Result resultCode)
        {
            if (TargetFragment == null) return;

            var i = new Intent();
            i.PutExtra(EXTRA_DATE, _date.ToString());

            TargetFragment.OnActivityResult(TargetRequestCode, (int)resultCode, i);
        }

        public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            _date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Arguments.PutString(EXTRA_DATE, _date.ToString());
        }
    }
}