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

        public class Posts
        {
            public Data data { get; set; }

        }
        public class Data
        {
            public Story[] story { get; set; }
            public Message[] message { get; set; }

        }

            public class Story
            {

            }

            public class Message
            {

            }
        
    }
}