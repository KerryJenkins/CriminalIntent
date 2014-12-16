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
using Java.Util;

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class Crime
    {
        public UUID Id { get; private set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool Solved { get; set; }

        public Crime()
        {
            Id = UUID.RandomUUID();
            Date = DateTime.Now;
        }
    }
}