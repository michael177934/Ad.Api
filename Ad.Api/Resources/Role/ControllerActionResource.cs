using System.Text.Json.Serialization;

namespace Ad.API.Resources
{
    public class ControllerActionResource
    {
        [JsonPropertyName("id")]
        public string Id => $"{Name}";

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("controllerId")]
        public string ControllerId { get; set; }
    }
}