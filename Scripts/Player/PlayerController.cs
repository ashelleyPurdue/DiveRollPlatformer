using Godot;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class PlayerController : KinematicBody
    {
        public Vector3 Velocity;

        public PlayerState CurrentState {get; private set;} = null;

        public readonly PlayerState WalkState = new PlayerWalkState();
        public readonly PlayerState FreeFallState = new PlayerFreeFallState();
        public readonly PlayerState StandardJumpState = new PlayerStandardJumpState();

        public IInputService Input {get; private set;}
        public ITimeService Time {get; private set;}

        [Inject]
        public void InjectDependencies(
            IInputService input,
            ITimeService time
        )
        {
            Input = input;
            Time = time;

            ChangeState(WalkState);
        }

        public void ChangeState(PlayerState state)
        {
            CurrentState?.OnStateExit();

            CurrentState = state;
            CurrentState.Player = this;

            CurrentState.OnStateEnter();
        }

        public override void _PhysicsProcess(float delta)
        {
            CurrentState.BeforeMove(delta);
            MoveAndSlide(Velocity, Vector3.Up);
            CurrentState.AfterMove(delta);
        }
    }
}
