using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    [Activity(Label = "CrimeListActivity", MainLauncher = true, Icon = "@drawable/icon")]
    public class CrimeListActivity : SingleFragmentActivity
    {

        protected override Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }
    }
}