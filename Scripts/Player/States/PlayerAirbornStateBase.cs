using Godot;

namespace DiveRollPlatformer
{
    public abstract class PlayerAirbornStateBase : PlayerState
    {
        protected void ApplyGravity(float gravity, float deltaTime)
        {
            Player.Velocity.y -= gravity * deltaTime;

            if (Player.Velocity.y < PlayerConstants.TERMINAL_VELOCITY_AIR)
                Player.Velocity.y = PlayerConstants.TERMINAL_VELOCITY_AIR;
        }
    }
}
