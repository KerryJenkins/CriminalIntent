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
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    [Activity(Label = "CrimePagerActivity"),
     MetaData("android.support.PARENT_ACTIVITY",
        Value = "dtc.nin.ukjenks.criminalintent.CrimeListActivity")]
    public class CrimePagerActivity : FragmentActivity
    {
        private ViewPager _viewPager;
        private List<Crime> _crimes;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            _viewPager = new ViewPager(this);
            _viewPager.Id = Resource.Id.viewPager;
            SetContentView(_viewPager);

            _crimes = CrimeLab.Create(this).Crimes;

            var fm = SupportFragmentManager;
            var fspa = new MyFragmentStatePagerAdapter(fm);
            fspa.Crimes = _crimes;
            _viewPager.Adapter = fspa;

            _viewPager.SetOnPageChangeListener(new MyPageChangeListener(this, this, _crimes));

            var crimeId = new Guid(Intent.GetStringExtra(CrimeFragment.EXTRA_CRIME_ID));
            for (int i = 0; i < _crimes.Count; i++)
            {
                if (_crimes[i].Id == crimeId)
                {
                    _viewPager.CurrentItem = i;
                    break;
                }
            }



        }

        public class MyPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
        {
            Context _context;
            List<Crime> _crimes;
            CrimePagerActivity _crimePagerActivity;

            private MyPageChangeListener(Context context)
            {
                _context = context;
            }

            public MyPageChangeListener(Context context, CrimePagerActivity crimePagerActivity, List<Crime> crimes) : this(context)
            {
                _crimes = crimes;
                _crimePagerActivity = crimePagerActivity;
            }

            #region IOnPageChangeListener implementation
            public void OnPageScrollStateChanged(int p0)
            {
            }

            public void OnPageScrolled(int p0, float p1, int p2)
            {
            }

            public void OnPageSelected(int position)
            {
                var crime = _crimes[position];
                if (crime.Title != null)
                {
                    _crimePagerActivity.Title = crime.Title;
                }

            }
            #endregion
        }
    }

    internal class MyFragmentStatePagerAdapter : FragmentStatePagerAdapter 
    {
        public List<Crime> Crimes {get; set;}

        public MyFragmentStatePagerAdapter(Android.Support.V4.App.FragmentManager fm) 
            : base(fm)
        {

        }
        public override int Count
        {
            get { return Crimes.Count; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            var crime = Crimes[position];
            return CrimeFragment.NewInstance(crime.Id);
        }

    }
}