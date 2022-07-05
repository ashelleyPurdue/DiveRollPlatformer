using Godot;

namespace DiveRollPlatformer
{
    public abstract class PlayerAirbornStateBase : PlayerState
    {
        protected abstract float Gravity {get;}

        public override void BeforeMove(float deltaTime)
        {
            ApplyGravity(deltaTime);
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

        protected void ApplyGravity(float deltaTime)
        {
            Player.Velocity.y -= Gravity * deltaTime;

            if (Player.Velocity.y < PlayerConstants.TERMINAL_VELOCITY_AIR)
                Player.Velocity.y = PlayerConstants.TERMINAL_VELOCITY_AIR;
        }
    }
}
