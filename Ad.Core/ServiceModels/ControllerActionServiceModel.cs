using System.Text.Json.Serialization;

namespace Ad.Core.Models
{
    public class ControllerActionServiceModel
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