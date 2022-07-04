using Godot;

namespace DiveRollPlatformer
{
    public interface IInputService
    {
        Vector2 RightStick {get;}

        bool JumpHeld {get;}
        bool JumpPressed {get;}
        bool JumpReleased {get;}
    }

    public class InputService : IInputService
    {
        public Vector2 RightStick => Input.GetVector(
            "RightStickLeft",
            "RightStickRight",
            "RightStickDown",
            "RightStickUp"
        );

        public bool JumpHeld => Input.IsActionPressed("Jump");
        public bool JumpPressed => Input.IsActionJustPressed("Jump");
        public bool JumpReleased => Input.IsActionJustReleased("Jump");
    }
}
