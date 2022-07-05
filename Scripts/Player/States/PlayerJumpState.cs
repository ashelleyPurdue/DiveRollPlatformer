using Godot;

namespace DiveRollPlatformer
{
    public class PlayerJumpState : PlayerAirbornStateBase
    {
        private float Gravity => PlayerConstants.JUMP_RISE_GRAVITY;
        private const float MinDuration = PlayerConstants.STANDARD_JUMP_MIN_DURATION;

        private float _jumpStartTime;
        private bool _jumpReleased;

        public override void OnStateEnter()
        {
            Player.Velocity.y = PlayerConstants.STANDARD_JUMP_VSPEED;

            _jumpStartTime = Player.Time.PhysicsTime;
            _jumpReleased = false;
        }

        public override void BeforeMove(float deltaTime)
        {
            _jumpReleased = _jumpReleased || !Player.Input.JumpHeld;

            ApplyGravity(Gravity, deltaTime);
            // TODO: Air strafing
        }

        public override void AfterMove(float deltaTime)
        {
            if (Player.IsOnFloor())
            {
                Player.ChangeState(Player.WalkState);
                return;
            }

            if (Player.Velocity.y <= 0)
            {
                Player.ChangeState(Player.FreeFallState);
                return;
            }

            // Cut the jump short if the button was released on the way up
            if (_jumpReleased && IsPastMinDuration())
            {
                Player.ChangeState(Player.JumpCutoffState);
                return;
            }
        }

        private bool IsPastMinDuration()
        {
            return (Player.Time.PhysicsTime - _jumpStartTime >= MinDuration);
        }
    }
}
