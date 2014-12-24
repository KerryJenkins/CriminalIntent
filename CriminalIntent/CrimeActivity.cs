using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Runtime;


namespace DTC.NIN.Ukjenks.CriminalIntent
{
    [Activity(Label = "CriminalIntent")]
    public class CrimeActivity : SingleFragmentActivity
    {
        protected override Fragment CreateFragment()
        {
            return new CrimeFragment();
        }
    }
}

