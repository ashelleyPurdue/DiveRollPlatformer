using Godot;

namespace DiveRollPlatformer
{
    public class PlayerDiveState : PlayerAirbornStateBase
    {
        public override void OnStateEnter()
        {
            Player.DoubleJumpArmed = false;

            Player.HAngleDeg = Player.GetHAngleDegInput();
            Player.FSpeed = PlayerConstants.DIVE_HSPEED_INITIAL;
            Player.Velocity.y = PlayerConstants.DIVE_JUMP_VSPEED;
        }

        public override void BeforeMove(float deltaTime)
        {
            FSpeedControls(deltaTime);
            ApplyGravity(PlayerConstants.DIVE_GRAVITY, deltaTime);
        }

        public override void AfterMove(float deltaTime)
        {
            // TODO: Roll when we hit the ground, instead of walking
            if (Player.IsOnFloor())
            {
                Player.ChangeState(Player.WalkState);
                return;
            }

            // TODO: Bonk if we hit a wall, instead of going into free-fall
            if (Player.IsOnWall())
            {
                Player.ChangeState(Player.FreeFallState);
                return;
            }
        }

        private void FSpeedControls(float deltaTime)
        {
            // Reduce FSpeed until it's at the minimum
            // If the player is pushing backwards on the left stick, reduce the speed
            // faster and let them slow down more
            float initSpeed = PlayerConstants.DIVE_HSPEED_INITIAL;
            float finalSpeed = PlayerConstants.DIVE_HSPEED_FINAL_MAX;
            float slowTime = PlayerConstants.DIVE_HSPEED_SLOW_TIME;

            float stickBackwardsComponent = -Player.LeftStickForwardComponent();
            if (stickBackwardsComponent > 0)
            {
                finalSpeed = Mathf.Lerp(
                    PlayerConstants.DIVE_HSPEED_FINAL_MAX,
                    PlayerConstants.DIVE_HSPEED_FINAL_MIN,
                    stickBackwardsComponent
                );
            }

            float friction = (initSpeed - finalSpeed) / slowTime;
            Player.FSpeed -= friction * deltaTime;
            if (Player.FSpeed < finalSpeed)
                Player.FSpeed = finalSpeed;

            Player.SyncVelocityToFSpeed();
        }
    }
}
