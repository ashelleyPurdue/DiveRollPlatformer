using Godot;

namespace DiveRollPlatformer
{
    public class PlayerJumpCutoffState : PlayerAirbornStateBase
    {
        protected override float Gravity => PlayerConstants.FREE_FALL_GRAVITY;

        public override void BeforeMove(float deltaTime)
        {
            // Immediately setting the VSpeed to 0 looks jarring, so instead
            // we'll exponentially decay it every frame.
            // Once it's decayed below a certain threshold, we'll let gravity do
            // the rest of the work so it still looks natural.
            float decayCutoff = PlayerConstants.STANDARD_JUMP_VSPEED / 2;
            if (Player.Velocity.y > decayCutoff)
            {
                Player.Velocity.y *= PlayerConstants.SHORT_JUMP_DECAY_RATE;
            }
            else
            {
                ApplyGravity(deltaTime);
            }

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
    }
}
