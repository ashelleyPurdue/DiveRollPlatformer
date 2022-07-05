namespace DiveRollPlatformer
{
    public class PlayerWalkState : PlayerState
    {
        private float _stateStartTime;

        public override void OnStateEnter()
        {
            _stateStartTime = Player.Time.PhysicsTime;
        }

        public override void BeforeMove(float deltaTime)
        {
            // Disarm the double-jump after spending too long on the ground
            if (Player.Time.PhysicsTime >= _stateStartTime + PlayerConstants.DOUBLE_JUMP_TIME_WINDOW)
                Player.DoubleJumpArmed = false;

            // Jump with the jump button
            if (JumpButtonBuffered())
                Jump();
        }

        public override void AfterMove(float deltaTime)
        {
            if (!Player.IsOnFloor())
            {
                Player.ChangeState(Player.FreeFallState);
            }
        }

        private void Jump()
        {
            if (!Player.DoubleJumpArmed)
            {
                Player.ChangeState(Player.StandardJumpState);
            }
            else
            {
                Player.ChangeState(Player.DoubleJumpState);
            }
        }

        private bool JumpButtonBuffered()
        {
            float bufferExpiration = Player.LastJumpPressTime + PlayerConstants.EARLY_JUMP_TIME;
            return Player.Time.PhysicsTime < bufferExpiration;
        }
    }
}
