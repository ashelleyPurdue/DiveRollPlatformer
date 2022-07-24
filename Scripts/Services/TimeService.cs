using Godot;

namespace DiveRollPlatformer
{
    public interface ITimeService
    {
        float Time {get;}
        float PhysicsTime {get;}
    }

    public partial class TimeService : Node, ITimeService
    {
        public float Time {get; private set;}
        public float PhysicsTime {get; private set;}

        public override void _Process(float delta)
        {
            Time += delta;
        }

        public override void _PhysicsProcess(float delta)
        {
            PhysicsTime += delta;
        }
    }
}
