using System;
using System.Reflection;
using System.Linq;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace DiveRollPlatformer.DependencyInjection
{
    public class ServiceGroupPropertySelectionBehavior : IPropertySelectionBehavior
    {
        public bool SelectProperty(
            Type implementationType,
            PropertyInfo prop
        )
        {
            return implementationType
                .GetCustomAttributes<ServiceGroupAttribute>()
                .Any();
        }
    }
}
