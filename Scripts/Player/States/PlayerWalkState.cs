using Godot;

namespace DiveRollPlatformer
{
    public class PlayerWalkState : PlayerState
    {
        private float _stateStartTime;

        public override void OnStateEnter()
        {
            _stateStartTime = Player.Time.PhysicsTime;
        }

        public override void BeforeMove(float deltaTime)
        {
            // Disarm the double-jump after spending too long on the ground
            if (Player.Time.PhysicsTime >= _stateStartTime + PlayerConstants.DOUBLE_JUMP_TIME_WINDOW)
                Player.DoubleJumpArmed = false;

            // Jump with the jump button
            if (JumpButtonBuffered())
                Jump();

            RotateWithLeftStick();
            // TODO: Accelerate with the left stick
        }

        public override void AfterMove(float deltaTime)
        {
            if (!Player.IsOnFloor())
            {
                Player.ChangeState(Player.FreeFallState);
            }
        }

        private void RotateWithLeftStick()
        {
            // Don't attempt to rotate if the left stick isn't being pushed
            if (Player.Input.LeftStick.Length() < PlayerConstants.LEFT_STICK_DEADZONE)
                return;

            var leftStick3D = Player.GetLeftStickWorldSpace();
            var leftStick2D = new Vector2(leftStick3D.x, leftStick3D.z);
            float desiredHAngle = Mathf.Rad2Deg(leftStick2D.Angle());

            Player.HAngleDeg = desiredHAngle;
            // TODO: Gradually rotate if we're moving faster than the threshold
        }

        private void Jump()
        {
            if (!Player.DoubleJumpArmed)
            {
                Player.ChangeState(Player.StandardJumpState);
            }
            else
            {
                Player.ChangeState(Player.DoubleJumpState);
            }
        }

        private bool JumpButtonBuffered()
        {
            float bufferExpiration = Player.LastJumpPressTime + PlayerConstants.EARLY_JUMP_TIME;
            return Player.Time.PhysicsTime < bufferExpiration;
        }
    }
}
