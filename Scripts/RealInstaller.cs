using SimpleInjector;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class RealInstaller : SiblingNodeInstaller
    {
        protected override void RegisterBindings(Container container)
        {
            container.Register<IInputService, InputService>();

            RegisterNode<ITimeService, TimeService>(container);
        }

        private void RegisterNode<TService, TNode>(Container container)
            where TService : class
            where TNode : Godot.Node, TService, new()
        {
            var node = new TNode();
            AddChild(node);
            container.RegisterInstance<TService>(node);
        }
    }
}
