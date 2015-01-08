using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Json;
using Java.IO;
using Newtonsoft.Json;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class CriminalIntentJSONSerializer
    {
        private string _filename;

        private string FullFilename
        {
            get
            {
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                return System.IO.Path.Combine(documentsPath, _filename);

            }
        }
        public CriminalIntentJSONSerializer(string f)
        {
            _filename = f;
        }

        public void SaveCrimes(List<Crime> crimes)
        {
            var json = JsonConvert.SerializeObject(crimes);
            System.IO.File.WriteAllText(FullFilename, json);
        }
            
        public List<Crime> LoadCrimes()
        {
            var json = System.IO.File.ReadAllText(FullFilename);
            var crimes = JsonConvert.DeserializeObject<List<Crime>>(json);
            return crimes;
        }


    }
}