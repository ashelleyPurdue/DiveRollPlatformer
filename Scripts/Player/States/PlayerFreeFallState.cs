using Godot;

namespace DiveRollPlatformer
{
    public class PlayerFreeFallState : PlayerAirbornStateBase
    {
        protected override float Gravity => PlayerConstants.FREE_FALL_GRAVITY;
    }
}
