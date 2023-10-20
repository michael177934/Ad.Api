using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Ad.API.Resources;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Tarvcent.API.Resources;

namespace Ad.API.ActionFilters
{
    public interface IControllerDiscoveryService
    {
        IEnumerable<ControllerResource> GetControllers();
    }

    public class ControllerDiscoveryService : IControllerDiscoveryService
    {
        private readonly IActionDescriptorCollectionProvider _provider;
        private List<ControllerResource> _apiControllers;

        public ControllerDiscoveryService(IActionDescriptorCollectionProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<ControllerResource> GetControllers()
        {
            if (_apiControllers != null)
                return _apiControllers;

            _apiControllers = new List<ControllerResource>();

            var items = _provider
                .ActionDescriptors.Items
                .Where(descriptor => descriptor.GetType() == typeof(ControllerActionDescriptor))
                .Select(descriptor => (ControllerActionDescriptor) descriptor)
                .GroupBy(descriptor => descriptor.ControllerTypeInfo.FullName)
                .ToList();

            foreach (var actionDescriptors in items)
            {
                if (!actionDescriptors.Any())
                    continue;


                var actionDescriptor = actionDescriptors.First();
                var controllerTypeInfo = actionDescriptor.ControllerTypeInfo;

                var controllerDisplayName =
                    controllerTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;

                if (controllerDisplayName == null)
                    continue;

                var currentController = new ControllerResource
                {
                    DisplayName = controllerDisplayName,
                    Name = actionDescriptor.ControllerName
                };

                var actions = new List<ControllerActionResource>();
                foreach (var descriptor in actionDescriptors.GroupBy
                    (a => a.ActionName).Select(g => g.First()))
                {
                    var methodInfo = descriptor.MethodInfo;
                    var displayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;

                    if (displayName != null)
                        actions.Add(new ControllerActionResource
                        {
                            ControllerId = currentController.Id,
                            Name = descriptor.AttributeRouteInfo?.Template,
                            DisplayName = displayName
                        });
                }

                currentController.Actions = actions;
                _apiControllers.Add(currentController);
            }

            return _apiControllers;
        }
    }
}