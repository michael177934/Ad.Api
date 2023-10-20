using System.Collections.Generic;

namespace Ad.API.Resources.Role
{
    public class CreateRoleResource
    {
        public string Name { get; set; }
        public IEnumerable<ControllerResource> SelectedControllers { get; set; }
    }
}