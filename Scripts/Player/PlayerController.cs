using Godot;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class PlayerController : KinematicBody
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

        public Vector3 Velocity;
        public float FSpeed;
        public float HAngleDeg
        {
            get => RotationDegrees.y + 90;
            set
            {
                var rot = RotationDegrees;
                rot.y = value - 90;
                RotationDegrees = rot;
            }
        }

        public bool DoubleJumpArmed;
        public Vector3 Forward => -Transform.basis.z;

        [ServiceGroup]
        public class PlayerServices
        {
            public IInputService Input {get; set;}
            public ITimeService Time {get; set;}
            public IDebugDisplay Debug {get; set;}
        }
        public PlayerServices Services {get; private set;}

        [Inject]
        public void InjectDependencies(PlayerServices services)
        {
            Services = services;
            ChangeState(States.Walk);
        }

        public void ChangeState(PlayerState state)
        {
            CurrentState?.OnStateExit();

            CurrentState = state;
            CurrentState.Player = this;

            StateStartTime = Services.Time.PhysicsTime;
            CurrentState.OnStateEnter();
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Services.Input.JumpPressed)
                LastJumpPressTime = Services.Time.PhysicsTime;

            CurrentState.BeforeMove(delta);
            MoveAndSlide(
                linearVelocity: Velocity,
                upDirection: Vector3.Up,
                stopOnSlope: true
            );
            CurrentState.AfterMove(delta);

            if (IsOnFloor())
                LastGroundedTime = Services.Time.PhysicsTime;

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
            var camera = GetViewport().GetCamera();

            float length = Services.Input.LeftStick.Length();
            float angle = Services.Input.LeftStick.Angle();
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
            Services.Debug.ShowValue("State", CurrentState.GetType().Name);
            Services.Debug.ShowValue("Position", Translation);
            Services.Debug.ShowValue("Velocity", Velocity);
            Services.Debug.ShowValue("Left Stick", Services.Input.LeftStick);
            Services.Debug.ShowValue("Left Stick(world)", GetLeftStickWorldSpace());
            Services.Debug.ShowValue("HAngleDeg", HAngleDeg);
            Services.Debug.ShowValue("HAngleDeg(input)", GetHAngleDegInput());
        }
    }
}
