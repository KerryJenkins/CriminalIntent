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
    [Android.App.Activity(Label = "CrimeListActivity", MainLauncher = true, Icon = "@drawable/Icon")]
    public class CrimeListActivity : SingleFragmentActivity
    {

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }
    }
}