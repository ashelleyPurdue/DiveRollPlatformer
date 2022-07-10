using Godot;

namespace DiveRollPlatformer
{
    public abstract class PlayerAirbornStateBase : PlayerState
    {
        protected void ApplyGravity(float gravity, float deltaTime)
        {
            Player.Velocity.y -= gravity * deltaTime;

            if (Player.Velocity.y < PlayerConstants.TerminalVelocityAir)
                Player.Velocity.y = PlayerConstants.TerminalVelocityAir;
        }

        protected void AirStrafingControls(float deltaTime)
        {
            // Always be facing the left stick.
            // This gives the player the illusion of having more control,
            // without actually affecting their velocity.
            // It also makes it easier to tell which direction they would dive
            // in, if they were to press the dive button right now.
            Player.HAngleDeg = Player.GetHAngleDegInput();

            // Allow the player to redirect their velocity for free for a short
            // time after jumping, in case they pressed the jump button while
            // they were still moving the stick.
            // After that time is up, air strafing controls kick in.
            if (IsInJumpRedirectTimeWindow())
            {
                Player.SyncVelocityToFSpeed();
                return;
            }

            // In the air, we let the player "nudge" their velocity by applying
            // a force in the direction the stick is being pushed.
            // Unlike on the ground, you *will* lose speed and slide around if
            // you try to change your direction.
            var inputVector = Player.GetLeftStickWorldSpace();

            float accel = PlayerConstants.FAccelAir;
            float maxSpeed = PlayerConstants.FSpeedMaxAir;

            // Apply a force to get our new velocity.
            var oldVelocity = Player.Velocity.Flattened();
            var newVelocity = Player.Velocity.Flattened() + (inputVector * accel * deltaTime);

            // Only let the player accellerate up to the normal ground speed.
            // We won't slow them down if they're already going faster than
            // that, though (eg: due to a speed boost from wall jumping)
            float oldSpeed = oldVelocity.Length();
            float newSpeed = newVelocity.Length();

            bool wasAboveGroundSpeedLimit = oldSpeed > PlayerConstants.FSpeedMaxGround;
            bool nowAboveGroundSpeedLimit = newSpeed > PlayerConstants.FSpeedMaxGround;

            if (newSpeed > oldSpeed)
            {
                if (wasAboveGroundSpeedLimit)
                    newSpeed = oldSpeed;
                else if (nowAboveGroundSpeedLimit)
                    newSpeed = PlayerConstants.FSpeedMaxGround;
            }

            // We WILL, however, slow them down if they're going past the max
            // air speed.  That's a hard maximum.
            if (newSpeed > maxSpeed)
                newSpeed = maxSpeed;

            // Apply the changes to the velocity
            Vector3 flatVel = newVelocity.Normalized() * newSpeed;

            float vspeed = Player.Velocity.y;
            Player.Velocity = flatVel;
            Player.Velocity.y = vspeed;

            // Keep FSpeed up-to-date, so it'll be correct when we land.
            Player.FSpeed = flatVel.ComponentAlong(Player.Forward);
        }

        private bool IsInJumpRedirectTimeWindow()
        {
            float endTime = (Player.StateStartTime + PlayerConstants.JumpRedirectTime);
            return Player.Time.PhysicsTime < endTime;
        }
    }
}
