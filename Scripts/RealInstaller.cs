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
    }
}
