namespace DiveRollPlatformer
{
    public class PlayerStandardJumpState : PlayerJumpStateBase
    {
        protected override float JumpVelocity => PlayerConstants.StandardJumpVSpeed;
        protected override float Gravity => PlayerConstants.JumpRiseGravity;
        protected override float MinDuration => PlayerConstants.StandardJumpMinDuration;
    }
}
