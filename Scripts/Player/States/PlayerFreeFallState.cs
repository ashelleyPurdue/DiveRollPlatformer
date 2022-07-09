using Godot;

namespace DiveRollPlatformer
{
    public class PlayerFreeFallState : PlayerAirbornStateBase
    {
        private float Gravity => PlayerConstants.FREE_FALL_GRAVITY;

        public override void BeforeMove(float deltaTime)
        {
            // Dive when the player presses the button.
            // Putting this logic in BeforeMove() instead of AfterMove()
            // reduces the perceived input delay by 1 physics frame.
            if (Player.Input.DivePressed)
            {
                Player.ChangeState(Player.DiveState);
                Player.DiveState.BeforeMove(deltaTime);
                return;
            }

            ApplyGravity(Gravity, deltaTime);
            AirStrafingControls(deltaTime);
        }

        public override void AfterMove(float deltaTime)
        {
            if (Player.IsOnFloor())
            {
                Player.ChangeState(Player.WalkState);
                return;
            }
        }
    }
}
