using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RestCountry
    {
        public RestCountry()
        {
            Name = string.Empty;
            Region = string.Empty;
        }
        public string Name { get; set; }
        public string Region { get; set; }

    }

}
