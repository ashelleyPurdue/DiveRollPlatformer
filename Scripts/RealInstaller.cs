using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class RealInstaller : SiblingNodeInstaller
    {
        protected override void RegisterBindings(SimpleInjector.Container container)
        {
            container.Register<IInputService, InputService>();
        }
    }
}
