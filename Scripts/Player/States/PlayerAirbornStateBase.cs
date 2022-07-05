using Godot;

namespace DiveRollPlatformer
{
    public abstract class PlayerAirbornStateBase : PlayerState
    {
        protected abstract float Gravity {get;}

        public override void BeforeMove(float deltaTime)
        {
            // Apply gravity
            Player.Velocity.y -= Gravity * deltaTime;
            Player.Velocity.y = Mathf.Max(
                Player.Velocity.y,
                PlayerConstants.TERMINAL_VELOCITY_AIR
            );

            // TODO: Air strafing controls
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
