using System;
using Godot;
using SimpleInjector;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class RealInstaller : SiblingNodeInstaller
    {
        protected override void RegisterBindings()
        {
            Container.Register<IInputService, InputService>();

            RegisterNodeSingleton<ITimeService, TimeService>();
        }

        private void RegisterNodeSingleton<TService, TNode>()
            where TService : class
            where TNode : Godot.Node, TService, new()
        {
            var node = new TNode();
            AddChild(node);
            Container.RegisterInstance<TService>(node);
        }

        private void RegisterSceneSingleton<TService>(string scenePath)
            where TService : class
        {
            Container.RegisterSingleton<TService>(Factory);

            TService Factory()
            {
                var node = GD.Load<PackedScene>(scenePath).Instance();

                if (!(node is TService s))
                    throw new Exception($"The root node of {scenePath} does not implement {typeof(TService).Name}");

                Utils.InitializeNode(node, Container);

                AddChild(node);
                return s;
            }
        }
    }
}
