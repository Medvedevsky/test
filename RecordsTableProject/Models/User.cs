using Newtonsoft.Json;

namespace RecordsTable.Models
{
    public class User
    {
        public int Id { get; set; }

        [JsonProperty("user_token")]
        public string UserToken { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        [JsonProperty("is_approved")]
        public bool isApproved { get; set; }
    }
}
