namespace RapidApiProject.Models
{
    public class GetCityIdViewModel
    {
        public Data[] data { get; set; }

        public class Data
        {
            public string dest_id { get; set; }
        }
    }
}
