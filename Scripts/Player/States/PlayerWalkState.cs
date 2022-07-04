namespace DiveRollPlatformer
{
    public class PlayerWalkState : PlayerState
    {
        public override void BeforeMove(float deltaTime)
        {
            if (Player.Input.JumpPressed)
            {
                Player.ChangeState(Player.JumpState);
            }
        }

        public override void AfterMove(float deltaTime)
        {
            // TODO: switch to falling state if not on the ground
        }
    }
}
