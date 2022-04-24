using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsTable.Models
{
    public class Record
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [JsonProperty("seance_length")]
        public int SeanceLength { get; set; }

        public Staff Staff { get; set; }

        public Client Client { get; set; }

        public string Comment { get; set; }

        [JsonProperty("resource_instance_ids")]
        public List<int> ResourceInstanceIds { get; set; }
    }
}
