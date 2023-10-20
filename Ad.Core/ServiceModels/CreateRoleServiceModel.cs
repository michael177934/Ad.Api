using System;
using System.Collections.Generic;

namespace Ad.Core.Models
{
    public class CreateRoleServiceModel
    {
        public string Name { get; set; }
        public IEnumerable<ControllerServiceModel> SelectedControllers { get; set; }
        public IEnumerable<RoleProviderMenuServiceModel> Menus { get; set; }
        public string CreatedBy { get; set; }
        public Guid TenantId { get; set; }
    }
}