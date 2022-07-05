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
            if (Player.Input.JumpPressed)
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
        }

        public override void AfterMove(float deltaTime)
        {
            if (!Player.IsOnFloor())
            {
                Player.ChangeState(Player.FreeFallState);
            }
        }
    }
}
