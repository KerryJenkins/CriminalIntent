using System;
using System.Collections.Generic;
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
    public class CrimeListFragment : ListFragment
    {
        private const string TAG = "CrimeListFragment";

        private List<Crime> _crimes;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Activity.SetTitle(Resource.String.crimes_title);
            var crimeLab = CrimeLab.Create(Activity);
            _crimes = crimeLab.Crimes;

            var adapter = new CrimeListAdapter(Activity, _crimes.ToArray());

            ListAdapter = adapter;
        }

        public override void OnResume()
        {
            base.OnResume();

            var adapter = ListAdapter as CrimeListAdapter;
            adapter.NotifyDataSetChanged();

        }
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            Crime crime = _crimes[position];
            //Log.Debug(TAG, crime.Title + " was clicked");

            var i = new Intent(this.Activity, typeof(CrimePagerActivity));
            i.PutExtra(CrimeFragment.EXTRA_CRIME_ID, crime.Id.ToString());
            StartActivity(i);
        }
    }
}