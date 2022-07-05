namespace DiveRollPlatformer
{
    public static class AngleMath
    {
        /// <summary>
        /// Returns the shortest distance (in degrees) between two angles.
        /// </summary>
        public static float DeltaAngleDeg(float current, float target)
        {
            float diff = ( target - current + 180 ) % 360 - 180;
            return diff < -180 ? diff + 360 : diff;
        }
    }
}
