using Godot;

namespace DiveRollPlatformer
{
    public class PlayerJumpState : PlayerAirbornStateBase
    {
        protected override float Gravity => PlayerConstants.JUMP_RISE_GRAVITY;

        public override void OnStateEnter()
        {
            Player.Velocity.y = PlayerConstants.STANDARD_JUMP_VSPEED;
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
