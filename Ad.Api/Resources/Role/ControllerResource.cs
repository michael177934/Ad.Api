using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ad.API.Resources
{
    public class ControllerResource
    {
        [JsonPropertyName("id")]
        public string Id => $"api/{Name}";

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("actions")]
        public IEnumerable<ControllerActionResource> Actions { get; set; }
    }
}