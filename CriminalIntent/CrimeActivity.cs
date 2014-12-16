using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Runtime;


namespace DTC.NIN.Ukjenks.CriminalIntent
{
    [Activity(Label = "CriminalIntent", MainLauncher = true, Icon = "@drawable/icon")]
    public class CrimeActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_crime);

            var fm = FragmentManager;
            var fragment = fm.FindFragmentById(Resource.Id.fragmentContainer);

            if (fragment == null)
            {
                fragment = new CrimeFragment();
                fm.BeginTransaction()
                    .Add(Resource.Id.fragmentContainer, fragment)
                    .Commit();
            }

        }
    }
}

