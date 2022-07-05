namespace DiveRollPlatformer
{
    public abstract class PlayerState
    {
        public PlayerController Player {get; set;}

        public virtual void ResetState() {}

        public virtual void OnStateEnter() {}
        public virtual void OnStateExit() {}

        public virtual void BeforeMove(float deltaTime) {}
        public virtual void AfterMove(float deltaTime) {}
    }
}
