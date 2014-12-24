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
    //[Activity(Label = "SingleFragmentActivity")]
    public abstract class SingleFragmentActivity : Activity
    {
        protected abstract Fragment CreateFragment();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_fragment);

            var fm = FragmentManager;
            var fragment = fm.FindFragmentById(Resource.Id.fragmentContainer);

            if (fragment == null)
            {
                fragment = CreateFragment();
                fm.BeginTransaction()
                    .Add(Resource.Id.fragmentContainer, fragment)
                    .Commit();
            }

        }
    }
}