namespace DiveRollPlatformer
{
    public class PlayerDoubleJumpState : PlayerJumpStateBase
    {
        protected override float JumpVelocity => PlayerConstants.DoubleJumpVSpeed;
        protected override float Gravity => PlayerConstants.JumpRiseGravity;
        protected override float MinDuration => PlayerConstants.DoubleJumpMinDuration;

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            // By entering this state, the player expends their double-jump.
            // They'll need to do any other kind of jump again to re-arm it.
            Player.DoubleJumpArmed = false;

            // TODO: Apply a horizontal speed boost when double jumping
        }
    }
}
