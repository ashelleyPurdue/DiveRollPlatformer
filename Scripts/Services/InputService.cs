using Godot;

namespace DiveRollPlatformer
{
    public interface IInputService
    {
        Vector2 LeftStick {get;}
        Vector2 RightStick {get;}

        bool JumpHeld {get;}
        bool JumpPressed {get;}
        bool JumpReleased {get;}

        bool DivePressed {get;}
    }

    public class InputService : IInputService
    {
        public Vector2 LeftStick => Input.GetVector(
            "LeftStickLeft",
            "LeftStickRight",
            "LeftStickDown",
            "LeftStickUp"
        );

        public Vector2 RightStick => Input.GetVector(
            "RightStickLeft",
            "RightStickRight",
            "RightStickDown",
            "RightStickUp"
        );

        public bool JumpHeld => Input.IsActionPressed("Jump");
        public bool JumpPressed => Input.IsActionJustPressed("Jump");
        public bool JumpReleased => Input.IsActionJustReleased("Jump");

        public bool DivePressed => Input.IsActionJustPressed("Dive");
    }
}
