using Godot;

namespace DiveRollPlatformer
{
    public class PlayerJumpState : PlayerState
    {
        public override void OnStateEnter()
        {
            Player.Velocity.y = PlayerConstants.STANDARD_JUMP_VSPEED;
        }

        public override void BeforeMove(float deltaTime)
        {
            // Apply gravity
            Player.Velocity.y -= PlayerConstants.JUMP_RISE_GRAVITY * deltaTime;
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

            if (Player.Velocity.y <= 0 || !Player.Input.JumpHeld)
            {
                Player.ChangeState(Player.FreeFallState);
                return;
            }
        }
    }
}
