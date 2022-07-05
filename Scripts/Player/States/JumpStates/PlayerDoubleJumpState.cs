namespace DiveRollPlatformer
{
    public class PlayerDoubleJumpState : PlayerJumpStateBase
    {
        protected override float JumpVelocity => PlayerConstants.DOUBLE_JUMP_VSPEED;
        protected override float Gravity => PlayerConstants.JUMP_RISE_GRAVITY;
        protected override float MinDuration => PlayerConstants.DOUBLE_JUMP_MIN_DURATION;

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
