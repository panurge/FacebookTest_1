using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookLoginASPnetWebForms.Models
{
    public class FacebookData
    {
        public class User
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string link { get; set; }
            public string username { get; set; }
            public string gender { get; set; }
            public string locale { get; set; }
            public string birthday { get; set; }
        }


        public class From
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Datum2
        {
            public string created_time { get; set; }
            public From from { get; set; }
            public string message { get; set; }
            public string id { get; set; }
        }

        public class Cursors
        {
            public string before { get; set; }
            public string after { get; set; }
        }

        public class Paging
        {
            public Cursors cursors { get; set; }
        }

        public class Comments
        {
            public List<Datum2> data { get; set; }
            public Paging paging { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public string message { get; set; }
            public Comments comments { get; set; }
        }

        public class Paging2
        {
            public string previous { get; set; }
            public string next { get; set; }
        }

        public class Posts
        {
            public List<Datum> data { get; set; }
            public Paging2 paging { get; set; }
        }

        public class RootObject
        {
            public Posts posts { get; set; }
            public string id { get; set; }
        }

    }
}