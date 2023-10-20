using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ad.Core.Models
{
    public class ControllerServiceModel
    {
        [JsonPropertyName("id")]
        public string Id => $"api/{Name}";

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("actions")]
        public IEnumerable<ControllerActionServiceModel> Actions { get; set; }
    }
}