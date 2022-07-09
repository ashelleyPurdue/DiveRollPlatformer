using SimpleInjector;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class RealInstaller : SiblingNodeInstaller
    {
        protected override void RegisterBindings()
        {
            Container.Register<IInputService, InputService>();

            RegisterNode<ITimeService, TimeService>();
        }

        private void RegisterNode<TService, TNode>()
            where TService : class
            where TNode : Godot.Node, TService, new()
        {
            var node = new TNode();
            AddChild(node);
            Container.RegisterInstance<TService>(node);
        }
    }
}
