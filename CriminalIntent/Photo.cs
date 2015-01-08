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
    public class Photo
    {
        public string Filename { get; private set; }

        public Photo(string filename) 
        {
            Filename = filename;
        }
    }
}