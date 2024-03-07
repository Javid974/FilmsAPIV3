using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Country
    {
        public Country()
        {
            Iso_3166_1 = string.Empty;
            English_name = string.Empty;
            Native_name = string.Empty;
            Region = string.Empty;
        }
        [Key]
        public string Iso_3166_1 { get; set; }
        public string English_name { get; set; }
        public string Native_name { get; set; }
        public string Region { get; set; }
    }

}