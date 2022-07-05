using Godot;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class PlayerController : KinematicBody
    {
        public Vector3 Velocity;
        public float HAngleDeg
        {
            get => RotationDegrees.y;
            set
            {
                var rot = RotationDegrees;
                rot.y = value;
                RotationDegrees = rot;
            }
        }
        public bool DoubleJumpArmed;

        public float LastJumpPressTime {get; private set;} = float.MinValue;
        public float LastGroundedTime {get; private set;} = float.MinValue;

        public PlayerState CurrentState {get; private set;} = null;

        public readonly PlayerState WalkState = new PlayerWalkState();
        public readonly PlayerState FreeFallState = new PlayerFreeFallState();
        public readonly PlayerState StandardJumpState = new PlayerStandardJumpState();
        public readonly PlayerState DoubleJumpState = new PlayerDoubleJumpState();

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
            if (Input.JumpPressed)
                LastJumpPressTime = Time.PhysicsTime;

            CurrentState.BeforeMove(delta);
            MoveAndSlide(Velocity, Vector3.Up);
            CurrentState.AfterMove(delta);

            if (IsOnFloor())
                LastGroundedTime = Time.PhysicsTime;
        }

        public Vector3 GetLeftStickWorldSpace()
        {
            var camera = GetViewport().GetCamera();

            float length = Input.LeftStick.Length();
            float angle = Input.LeftStick.Angle();
            angle += camera.Rotation.y;

            return new Vector3(
                -length * Mathf.Sin(angle),
                0,
                length * Mathf.Cos(angle)
            );
        }
    }
}
