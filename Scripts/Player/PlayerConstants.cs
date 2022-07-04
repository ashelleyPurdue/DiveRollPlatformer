using Godot;

namespace DiveRollPlatformer
{
    public static class PlayerConstants
    {
        public const float BODY_HEIGHT = 1.4558f;
        public const float BODY_RADIUS = 0.375f;

        public const float SHORT_JUMP_DECAY_RATE = 0.7f;

        // These constants will determine the initial jump velocity
        // and rising/falling gravity strength
        public const float STANDARD_JUMP_MAX_HEIGHT = 5;
        public const float STANDARD_JUMP_MIN_DURATION = 0.05f;
        public const float STANDARD_JUMP_FULL_RISE_TIME = 0.404f;
        public const float STANDARD_JUMP_FULL_FALL_TIME = 0.328f;

        // If you jump again shortly after you land, you'll do a "double jump."
        // Not the mid-air kind of double-jump, but the "3D Mario" kind.
        public const float DOUBLE_JUMP_MAX_HEIGHT = 8;
        public const float DOUBLE_JUMP_MIN_DURATION = 0.2f;
        public const float DOUBLE_JUMP_HSPEED_BOOST = 2.4f;
        public const float DOUBLE_JUMP_TIME_WINDOW = 0.1f;

        // Side flip constants
        public const float SIDE_FLIP_MAX_HEIGHT = 7.76f;
        public const float SIDE_FLIP_MIN_DURATION = 0.5f;

        public const float HSPEED_MIN = 2;
        public const float HSPEED_MAX_GROUND = 8;
        public const float HSPEED_MAX_AIR = 20;

        public const float TERMINAL_VELOCITY_AIR = -100;
        public const float TERMINAL_VELOCITY_WALL_SLIDE = -10;

        public const float HACCEL_GROUND = 15;
        public const float HACCEL_AIR = 30;
        public const float MIN_LANDING_HSPEED_MULT = 0.25f;
        public const float SKID_DURATION = 0.5f;

        public const float BONK_SPEED = -3;
        public const float LEDGE_GRAB_VSPEED = 11;
        public const float LEDGE_GRAB_HSPEED = 4;
        public const float LEDGE_GRAB_DURATION = 0.15f;

        public const float DIVE_JUMP_HEIGHT = 4;
        public const float DIVE_GRAVITY = 100;
        public const float DIVE_HSPEED_INITIAL = 20;
        public const float DIVE_HSPEED_FINAL_MAX = 10;
        public const float DIVE_HSPEED_FINAL_MIN = 5f;
        public const float DIVE_HSPEED_SLOW_TIME = 0.5f;

        public const float BONK_START_VSPEED = 10f;
        public const float BONK_START_HSPEED = -5f;
        public const float BONK_GRAVITY = 50;
        public const float BONK_SLOW_TIME = 0.75f;
        public const float BONK_DURATION = 0.3f;
        public const float BONK_MAX_BOUNCE_COUNT = 1;
        public const float BONK_BOUNCE_MULTIPLIER = 0.25f;

        public const float ROT_SPEED_DEG = 360 * 2;
        public const float FRICTION_WALL_SLIDE = 10;

        public const float COYOTE_TIME = 0.1f;      // Allows you to press the jump button a little "late" and still jump
        public const float EARLY_JUMP_TIME = 0.1f;  // Allows you to press the jump button a little "early" and still jump


        public const float WALL_JUMP_MIN_HSPEED = 10;
        public const float WALL_JUMP_HSPEED_BOOST = 1;
        public const float WALL_JUMP_HEIGHT = 5.5f;
        public const float WALL_JUMP_MIN_HDIST = 1; // air strafing is disabled
                                                    // after a wall jump until
                                                    // the player has moved this
                                                    // far away from the wall.

        public const float MAX_PIVOT_SPEED = 3f; // If you're below this speed, you can pivot on a dime.

        public const float JUMP_REDIRECT_TIME = 0.1f;

        public const float ROLL_TIME = 0.25f;
        public const float ROLL_COOLDOWN = 0.5f;
        public const float ROLL_DISTANCE = 5;
        public const float ROLL_JUMP_HSPEED = 10;
        public const float ROLL_ROT_SPEED_DEG = 180 / ROLL_TIME;

        public const float LEFT_STICK_DEADZONE = 0.001f;

        public static readonly float STANDARD_JUMP_VSPEED;
        public static readonly float DOUBLE_JUMP_VSPEED;
        public static readonly float SIDE_FLIP_VSPEED;
        public static readonly float DIVE_JUMP_VSPEED;
        public static readonly float WALL_JUMP_VSPEED;
        public static readonly float JUMP_RISE_GRAVITY;
        public static readonly float FREE_FALL_GRAVITY;
        public static float WALL_SLIDE_GRAVITY => JUMP_RISE_GRAVITY;

        // Using a constant instead of Engine.IterationsPerSecond so that this
        // class can be used in a static constructor.  This constant should
        // match the value configured in the Project Settings, or else the
        // computed gravity/vspeed values won't accurately result in the
        // specified jump height or rise/fall times.
        public const float FIXED_TIMESTEP = 1 / 60f;

        static PlayerConstants()
        {
            (STANDARD_JUMP_VSPEED, JUMP_RISE_GRAVITY) = AccelerationMath.InitialVelAndFrictionFor(
                PlayerConstants.STANDARD_JUMP_MAX_HEIGHT,
                PlayerConstants.STANDARD_JUMP_FULL_RISE_TIME,
                PlayerConstants.FIXED_TIMESTEP
            );

            FREE_FALL_GRAVITY = AccelerationMath.AccelerationForDistanceOverTime(
                PlayerConstants.STANDARD_JUMP_MAX_HEIGHT,
                PlayerConstants.STANDARD_JUMP_FULL_FALL_TIME,
                PlayerConstants.FIXED_TIMESTEP
            );

            DOUBLE_JUMP_VSPEED = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.DOUBLE_JUMP_MAX_HEIGHT,
                PlayerConstants.JUMP_RISE_GRAVITY,
                PlayerConstants.FIXED_TIMESTEP
            );

            SIDE_FLIP_VSPEED = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.SIDE_FLIP_MAX_HEIGHT,
                PlayerConstants.JUMP_RISE_GRAVITY,
                PlayerConstants.FIXED_TIMESTEP
            );

            DIVE_JUMP_VSPEED = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.DIVE_JUMP_HEIGHT,
                PlayerConstants.DIVE_GRAVITY,
                PlayerConstants.FIXED_TIMESTEP
            );

            WALL_JUMP_VSPEED = AccelerationMath.VelocityForDistanceWithFriction(
                PlayerConstants.WALL_JUMP_HEIGHT,
                PlayerConstants.JUMP_RISE_GRAVITY,
                PlayerConstants.FIXED_TIMESTEP
            );
        }
    }
}
