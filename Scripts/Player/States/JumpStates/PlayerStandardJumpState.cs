namespace DiveRollPlatformer
{
    public class PlayerStandardJumpState : PlayerJumpStateBase
    {
        protected override float JumpVelocity => PlayerConstants.STANDARD_JUMP_VSPEED;
        protected override float Gravity => PlayerConstants.JUMP_RISE_GRAVITY;
        protected override float MinDuration => PlayerConstants.STANDARD_JUMP_MIN_DURATION;
    }
}
