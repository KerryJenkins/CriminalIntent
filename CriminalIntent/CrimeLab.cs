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
    public class CrimeLab
    {
        private static CrimeLab _crimeLab;
        private Context _appContext;
        private static Object _creationLock = new Object();
        private List<Crime> _crimes;

        private CrimeLab(Context appContext)
        {
            _appContext = appContext;
            _crimes = new List<Crime>();

            for (int i = 0; i < 100; i++)
            {
                var crime = new Crime();
                crime.Title = "Crime #" + i;
                crime.Solved = (i % 2 == 0);
                _crimes.Add(crime);
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
    }
}