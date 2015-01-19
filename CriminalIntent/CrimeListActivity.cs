using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    [Android.App.Activity(Label = "@string/ApplicationName", 
        MainLauncher = true, 
        Icon = "@drawable/Icon")]
    public class CrimeListActivity : SingleFragmentActivity, 
                                     CrimeListFragment.Callbacks,
                                     CrimeFragment.Callbacks
    {

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }

        protected override int GetLayoutResId() {
            return Resource.Layout.activity_masterdetail;
            //return Resource.Layout.activity_twopane;
        }

        public void onCrimeSelected(Crime crime)
        {
            if (FindViewById(Resource.Id.detailFragmentContainer) == null)
            {
                // Start an instance of CrimePagerActivity
                var i = new Intent(this, typeof(CrimePagerActivity));
                i.PutExtra(CrimeFragment.EXTRA_CRIME_ID, crime.Id.ToString());
                StartActivity(i);
            }
            else
            {
                var fm = SupportFragmentManager;
                var ft = fm.BeginTransaction();

                var oldDetail = fm.FindFragmentById(Resource.Id.detailFragmentContainer);
                var newDetail = CrimeFragment.NewInstance(crime.Id);

                if (oldDetail != null)
                {
                    ft.Remove(oldDetail);
                }
                ft.Add(Resource.Id.detailFragmentContainer, newDetail);
                ft.Commit();
            }
        }

        public void OnCrimeUpdated(Crime crime)
        {
            var fm = SupportFragmentManager;
            var listFragment = (CrimeListFragment)fm.FindFragmentById(Resource.Id.fragmentContainer);
            listFragment.UpdateUI();
        }

    }
}