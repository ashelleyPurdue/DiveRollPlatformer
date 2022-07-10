using System;
using System.Reflection;
using System.Linq;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace DiveRollPlatformer.DependencyInjection
{
    public class InjectPropertiesAttributePropertySelectionBehavior : IPropertySelectionBehavior
    {
        public bool SelectProperty(
            Type implementationType,
            PropertyInfo prop
        )
        {
            return implementationType
                .GetCustomAttributes<InjectPropertiesAttribute>()
                .Any();
        }
    }
}
