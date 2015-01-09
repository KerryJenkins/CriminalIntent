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

namespace DTC.NIN.Ukjenks.CriminalIntent
{
    public class Crime
    {
 
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool Solved { get; set; }
        public Photo Picture { get; set; }

        public string Suspect { get; set; }

        public Crime()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}