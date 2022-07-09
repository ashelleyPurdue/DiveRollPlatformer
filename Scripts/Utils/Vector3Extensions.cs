using Godot;

namespace DiveRollPlatformer
{
    public static class Vector3Extensions
    {
        public static Vector3 Flattened(this Vector3 v)
        {
            v.y = 0;
            return v;
        }

        /// <summary>
        /// Returns the component of this vector along the target vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float ComponentAlong(this Vector3 v, Vector3 target)
        {
            return v.Dot(target) / target.Length();
        }
    }
}
