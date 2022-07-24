using Godot;
using DependencyInjection;

namespace DiveRollPlatformer
{
    public partial class PlayerController : CharacterBody3D
    {
        public PlayerState CurrentState {get; private set;} = null;
        public readonly PlayerStateCollection States = new PlayerStateCollection();
        public class PlayerStateCollection
        {
            public readonly PlayerState Walk = new PlayerWalkState();
            public readonly PlayerState FreeFall = new PlayerFreeFallState();
            public readonly PlayerState StandardJump = new PlayerStandardJumpState();
            public readonly PlayerState DoubleJump = new PlayerDoubleJumpState();
            public readonly PlayerState Dive = new PlayerDiveState();
        }

        public float StateStartTime {get; private set;}
        public float LastJumpPressTime {get; private set;} = float.MinValue;
        public float LastGroundedTime {get; private set;} = float.MinValue;

        public new Vector3 Velocity;
        public float FSpeed;
        public float HAngleDeg
        {
            get => Mathf.Rad2Deg(GlobalRotation.y) + 90;
            set
            {
                var rot = GlobalRotation;
                rot.y = Mathf.Deg2Rad(value - 90);
                GlobalRotation = rot;
            }
        }

        public bool DoubleJumpArmed;

        public Vector3 Forward => -Transform.basis.z;

        public IInputService Input {get; private set;}
        public ITimeService Time {get; private set;}
        public IDebugDisplay Debug {get; private set;}

        [Inject]
        public void InjectDependencies(
            IInputService input,
            ITimeService time,
            IDebugDisplay debug
        )
        {
            Input = input;
            Time = time;
            Debug = debug;

            ChangeState(States.Walk);
        }

        public void ChangeState(PlayerState state)
        {
            CurrentState?.OnStateExit();

            CurrentState = state;
            CurrentState.Player = this;

            StateStartTime = Time.PhysicsTime;
            CurrentState.OnStateEnter();
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.JumpPressed)
                LastJumpPressTime = Time.PhysicsTime;

            CurrentState.BeforeMove(delta);

            base.Velocity = Velocity;
            MoveAndSlide();

            CurrentState.AfterMove(delta);

            if (IsOnFloor())
                LastGroundedTime = Time.PhysicsTime;

            ShowDebugValues();
        }

        public void SyncVelocityToFSpeed()
        {
            float vspeed = Velocity.y;
            Velocity = Forward * FSpeed;
            Velocity.y = vspeed;
        }

        public Vector3 GetLeftStickWorldSpace()
        {
            var camera = GetViewport().GetCamera3d();

            float length = Input.LeftStick.Length();
            float angle = Input.LeftStick.Angle();
            angle += camera.Rotation.y;

            return new Vector3(
                length * Mathf.Cos(angle),
                0,
                -length * Mathf.Sin(angle)
            );
        }

        public float LeftStickForwardComponent()
        {
            return GetLeftStickWorldSpace().ComponentAlong(Forward);
        }

        public float GetHAngleDegInput()
        {
            var leftStick3D = GetLeftStickWorldSpace();
            var leftStick2D = new Vector2(leftStick3D.x, -leftStick3D.z);

            return Mathf.Rad2Deg(leftStick2D.Angle());
        }

        private void ShowDebugValues()
        {
            Debug.ShowValue("State", CurrentState.GetType().Name);
            Debug.ShowValue("Position", GlobalPosition);
            Debug.ShowValue("Velocity", Velocity);
            Debug.ShowValue("Left Stick", Input.LeftStick);
            Debug.ShowValue("Left Stick(world)", GetLeftStickWorldSpace());
            Debug.ShowValue("HAngleDeg", HAngleDeg);
            Debug.ShowValue("HAngleDeg(input)", GetHAngleDegInput());
        }
    }
}
