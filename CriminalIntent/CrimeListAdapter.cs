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
    public class CrimeListAdapter : ArrayAdapter<Crime>
    {
        private List<Crime> _crimes;
        private Activity _context;

        public CrimeListAdapter(Activity context, List<Crime> crimes) : 
            base(context, Android.Resource.Layout.SimpleListItem1, crimes)
        {
            _context = context;
            _crimes = crimes;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return _crimes.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_item_crime, null);
            }
            var crime = _crimes[position];
            
            var titleTextView = view.FindViewById<TextView>(Resource.Id.crime_list_item_titleTextView);
            titleTextView.Text = crime.Title;

            var dateTextView = view.FindViewById<TextView>(Resource.Id.crime_list_item_dateTextView);
            dateTextView.Text = crime.Date.ToString();

            var solvedCheckBox = view.FindViewById<CheckBox>(Resource.Id.crime_list_item_solvedCheckBox);
            solvedCheckBox.Checked = crime.Solved;

            //view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _items[position].ToString();
            return view;
        }
    }

}