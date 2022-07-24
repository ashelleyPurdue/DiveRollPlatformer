using System;
using Godot;
using SimpleInjector;
using DependencyInjection;

namespace DiveRollPlatformer
{
    public partial class RealInstaller : SiblingNodeInstaller
    {
        protected override void RegisterBindings()
        {
            Container.Register<IInputService, InputService>();

            RegisterNodeSingleton<ITimeService, TimeService>();
            RegisterSceneSingleton<IDebugDisplay>("res://Prefabs/DebugDisplay.tscn");
        }
    }
}
