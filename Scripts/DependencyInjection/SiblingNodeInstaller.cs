using System;
using Godot;

namespace DependencyInjection
{
    public abstract class SiblingNodeInstaller : Node
    {
        protected SimpleInjector.Container Container;
        private bool _hasInjected = false;

        public override void _EnterTree()
        {
            base._EnterTree();
            Container = new SimpleInjector.Container();
            RegisterBindings();

            // We want InitializeSiblingNodes() to run _after_ all of the
            // siblings have been loaded into the scene, but _before_ any of
            // them process.  To achieve this, we call InitializeSiblingNodes()
            // in a _Process() or _PhysicsProcess() callback(whichever runs first)
            // and set this script to be the highest priority.
            this.ProcessPriority = int.MinValue;
        }

        public override void _PhysicsProcess(float delta) => InitializeSiblingNodes();
        public override void _Process(float delta) => InitializeSiblingNodes();

        protected abstract void RegisterBindings();

        protected void RegisterNodeSingleton<TService, TNode>()
            where TService : class
            where TNode : Godot.Node, TService, new()
        {
            var node = new TNode();
            AddChild(node);
            Container.RegisterInstance<TService>(node);
        }

        protected void RegisterSceneSingleton<TService>(string scenePath)
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

        private void InitializeSiblingNodes()
        {
            if (_hasInjected)
                return;
            _hasInjected = true;

            Utils.InitializeNode(GetParent(), Container);
        }
    }
}
