using Godot;

namespace DiveRollPlatformer
{
    public class PlayerFreeFallState : PlayerAirbornStateBase
    {
        private float Gravity => PlayerConstants.FREE_FALL_GRAVITY;

        public override void BeforeMove(float deltaTime)
        {
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
        }
    }
}
