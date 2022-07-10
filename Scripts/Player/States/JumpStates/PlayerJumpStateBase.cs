using Godot;

namespace DiveRollPlatformer
{
    public abstract class PlayerJumpStateBase : PlayerAirbornStateBase
    {
        protected abstract float JumpVelocity {get;}
        protected abstract float Gravity {get;}
        protected abstract float MinDuration {get;}

        private float _jumpStartTime;
        private bool _jumpReleased;

        public override void OnStateEnter()
        {
            Player.Velocity.y = JumpVelocity;
            Player.DoubleJumpArmed = true;

            _jumpStartTime = Player.Time.PhysicsTime;
            _jumpReleased = false;
        }

        public override void BeforeMove(float deltaTime)
        {
            // Dive when the player presses the button.
            // Putting this logic in BeforeMove() instead of AfterMove()
            // reduces the perceived input delay by 1 physics frame.
            if (Player.Input.DivePressed)
            {
                Player.ChangeState(Player.States.Dive);
                Player.States.Dive.BeforeMove(deltaTime);
                return;
            }

            _jumpReleased = _jumpReleased || !Player.Input.JumpHeld;
            CutJumpShortIfReleased();

            ApplyGravity(Gravity, deltaTime);
            AirStrafingControls(deltaTime);
        }

        public override void AfterMove(float deltaTime)
        {
            if (Player.IsOnFloor())
            {
                Player.ChangeState(Player.States.Walk);
                return;
            }

            if (Player.Velocity.y <= 0)
            {
                Player.ChangeState(Player.States.FreeFall);
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
            float decayCutoff = PlayerConstants.StandardJumpVSpeed / 2;

            bool shouldDecay =
                _jumpReleased &&
                IsPastMinDuration() &&
                Player.Velocity.y > decayCutoff;

            if (shouldDecay)
                Player.Velocity.y *= PlayerConstants.ShortJumpDecayRate;
        }

        private bool IsPastMinDuration()
        {
            return (Player.Time.PhysicsTime - _jumpStartTime >= MinDuration);
        }
    }
}
