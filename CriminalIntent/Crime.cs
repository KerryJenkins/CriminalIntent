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

        public Crime()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
        }

        public Crime(JSONObject json)
        {
            Id = new Guid(json.GetString(JSON_ID));
            if (json.Has(JSON_TITLE))
            {
                Title = json.GetString(JSON_TITLE);
            }
            Solved = json.GetBoolean(JSON_SOLVED);
            Date = DateTime.ParseExact(json.GetString(JSON_DATE), "O", null);
        }

        public JSONObject toJSON()
        {
            var json = new JSONObject();
            json.Put(JSON_ID, Id.ToString());
            json.Put(JSON_TITLE, Title);
            json.Put(JSON_SOLVED, Solved);
            json.Put(JSON_DATE, Date.ToString("O"));
            return json;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}