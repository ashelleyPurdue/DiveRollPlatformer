using Godot;

namespace DiveRollPlatformer
{
    public static class AccelerationMath
    {
        /// <summary>
        /// Calculates the initial velocity and friction it would take for an
        /// object to come to a stop after travelling a given distance in a given
        /// amount of time.
        ///
        /// The actual time it takes will be accurate within one fixed timestep.
        /// The actual distance it travels will be accurate within 0.01 units
        /// </summary>
        /// <param name="initialVel"></param>
        /// <param name="distance"></param>
        /// <param name="timeSeconds"></param>
        /// <returns></returns>
        public static (float initialVel, float friction) InitialVelAndFrictionFor(
            float distance,
            float timeSeconds,
            float fixedDeltaTime
        )
        {
            // Round the time taken to the next frame
            int timeFrames = Mathf.CeilToInt(timeSeconds / fixedDeltaTime);
            float adjustedTimeFrames = timeFrames * fixedDeltaTime;

            // Get an initial estimate for how much velocity we'd need
            float fps = 1 / fixedDeltaTime;
            float discreteConverter = fps / (fps + 1);
            float initVelMetersPerFrame = 2 * distance / timeFrames;
            float initVel = initVelMetersPerFrame * fps * discreteConverter;

            // Use brute force to find a friction that will result in it stopping
            // in approximately that distance.
            float friction = BinarySearch(
                0,
                distance * 100,
                0.01f,
                100,
                g => Mathf.Abs(distance - MaxDist(initVel, g, fixedDeltaTime))
            );

            // Remember when I said the initial velocity was an "initial estimate"?
            // Yeah, well, it's also our final estimate.  LOL.
            return (initVel, friction);
        }

        public static float AccelerationForDistanceOverTime(
            float distance,
            float time,
            float fixedDeltaTime
        )
        {
            // Round the time to the next frame
            int timeFrames = Mathf.CeilToInt(time / fixedDeltaTime);
            float adjustedTime = timeFrames * fixedDeltaTime;

            return BinarySearch(
                0,
                distance * 100,
                0.01f,
                100,
                FindError
            );

            float FindError(float accel)
            {
                float actualTime = TimeToTravelDistance(distance, accel, fixedDeltaTime);
                return Mathf.Abs(adjustedTime - actualTime);
            }
        }

        public static float VelocityForDistanceWithFriction(
            float distance,
            float friction,
            float fixedDeltaTime
        )
        {
            return BinarySearch(
                0,
                distance * 100,
                0.01f,
                100,
                FindError
            );

            float FindError(float jumpVel)
            {
                float actualHeight = MaxDist(jumpVel, friction, fixedDeltaTime);
                return Mathf.Abs(actualHeight - distance);
            }
        }

        private static float TimeToTravelDistance(
            float distance,
            float acceleration,
            float fixedDeltaTime
        )
        {
            float v = 0;
            float t = 0;
            for (float y = distance; y > 0; y += v * fixedDeltaTime)
            {
                v -= acceleration * fixedDeltaTime;
                t += fixedDeltaTime;
            }

            return t;
        }

        private static float MaxDist(float initVel, float friction, float fixedDeltaTime)
        {
            float x = 0;
            for (float v = initVel; v > 0; v -= friction * fixedDeltaTime)
                x += v * fixedDeltaTime;

            return x;
        }

        private static float BinarySearch(
            float min,
            float max,
            float errorTolerance,
            int maxIterations,
            System.Func<float, float> findError
        )
        {

            for (int i = 0; i < maxIterations; i++)
            {
                float mid = (min + max) / 2;

                float error = findError(mid);
                if (error <= errorTolerance)
                {
                    GD.Print("Within error tolerance");
                    return mid;
                }

                // Decide whether to take the bottom path or the top path.
                // We'll go with whichever one decreases the error the most.
                // If neither of them cause the error to go down, then we'll use their
                // midpoints instead.
                float maxError = findError((max + mid) / 2);
                float minError = findError((min + mid) / 2);

                float maxErrorDecrease = error - maxError;
                float minErrorDecrease = error - minError;

                if (maxErrorDecrease <= 0 && minErrorDecrease <= 0)
                {
                    min = (min + mid) / 2;
                    max = (mid + max) / 2;
                    continue;
                }
                else if (maxErrorDecrease > minErrorDecrease)
                {
                    min = mid;
                    continue;
                }
                else if (maxErrorDecrease < minErrorDecrease)
                {
                    max = mid;
                    continue;
                }
                else
                {
                    GD.Print("They both have the same error.  Panic!");

                    // They're both the same, so explore both paths and choose the one
                    // with the smallest error
                    int remainingIterations = maxIterations - i;
                    float bottomResult = BinarySearch(min, mid, errorTolerance, remainingIterations / 2, findError);
                    float topResult = BinarySearch(mid, max, errorTolerance, remainingIterations / 2, findError);

                    float bottomError = findError(bottomResult);
                    float topError = findError(topResult);

                    return bottomError < topError
                        ? bottomResult
                        : topResult;
                }
            }

            return (min + max) / 2;
        }
    }
}
