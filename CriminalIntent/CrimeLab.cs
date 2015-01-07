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
using Android.Util;


namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class CrimeLab
    {
        private const string TAG = "CrimeLab";
        private const string FILENAME = "crimes.json";

        private static CrimeLab _crimeLab;
        private Context _appContext;
        private static Object _creationLock = new Object();
        private List<Crime> _crimes;
        private CriminalIntentJSONSerializer _serializer;

        private CrimeLab(Context appContext)
        {
            _appContext = appContext;
            _serializer = new CriminalIntentJSONSerializer(FILENAME);
            try
            {
                _crimes = _serializer.LoadCrimes();
            }
            catch (Exception ex)
            {
                _crimes = new List<Crime>();
                Log.Error(TAG, "Error loading crimes:", ex.ToString());
            }
         }

        public Crime GetCrime(Guid id)
        {
            foreach (var crime in _crimes) {
                if (crime.Id == id)
                {
                    return crime;
                }
            }
            return null;
        }

        public void AddCrime(Crime c)
        {
            _crimes.Add(c);
        }

        public List<Crime> Crimes {
            get
            {
                return _crimes;
            }
        }

        public static CrimeLab Create(Context context)
        {
            lock (_creationLock)
            {
                if (_crimeLab == null)
                {
                    _crimeLab = new CrimeLab(context.ApplicationContext);
                }
            }
            return _crimeLab;
        }

        public Boolean SaveCrimes()
        {
            try
            {
                _serializer.SaveCrimes(_crimes);
                Log.Debug(TAG, "crimes saved to file");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "Error saving crimes: ", ex);
                return false;
            }
        }
    }
}