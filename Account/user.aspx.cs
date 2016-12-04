using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using FacebookLoginASPnetWebForms.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MiniJson.Adaptor;
using System.Xml;
using System.Runtime.Serialization;
using System.Text;
using System.Runtime.Serialization.Json;

//using Facebook;

namespace FacebookLoginASPnetWebForms.account
{
   

    public class SearchResult
    {
        public string message { get; set; }
        public string id { get; set; }
        public string comments { get; set; }
    }
    public class Comments
    {
        public Data[] data { get; set; }
    }
    public class Data
    {
        public string message { get; set; }
        public string id { get; set; }
    }

    public partial class user : System.Web.UI.Page
    {
        public static XmlDocument doc;

        string ploostuff = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                // Get the Facebook code from the querystring
                if (Request.QueryString["code"] != "")
                {
                    var obj = GetFacebookUserData(Request.QueryString["code"]);

                    ListView1.DataSource = obj;
                    ListView1.DataBind();
                    ploo.Text = ploostuff;
                }
            }
        }

        protected List<FacebookData.User> GetFacebookUserData(string code)
        {
            // Exchange the code for an access token
            Uri targetUri = new Uri("https://graph.facebook.com/oauth/access_token?client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&client_secret=" + ConfigurationManager.AppSettings["FacebookAppSecret"] + "&redirect_uri=http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/account/user.aspx&code=" + code);
            Debug.WriteLine(targetUri.AbsoluteUri.ToString());
            HttpWebRequest at = (HttpWebRequest)HttpWebRequest.Create(targetUri);

            System.IO.StreamReader str = new System.IO.StreamReader(at.GetResponse().GetResponseStream());
            string token = str.ReadToEnd().ToString().Replace("access_token=", "");

            // Split the access token and expiration from the single string
            string[] combined = token.Split('&');
            string accessToken = combined[0];

            // Exchange the code for an extended access token
            Uri eatTargetUri = new Uri("https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&client_secret=" + ConfigurationManager.AppSettings["FacebookAppSecret"] + "&fb_exchange_token=" + accessToken);
            HttpWebRequest eat = (HttpWebRequest)HttpWebRequest.Create(eatTargetUri);

            StreamReader eatStr = new StreamReader(eat.GetResponse().GetResponseStream());
            string eatToken = eatStr.ReadToEnd().ToString().Replace("access_token=", "");

            // Split the access token and expiration from the single string
            string[] eatWords = eatToken.Split('&');
            string extendedAccessToken = eatWords[0];

            // Request the Facebook user information
            Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,last_name,gender,birthday,locale,link&access_token=" + accessToken);
            //Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,posts&access_token=" + "EAACEdEose0cBAHeHlwQTLyAbCoXDZAzpxZCJ3TB24wLxmfzBXh1rP6lvaLWLYZAm72udLR6VHQHHujIW888lZCZA4GOChLnqWINrbCObAMfWLk6Pp80vvCsRvCdS2DQTOnmggO6ujoMlsQXyRAmWGd5DEj2DekkrFgZAXArumy6AZDZD");
            HttpWebRequest user = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

            // Read the returned JSON object response
            StreamReader userInfo = new StreamReader(user.GetResponse().GetResponseStream());
            string jsonResponse = string.Empty;
            jsonResponse = userInfo.ReadToEnd();
            Debug.WriteLine(jsonResponse);
            //ploostuff = jsonResponse;
            // Deserialize and convert the JSON object to the Facebook.User object type
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string jsondata = jsonResponse;
            FacebookData.User converted = sr.Deserialize<FacebookData.User>(jsondata);
            Debug.WriteLine(converted);
            // Write the user data to a List
            List<FacebookData.User> currentUser = new List<FacebookData.User>();

            currentUser.Add(converted);
            Debug.WriteLine(currentUser);

            targetUserUri = new Uri("https://graph.facebook.com/me?fields=posts.fields(message,comments)&access_token=" + accessToken);
            //Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,posts&access_token=" + "EAACEdEose0cBAHeHlwQTLyAbCoXDZAzpxZCJ3TB24wLxmfzBXh1rP6lvaLWLYZAm72udLR6VHQHHujIW888lZCZA4GOChLnqWINrbCObAMfWLk6Pp80vvCsRvCdS2DQTOnmggO6ujoMlsQXyRAmWGd5DEj2DekkrFgZAXArumy6AZDZD");
            HttpWebRequest posts = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

            // Read the returned JSON object response
            StreamReader postsInfo = new StreamReader(posts.GetResponse().GetResponseStream());
            string postsJsonResponse = string.Empty;
            postsJsonResponse = postsInfo.ReadToEnd();
            JObject Jploostuff = JObject.Parse(postsJsonResponse);
            IList<JToken> results = Jploostuff["posts"]["data"].Children().ToList();

            //IList<SearchResult> searchResults = new List<SearchResult>();
            //foreach (JToken result in results)
            //{
            //    SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(result.ToString());
            //    searchResults.Add(searchResult);
            //}
            //foreach (SearchResult item in searchResults)
            //{
            //    Debug.WriteLine(item.message);
            //    Debug.WriteLine(item.comments);
            //    // Debug.WriteLine(item.id);

            //}
            //Debug.WriteLine("");
            //Debug.WriteLine("");

            Debug.WriteLine(postsJsonResponse);
            //FacebookClient fb2 = new FacebookClient();
            //fb2.AppId = "";
            //fb2.AppSecret = "";
            //Facebook.min
            doc = JsonConvert.DeserializeXmlNode((postsJsonResponse), "root");
            HttpUtility.HtmlEncode(doc);
            ploostuff = doc.InnerXml;
            ploostuff = ploostuff.Replace("<data>", "\r\n<data>");
            Debug.WriteLine(doc.InnerXml);
            XmlNodeList xml1 = doc.SelectNodes("/root/posts/data/comments/data/message");
            ploostuff = "";
            foreach (XmlNode xml2 in xml1)
            {
                ploostuff += HttpUtility.HtmlDecode(xml2.InnerText) + "\r\n";
                Debug.WriteLine(xml2.InnerText);
            }

            XmlNodeList xml3 = doc.SelectNodes("/root/posts/data/message");


            foreach (XmlNode xml4 in xml3)
            {
                ListBox1.Items.Add(HttpUtility.HtmlDecode(xml4.InnerText));
            
                Debug.WriteLine(HttpUtility.HtmlDecode(xml4.InnerText));
            }
            //var xml = System.Xml.Linq.XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(postsJsonResponse), new XmlDictionaryReaderQuotas()));


            //var jsonstuff = JsonConvert.DeserializeObject(postsJsonResponse) ; ;
            //// var jsonParser = new JsonParser(postsJsonResponse, true);
            //var jsonstuff2 = JsonConvert.DeserializeObject(jsonstuff[1]);
            //JOb
            //dynamic[] soap = { 1,2}; ;
            // //as Dictionary<string, object>;
            ////= jsonParser.DynamicResult.GetChildren();
            //List<object> entries = soap[1] as List<object>;

            //try
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        Dictionary<string, object> messageData = entries[i] as Dictionary<string, object>;
            //        object resultData = messageData["message"];
            //        Dictionary<string, object> fromData = messageData["from"] as Dictionary<string, object>;
            //        object resultData2 = fromData["name"];
            //        Debug.WriteLine("JSON string : " + resultData.ToString() + "2: " + resultData2.ToString());
            //    }
            //}
            //catch
            //{
            //    Debug.WriteLine("Done!!!");
            //}


            // Return the current Facebook user
            return currentUser;
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = ListBox1.SelectedIndex;
            string mess = ListBox1.SelectedValue;
            mess = mess.Replace("'", "&apos");
//            XmlNodeList xml3 = doc.SelectNodes("(/root/posts/data/message)[text() = '" + mess + "']comments/data/message");
            XmlNodeList xml3 = doc.SelectNodes("(/root/posts/data/message)[text() = '" + mess + "']/../comments/data/message");
            try
            {
                Debug.WriteLine("listbox" + mess + ", " + xml3.Item(1).InnerXml);

            }
            catch { }

            ploo.Text = "";

            foreach (XmlNode xml4 in xml3)
            {
                ploo.Text += HttpUtility.HtmlDecode(xml4.InnerText) + "\r\n";
                Debug.WriteLine(HttpUtility.HtmlDecode(xml4.InnerText));
            }
            ploo.Text += doc.InnerXml; 
        }
        
       
    }
}