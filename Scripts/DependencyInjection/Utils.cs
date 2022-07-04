using System;
using System.Reflection;
using System.Linq;
using Godot;

namespace DiveRollPlatformer.DependencyInjection
{
    public static class Utils
    {
        public static void InitializeNode(Node obj, IServiceProvider services)
        {
            // Call recursively on all children
            for (int i = 0; i < obj.GetChildCount(); i++)
                InitializeNode(obj.GetChild(i), services);

            // Inject dependencies for this one.
            var injectMethod = obj
                .GetType()
                .GetRuntimeMethods()
                .Where(m => m.GetCustomAttributes<InjectAttribute>().Any())
                .FirstOrDefault();

            if (injectMethod == null)
                return;

            var parameterValues = injectMethod
                .GetParameters()
                .Select(p => p.ParameterType)
                .Select(t => services.GetService(t))
                .ToArray();

            injectMethod.Invoke(obj, parameterValues);
        }
    }
}
