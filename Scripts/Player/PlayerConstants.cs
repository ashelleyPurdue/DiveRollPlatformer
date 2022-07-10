namespace DiveRollPlatformer
{
    public static class PlayerConstants
    {
        public const float BodyHeight = 1.4558f;
        public const float BodyRadius = 0.375f;

        public const float ShortJumpDecayRate = 0.7f;

        // These constants will determine the initial jump velocity
        // and rising/falling gravity strength
        public const float StandardJumpMaxHeight = 5;
        public const float StandardJumpMinDuration = 0.05f;
        public const float StandardJumpFullRiseTime = 0.404f;
        public const float StandardJumpFullFallTime = 0.328f;

        // If you jump again shortly after you land, you'll do a "double jump."
        // Not the mid-air kind of double-jump, but the "3D Mario" kind.
        public const float DoubleJumpMaxHeight = 8;
        public const float DoubleJumpMinDuration = 0.2f;
        public const float DoubleJumpFSpeedBoost = 2.4f;
        public const float DoubleJumpTimeWindow = 0.1f;

        // Side flip constants
        public const float SideFlipMaxHeight = 7.76f;
        public const float SideFlipMinDuration = 0.5f;

        public const float FSpeedMin = 2;
        public const float FSpeedMaxGround = 8;
        public const float FSpeedMaxAir = 20;

        public const float TerminalVelocityAir = -100;
        public const float TerminalVelocityWallSlide = -10;

        public const float FAccelGround = 15;
        public const float FAccelAir = 30;
        public const float MinLandingFSpeedMult = 0.25f;
        public const float SkidDuration = 0.5f;

        public const float BonkSpeed = -3;
        public const float LedgeGrabVSpeed = 11;
        public const float LedgeGrabFSpeed = 4;
        public const float LedgeGrabDuration = 0.15f;

        public const float DiveJumpHeight = 4;
        public const float DiveGravity = 100;
        public const float DiveFSpeedInitial = 20;
        public const float DiveFSpeedFinalMax = 10;
        public const float DiveFSpeedFinalMin = 5f;
        public const float DiveFSpeedSlowTime = 0.5f;

        public const float BonkStartVSpeed = 10f;
        public const float BonkStartFSpeed = -5f;
        public const float BonkGravity = 50;
        public const float BonkSlowTime = 0.75f;
        public const float BonkDuration = 0.3f;
        public const float BonkMaxBounceCount = 1;
        public const float BonkBounceMultiplier = 0.25f;

        public const float RotSpeedDeg = 360 * 2;
        public const float FrictionWallSlide = 10;

        public const float CoyoteTime = 0.1f;      // Allows you to press the jump button a little "late" and still jump
        public const float EarlyJumpTime = 0.1f;   // Allows you to press the jump button a little "early" and still jump


        public const float WallJumpMinFSpeed = 10;
        public const float WallJumpFSpeedBoost = 1;
        public const float WallJumpHeight = 5.5f;
        public const float WallJumpMinFDist = 1;    // air strafing is disabled
                                                    // after a wall jump until
                                                    // the player has moved this
                                                    // far away from the wall.

        public const float MaxPivotSpeed = 3f; // If you're below this speed, you can pivot on a dime.

        public const float JumpRedirectTime = 0.1f;

        public const float RollTime = 0.25f;
        public const float RollCooldown = 0.5f;
        public const float RollDistance = 5;
        public const float RollJumpFSpeed = 10;
        public const float RollRotSpeedDeg = 180 / RollTime;

        public const float LeftStickDeadzone = 0.001f;

        public static readonly float StandardJumpVSpeed;
        public static readonly float DoubleJumpVSpeed;
        public static readonly float SideFlipVSpeed;
        public static readonly float DiveJumpVSpeed;
        public static readonly float WallJumpVSpeed;
        public static readonly float JumpRiseGravity;
        public static readonly float FreeFallGravity;
        public static float WallSlideGravity => JumpRiseGravity;

        // Using a constant instead of Engine.IterationsPerSecond so that this
        // class can be used in a static constructor.  This constant should
        // match the value configured in the Project Settings, or else the
        // computed gravity/vspeed values won't accurately result in the
        // specified jump height or rise/fall times.
        public const float FixedTimestep = 1 / 60f;

        static PlayerConstants()
        {
            (StandardJumpVSpeed, JumpRiseGravity) = AccelerationMath.InitialVelAndFrictionFor(
                PlayerConstants.StandardJumpMaxHeight,
                PlayerConstants.StandardJumpFullRiseTime,
                PlayerConstants.FixedTimestep
            );

            FreeFallGravity = AccelerationMath.AccelerationForDistanceOverTime(
                PlayerConstants.StandardJumpMaxHeight,
                PlayerConstants.StandardJumpFullFallTime,
                PlayerConstants.FixedTimestep
            );

            DoubleJumpVSpeed = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.DoubleJumpMaxHeight,
                PlayerConstants.JumpRiseGravity,
                PlayerConstants.FixedTimestep
            );

            SideFlipVSpeed = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.SideFlipMaxHeight,
                PlayerConstants.JumpRiseGravity,
                PlayerConstants.FixedTimestep
            );

            DiveJumpVSpeed = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.DiveJumpHeight,
                PlayerConstants.DiveGravity,
                PlayerConstants.FixedTimestep
            );

            WallJumpVSpeed = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.WallJumpHeight,
                PlayerConstants.JumpRiseGravity,
                PlayerConstants.FixedTimestep
            );
        }
    }
}
