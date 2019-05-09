using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTApi.Models
{
        public class User
        {
            public string id { get; set; }
            public string name { get; set; }
            public string avatar { get; set; }
            public string country { get; set; }
            public string language { get; set; }
            public string primary_device_os { get; set; }
            public int api_version { get; set; }
            public string viber_version { get; set; }
            public int mcc { get; set; }
            public int mnc { get; set; }
            public string device_type { get; set; }
        }

        public class Sender
        {
            public string id { get; set; }
            public string name { get; set; }
            public string avatar { get; set; }
            public string country { get; set; }
            public string language { get; set; }
            public int api_version { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }

        public class Contact
        {
            public string name { get; set; }
            public string phone_number { get; set; }
        }

        public class Message
        {
            public string type { get; set; }
            public string text { get; set; }
            public string media { get; set; }
            public Location location { get; set; }
            public string tracking_data { get; set; }
            public int sticker_id { get; set; }
            public Contact contact { get; set; }
            public int size { get; set; }
            public string file_name { get; set; }
            public string thumbnail { get; set; }
            public int duration { get; set; }
        }

        public class CallbackData
        {
            public string @event { get; set; }
            public long timestamp { get; set; }
            public string message_token { get; set; }
            public string user_id { get; set; }
            public User user { get; set; }
            public string type { get; set; }
            public string context { get; set; }
            public bool subscribed { get; set; }
            public string desc { get; set; }
            public Sender sender { get; set; }
            public Message message { get; set; }
        }

        #region Server response for send_message event
        /*
        {"status":0,"status_message":"ok","message_token":5104681085634496937}
        */
        #endregion

        public class SendMessageResponse
        {
            public int status { get; set; }
            public string status_message { get; set; }
            public long message_token { get; set; }
        }

        public class GetUserProfileClass
        {
            public int status { get; set; }
            public string status_message { get; set; }
            public long message_token { get; set; }
            public User user { get; set; }
        }
}