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
    //[Activity(Label = "SingleFragmentActivity")]
    public abstract class SingleFragmentActivity : FragmentActivity
    {
        protected abstract Android.Support.V4.App.Fragment CreateFragment();

        protected virtual int GetLayoutResId()
        {
            return Resource.Layout.activity_fragment;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(GetLayoutResId());

            var fm = SupportFragmentManager;
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