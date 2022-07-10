using Godot;

namespace DiveRollPlatformer
{
    public class PlayerWalkState : PlayerState
    {
        private float _stateStartTime;

        public override void OnStateEnter()
        {
            _stateStartTime = Player.Time.PhysicsTime;

            // Ensure we're always "pushing" into the ground, so IsOnFloor()
            // doesn't flicker.
            Player.Velocity.y = -1;
        }

        public override void BeforeMove(float deltaTime)
        {
            // Disarm the double-jump after spending too long on the ground
            if (Player.Time.PhysicsTime >= _stateStartTime + PlayerConstants.DOUBLE_JUMP_TIME_WINDOW)
                Player.DoubleJumpArmed = false;

            // Jump with the jump button
            if (JumpButtonBuffered())
            {
                var targetState = Player.DoubleJumpArmed
                    ? Player.States.DoubleJump
                    : Player.States.StandardJump;

                Player.ChangeState(targetState);
                targetState.BeforeMove(deltaTime);
                return;
            }

            // Dive with the dive button
            if (Player.Input.DivePressed)
            {
                Player.ChangeState(Player.States.Dive);
                Player.States.Dive.BeforeMove(deltaTime);
                return;
            }

            RotateWithLeftStick();
            AccelerateWithLeftStick();

            Player.SyncVelocityToFSpeed();
        }

        public override void AfterMove(float deltaTime)
        {
            if (!Player.IsOnFloor())
            {
                Player.ChangeState(Player.States.FreeFall);
            }
        }

        private void RotateWithLeftStick()
        {
            // Don't attempt to rotate if the left stick isn't being pushed
            if (Player.Input.LeftStick.Length() < PlayerConstants.LEFT_STICK_DEADZONE)
                return;

            float desiredHAngle = Player.GetHAngleDegInput();
            Player.HAngleDeg = desiredHAngle;
            // TODO: Gradually rotate if we're moving faster than the threshold
        }

        private void AccelerateWithLeftStick()
        {
            float stickMagnitude = Player.Input.LeftStick.Length();
            float desiredSpeed = stickMagnitude * PlayerConstants.HSPEED_MAX_GROUND;

            Player.FSpeed = desiredSpeed;
            // TODO: Gradually accelerate instead of instantly
        }

        private bool JumpButtonBuffered()
        {
            float bufferExpiration = Player.LastJumpPressTime + PlayerConstants.EARLY_JUMP_TIME;
            return Player.Time.PhysicsTime < bufferExpiration;
        }
    }
}
