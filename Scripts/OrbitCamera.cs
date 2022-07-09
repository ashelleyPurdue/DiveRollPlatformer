using Godot;
using DiveRollPlatformer.DependencyInjection;

namespace DiveRollPlatformer
{
    public class OrbitCamera : Spatial
    {
        // Inspector parameters
        [Export] public NodePath TargetPath;
        private Spatial _target;

        // Constants
        private const bool INVERT_HORIZONTAL = true;
        private const bool INVERT_VERTICAL = true;

        private const float MAX_ROTSPEED_DEG = 180;
        private const float MIN_VANGLE_DEG = -89.99f;
        private const float MAX_VANGLE_DEG = 89.99f;
        private const float ORBIT_RADIUS = 15;
        private const float ZOOM_IN_SPEED = 50;
        private const float ZOOM_OUT_SPEED = 10;

        // Services
        private IInputService _input;

        // State variables
        private float _hAngleDeg = -90;
        private float _vAngleDeg = MAX_VANGLE_DEG;
        private float _zoomDistance = ORBIT_RADIUS;

        [Inject]
        public void InjectDependencies(IInputService input)
        {
            _input = input;
        }

        public override void _Ready()
        {
            _target = GetNode<Spatial>(TargetPath);
        }

        public override void _Process(float deltaTime)
        {
            // Adjust the angles with the right stick
            Vector2 rightStick = _input.RightStick;
            if (INVERT_HORIZONTAL) rightStick.x *= -1;
            if (INVERT_VERTICAL) rightStick.y *= -1;

            _hAngleDeg += rightStick.x * MAX_ROTSPEED_DEG * deltaTime;
            _vAngleDeg += rightStick.y * MAX_ROTSPEED_DEG * deltaTime;

            if (_vAngleDeg > MAX_VANGLE_DEG)
                _vAngleDeg = MAX_VANGLE_DEG;

            if (_vAngleDeg < MIN_VANGLE_DEG)
                _vAngleDeg = MIN_VANGLE_DEG;

            // Calculate the where the position would be (if we didn't zoom)
            Vector3 dir = SphericalToCartesian(_hAngleDeg, _vAngleDeg, 1);
            Vector3 pos = _target.GlobalTransform.origin + (dir * ORBIT_RADIUS);

            // TODO: zoom in if the camera is obstructed

            // Jump to the position and look at the thing
            LookAtFromPosition(pos, _target.GlobalTransform.origin, Vector3.Up);
        }

        private Vector3 SphericalToCartesian(float hAngleDeg, float vAngleDeg, float orbitRaidus)
        {
            float hAngleRad = Mathf.Deg2Rad(hAngleDeg);
            float vAngleRad = Mathf.Deg2Rad(vAngleDeg);

            float hRadius = Mathf.Cos(vAngleRad) * orbitRaidus;
            float height = Mathf.Sin(vAngleRad) * orbitRaidus;

            return new Vector3(
                Mathf.Cos(hAngleRad) * hRadius,
                height,
                Mathf.Sin(hAngleRad) * hRadius
            );
        }
    }
}
