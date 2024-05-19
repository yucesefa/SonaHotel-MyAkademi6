namespace RapidApiProject.Models
{
    public class SearchHotelDestination
    {
        public Data[] data { get; set; }

        public class Data
        {
            public string dest_id { get; set; }
        }
    }
}
