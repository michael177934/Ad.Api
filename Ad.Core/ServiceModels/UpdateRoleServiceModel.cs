using System;
using System.Collections.Generic;

namespace Ad.Core.Models
{
    public class UpdateRoleServiceModel
    {
        public string Name { get; set; }
        public IEnumerable<ControllerServiceModel> SelectedControllers { get; set; }
        public IEnumerable<RoleProviderMenuServiceModel> Menus { get; set; }
        public string ModifiedBy { get; set; }
        public Guid TenantId { get; set; }
    }
}