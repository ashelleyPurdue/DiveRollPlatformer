using Godot;

namespace DiveRollPlatformer.DependencyInjection
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

        private void InitializeSiblingNodes()
        {
            if (_hasInjected)
                return;
            _hasInjected = true;

            Utils.InitializeNode(GetParent(), Container);
        }
    }
}
