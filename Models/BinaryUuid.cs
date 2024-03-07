using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BinaryUuid
    {
        public BinaryUuid() {
            Base64 = string.Empty;
            SubType = string.Empty;
        }
        [JsonProperty("base64")]
        public string Base64 { get; set; }
        [JsonProperty("subType")]
        public string SubType { get; set; }

        public Guid ToGuid()
        {
            return new Guid(Convert.FromBase64String(Base64));
        }
    }
}
