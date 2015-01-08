using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public partial class CrimeListFragment : ListFragment
    {
        private const string TAG = "CrimeListFragment";
        private Boolean _subtitileVisible;
        private List<Crime> _crimes;
        private Button _addCrimeButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;

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

            var listView = v.FindViewById<ListView>(Android.Resource.Id.List);

            if (Build.VERSION.SdkInt < BuildVersionCodes.Honeycomb)
            {
                RegisterForContextMenu(listView);
            }
            else
            {
                listView.ChoiceMode = ChoiceMode.MultipleModal;
                listView.SetMultiChoiceModeListener(this as AbsListView.IMultiChoiceModeListener); 
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

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            Activity.MenuInflater.Inflate(Resource.Menu.crime_list_item_context, menu);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = item.MenuInfo as Android.Widget.AdapterView.AdapterContextMenuInfo;
            var position = info.Position;
            var adapter = ListAdapter as CrimeListAdapter;
            var crime = adapter[position];

            switch (item.ItemId)
            {
                case Resource.Id.menu_item_delete_crime:
                    CrimeLab.Create(Activity).DeleteCrime(crime);
                    adapter.NotifyDataSetChanged();
                    return true;
            }
            return base.OnContextItemSelected(item);
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

    public partial class CrimeListFragment : AbsListView.IMultiChoiceModeListener
    {

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
        {
            // Required, but not used in this implementation
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                switch (item.ItemId)
                {
                    case Resource.Id.menu_item_delete_crime:
                        var adapter = ListAdapter as CrimeListAdapter;
                        var crimeLab = CrimeLab.Create(Activity);
                        for (int i = adapter.Count - 1; i >= 0; i--)
                        {
                            if (ListView.IsItemChecked(i)) {
                                crimeLab.DeleteCrime(adapter[i]);
                            }
                        }
                        mode.Finish();
                        adapter.NotifyDataSetChanged();
                        return true;
                    default:
                        return false;
                }
            }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            var inflater = mode.MenuInflater;
            inflater.Inflate(Resource.Menu.crime_list_item_context, menu);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            // Required, but not used in this implementation
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }
    }

}