using System;

namespace DiveRollPlatformer.DependencyInjection
{
    /// <summary>
    /// Tagging a class with this attribute will enable property injection for
    /// _all_ public properties in that class.  It will also cause
    /// SiblingNodeInstaller to automatically register it.
    ///
    /// Use this for classes that only act as "service groups"---dumb
    /// collections of services that are bundled together.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceGroupAttribute : Attribute {}
}
