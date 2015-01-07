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
        private Boolean _subtitileVisible;
        private List<Crime> _crimes;
        private Button _addCrimeButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetHasOptionsMenu(true);

            Activity.SetTitle(Resource.String.crimes_title);
            var crimeLab = CrimeLab.Create(Activity);
            _crimes = crimeLab.Crimes;

            var adapter = new CrimeListAdapter(Activity, _crimes);

            ListAdapter = adapter;

            RetainInstance = true;
            _subtitileVisible = false;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_empty_list, container, false);

            _addCrimeButton = v.FindViewById<Button>(Resource.Id.initial_crimeButton);
            _addCrimeButton.Click += _addCrimeButton_Click;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
            {
                if (_subtitileVisible)
                {
                    Activity.ActionBar.SetSubtitle(Resource.String.subtitle);
                }
            }

            return v;
        }

        void _addCrimeButton_Click(object sender, EventArgs e)
        {
            var crime = new Crime();
            CrimeLab.Create(Activity).AddCrime(crime);
            var i = new Intent(Activity, typeof(CrimePagerActivity));
            i.PutExtra(CrimeFragment.EXTRA_CRIME_ID, crime.Id.ToString());
            StartActivityForResult(i, 0);
        }

        public override void OnResume()
        {
            base.OnResume();

            var adapter = ListAdapter as CrimeListAdapter;
            adapter.NotifyDataSetChanged();

        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.fragment_crime_list, menu);
            var showSubtitle = menu.FindItem(Resource.Id.menu_item_show_subtitle);
            if (_subtitileVisible && showSubtitle != null)
            {
                showSubtitle.SetTitle(Resource.String.hide_subtitle);
            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_item_new_crime:
                    var crime = new Crime();
                    CrimeLab.Create(Activity).AddCrime(crime);
                    var intent = new Intent(Activity, typeof(CrimePagerActivity));
                    intent.PutExtra(CrimeFragment.EXTRA_CRIME_ID, crime.Id.ToString());
                    StartActivityForResult(intent, 0);
                    return true;
                
                case Resource.Id.menu_item_show_subtitle:
                    if (Activity.ActionBar.Subtitle == null)
                    {
                        _subtitileVisible = true;
                        Activity.ActionBar.SetSubtitle(Resource.String.subtitle);
                        item.SetTitle(Resource.String.hide_subtitle);
                    }
                    else
                    {
                        _subtitileVisible = false;
                        Activity.ActionBar.Subtitle = null;
                        item.SetTitle(Resource.String.show_subtitle);

                    }
                    
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
           }
        }
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            Crime crime = _crimes[position];
            //Log.Debug(TAG, crime.Title + " was clicked");

            var i = new Intent(Activity, typeof(CrimePagerActivity));
            i.PutExtra(CrimeFragment.EXTRA_CRIME_ID, crime.Id.ToString());
            StartActivity(i);
        }
    }
}