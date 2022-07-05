using Godot;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class PlayerController : KinematicBody
    {
        public Vector3 Velocity;

        public readonly PlayerState WalkState = new PlayerWalkState();
        public readonly PlayerState JumpState = new PlayerJumpState();

        public IInputService Input {get; private set;}

        private PlayerState _currentState = null;

        [Inject]
        public void InjectDependencies(
            IInputService input
        )
        {
            Input = input;
        }

        public override void _Ready()
        {
            ChangeState(WalkState);
        }

        public void ChangeState(PlayerState state)
        {
            _currentState?.OnStateExit();

            _currentState = state;
            _currentState.Player = this;

            _currentState.OnStateEnter();
        }

        public override void _PhysicsProcess(float delta)
        {
            _currentState.BeforeMove(delta);
            MoveAndSlide(Velocity, Vector3.Up);
            _currentState.AfterMove(delta);
        }
    }
}
