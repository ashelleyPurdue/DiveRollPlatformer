using Godot;

namespace DiveRollPlatformer
{
    public class PlayerDiveState : PlayerAirbornStateBase
    {
        public override void OnStateEnter()
        {
            Player.DoubleJumpArmed = false;

            Player.HAngleDeg = Player.GetHAngleDegInput();
            Player.FSpeed = PlayerConstants.DiveFSpeedInitial;
            Player.Velocity.y = PlayerConstants.DiveJumpVSpeed;
        }

        public override void BeforeMove(float deltaTime)
        {
            FSpeedControls(deltaTime);
            ApplyGravity(PlayerConstants.DiveGravity, deltaTime);
        }

        public override void AfterMove(float deltaTime)
        {
            // TODO: Roll when we hit the ground, instead of walking
            if (Player.IsOnFloor())
            {
                Player.ChangeState(Player.States.Walk);
                return;
            }

            // TODO: Bonk if we hit a wall, instead of going into free-fall
            if (Player.IsOnWall())
            {
                Player.ChangeState(Player.States.FreeFall);
                return;
            }
        }

        private void FSpeedControls(float deltaTime)
        {
            // Reduce FSpeed until it's at the minimum
            // If the player is pushing backwards on the left stick, reduce the speed
            // faster and let them slow down more
            float initSpeed = PlayerConstants.DiveFSpeedInitial;
            float finalSpeed = PlayerConstants.DiveFSpeedFinalMax;
            float slowTime = PlayerConstants.DiveFSpeedSlowTime;

            float stickBackwardsComponent = -Player.LeftStickForwardComponent();
            if (stickBackwardsComponent > 0)
            {
                finalSpeed = Mathf.Lerp(
                    PlayerConstants.DiveFSpeedFinalMax,
                    PlayerConstants.DiveFSpeedFinalMin,
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
