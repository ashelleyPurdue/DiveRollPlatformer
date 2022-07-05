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
            // TODO: Use lower gravity when rising
            Player.Velocity.y -= PlayerConstants.FREE_FALL_GRAVITY * deltaTime;
            Player.Velocity.y = Mathf.Max(
                Player.Velocity.y,
                PlayerConstants.TERMINAL_VELOCITY_AIR
            );

            // TODO: Air strafing controls
        }

        public override void AfterMove(float deltaTime)
        {
            if (Player.IsOnFloor())
                Player.ChangeState(Player.WalkState);
        }
    }
}
