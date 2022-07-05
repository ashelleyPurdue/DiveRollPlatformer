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
            CutJumpShortIfReleased();

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
        }

        private void CutJumpShortIfReleased()
        {
            // Cut the jump short if the button was released on the way up.
            // Immediately setting the VSpeed to 0 looks jarring, so instead
            // we'll exponentially decay it every frame.
            // Once it's decayed below a certain threshold, we'll let gravity do
            // the rest of the work so it still looks natural.
            float decayCutoff = PlayerConstants.STANDARD_JUMP_VSPEED / 2;

            bool shouldDecay =
                _jumpReleased &&
                IsPastMinDuration() &&
                Player.Velocity.y > decayCutoff;

            if (shouldDecay)
                Player.Velocity.y *= PlayerConstants.SHORT_JUMP_DECAY_RATE;
        }

        private bool IsPastMinDuration()
        {
            return (Player.Time.PhysicsTime - _jumpStartTime >= MinDuration);
        }
    }
}
