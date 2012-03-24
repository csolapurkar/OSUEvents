using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace OSUEvents
{
    //Holds info for a single Event Source feed
    public class EventsData
    {
        public string source;
        private List<EventItem> _Items = new List<EventItem>();

        public List<EventItem> Items
        {
            get
            {
                return this._Items;
            }
        }
    }

    //Holds info for a single Event
    public class EventItem
    {
        //Windows.Data.Json.JsonObject myobj = new JsonObject();
        private DateTime _startdate;
        private DateTime _enddate;
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string eventtype { get; set; }
        public string eventlink { get; set; }
        public string detailslink { get; set; }
        public string description { get; set; }
        public string contactname { get; set; }
        public string contactemail { get; set; }
        public string contactnumber { get; set; }
        public string location { get; set; }
        public string startdate 
        {
            get 
            {
                return _startdate.ToString();
            }
            set 
            {               
                DateTime.TryParse(value, out _startdate);                
            } 
        }
        public string enddate
        {
            get
            {
                return _enddate.ToString();
            }
            set
            {
                DateTime.TryParse(value, out _enddate);
            }
        }
    }

    // Holds a collection of Event Sources
    public class EventsDataSource
    {
        private ObservableCollection<EventsData> _EventsList = new ObservableCollection<EventsData>();
        public ObservableCollection<EventsData> EventsList
        {
            get
            {
                return this._EventsList;
            }
        }

        public async Task getEventsAsync()
        {
            string url1 = "http://5.130.180.248:3000/get_events";
            EventsData feed1 = await GetEventsFeedData(url1);
            this.EventsList.Add(feed1);
        }

        public async Task<string> GetJsonString(string url)
        {
            HttpClient myclient = new System.Net.Http.HttpClient();
            
            Uri myuri = new Uri(url);
            HttpResponseMessage response = await myclient.GetAsync(myuri);
            string resstr = await response.Content.ReadAsStringAsync();
            return resstr;
        }

        public async Task<EventsData> GetEventsFeedData(string url)
        {
            char[] trimchars = { '\"', ' ', '\t', '\n' };
            try
            {
                string jsonstr = await GetJsonString(url);
                JsonObject myobj = JsonObject.Parse(jsonstr);
                JsonObject myres = myobj.GetNamedObject("result");
                bool status = myres.GetNamedBoolean("status");
                JsonArray data = myres.GetNamedArray("data");

                EventsData eventslist = new EventsData();

                if (status)
                {
                    //get the list of events
                    foreach (var item in data)
                    {
                        //get the list of key, value pairs in each event
                        Dictionary<string, string> tempdict = new Dictionary<string, string>();
                        foreach (var details in item.GetObject())
                        {
                            //textBox1.Text += details.Key + " " + details.Value.Stringify() + "\n";
                            tempdict.Add(details.Key, details.Value.Stringify());
                        }

                        EventItem tempitem = new EventItem();
                        bool res;
                        string resstr;
                        #region regionEventDetails
                        res = tempdict.TryGetValue("category", out resstr);
                        if (res) { tempitem.category = resstr; } resstr = null;

                        res = tempdict.TryGetValue("contactemail", out resstr);
                        if (res) { tempitem.contactemail = resstr; } resstr = null;

                        res = tempdict.TryGetValue("contactname", out resstr);
                        if (res) { tempitem.contactname = resstr; } resstr = null;

                        res = tempdict.TryGetValue("contactnumber", out resstr);
                        if (res) { tempitem.contactnumber = resstr; } resstr = null;

                        res = tempdict.TryGetValue("description", out resstr);
                        if (res) { tempitem.description = resstr; } resstr = null;

                        res = tempdict.TryGetValue("detailslink", out resstr);
                        if (res) { tempitem.detailslink = resstr; } resstr = null;

                        res = tempdict.TryGetValue("end_date", out resstr);
                        if (resstr!= "null" && res) 
                        {                            
                            tempitem.enddate = resstr.Trim(trimchars); 
                        } 
                        resstr = null;

                        res = tempdict.TryGetValue("start_date", out resstr);
                        if (resstr != "null" &&  res)
                        {
                            tempitem.startdate = resstr.Trim(trimchars);
                        }
                        resstr = null;
                        
                        res = tempdict.TryGetValue("id", out resstr);
                        if (res) { tempitem.id = Convert.ToInt32(resstr); } resstr = null;

                        res = tempdict.TryGetValue("eventlink", out resstr);
                        if (res) { tempitem.eventlink = resstr; } resstr = null;

                        res = tempdict.TryGetValue("eventtype", out resstr);
                        if (res) { tempitem.eventtype = resstr; } resstr = null;

                        res = tempdict.TryGetValue("location", out resstr);
                        if (res) { tempitem.location = resstr; } resstr = null;

                        res = tempdict.TryGetValue("name", out resstr);
                        if (res) { tempitem.name = resstr.Trim(trimchars); } resstr = null;

                        
                        /*
                        //tempitem.contactemail = tempdict["contactemail"];
                        //tempitem.contactname = tempdict["contactname"];
                        //tempitem.contactnumber = tempdict["contactnumber"];
                        //tempitem.description = tempdict["description"];
                        //tempitem.detailslink = tempdict["detailslink"];
                        //tempitem.enddate = tempdict["enddate"];
                        //tempitem.id = Convert.ToInt32(tempdict["id"]);
                        //tempitem.eventlink = tempdict["eventlink"];
                        //tempitem.eventtype = tempdict["eventtype"];
                        //tempitem.location = tempdict["location"];
                        //tempitem.name = tempdict["name"];
                        //tempitem.startdate = tempdict["startdate"];
                        //textBox1.Text += "\n";*/
                        #endregion
                        eventslist.Items.Add(tempitem);
                    }
                }
                return eventslist;
            }
            catch (Exception e)
            {                
                throw;
            }
        }    
    }

    public class CategoryData
    {
        private List<CategoryItem> _Items = new List<CategoryItem>();

        public List<CategoryItem> Items
        {
            get
            {
                return this._Items;
            }
        }
    }
    
    public class CategoryDataSource
    {
        private ObservableCollection<CategoryData> _Categories = new ObservableCollection<CategoryData>();

        public ObservableCollection<CategoryData> Categories
        {
            get
            {
                return this._Categories;
            }
        }
        public async Task getCategoriesAsync()
        {
            string url1 = "http://5.130.180.248:3000/get_event_categories";
            CategoryData feed1 = await GetCategoriesData(url1);
            this.Categories.Add(feed1);
        }

        public async Task<string> GetJsonString(string url)
        {
            HttpClient myclient = new System.Net.Http.HttpClient();
            Uri myuri = new Uri(url);
            HttpResponseMessage response = await myclient.GetAsync(myuri);
            string resstr = await response.Content.ReadAsStringAsync();
            return resstr;
        }

        public async Task<CategoryData> GetCategoriesData(string url)
        {
            char[] trimchars = { '\"', ' ', '\t', '\n' };
            try
            {
                string jsonstr = await GetJsonString(url);
                JsonObject myobj = JsonObject.Parse(jsonstr);
                JsonObject myres = myobj.GetNamedObject("result");
                bool status = myres.GetNamedBoolean("status");
                JsonArray data = myres.GetNamedArray("data");

                CategoryData catlist = new CategoryData();

                if (status)
                {
                    //get the list of categories
                    foreach (var item in data)
                    {
                        //get the list of key, value pairs in each Category
                        Dictionary<string, string> tempdict = new Dictionary<string, string>();
                        foreach (var details in item.GetObject())
                        {
                            tempdict.Add(details.Key, details.Value.Stringify());
                        }

                        CategoryItem tempitem = new CategoryItem();
                        bool res;
                        string resstr;
                        #region regionCategoryDetails
                        res = tempdict.TryGetValue("id", out resstr);
                        if (res) { tempitem.id = Convert.ToInt32(resstr); } resstr = null;

                        res = tempdict.TryGetValue("name", out resstr);
                        if (res) { tempitem.name = resstr.Trim(trimchars); } resstr = null;
                        #endregion
                        catlist.Items.Add(tempitem);
                    }
                }
                return catlist;
            }
            catch (Exception e)
            {
                throw;
            }
        }        
    }

    public class CategoryItem
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
