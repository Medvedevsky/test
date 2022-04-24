using Newtonsoft.Json;

namespace RecordsTable.Models
{
    public class Seanses
    {
        public string Time { get; set; }

        [JsonProperty("is_free")]
        public bool IsFree { get; set; }
    }
}
