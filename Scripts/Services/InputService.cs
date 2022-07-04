using Godot;

namespace DiveRollPlatformer
{
    public interface IInputService
    {
        Vector2 RightStick {get;}
    }

    public class InputService : IInputService
    {
        public Vector2 RightStick => Input.GetVector(
            "RightStickLeft",
            "RightStickRight",
            "RightStickDown",
            "RightStickUp"
        );
    }
}
