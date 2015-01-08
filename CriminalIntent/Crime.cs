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
        private const string JSON_ID = "id";
        private const string JSON_TITLE = "title";
        private const string JSON_SOLVED = "solved";
        private const string JSON_DATE = "date";

        public Guid Id { get; private set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool Solved { get; set; }
        public Photo Picture { get; set; }

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