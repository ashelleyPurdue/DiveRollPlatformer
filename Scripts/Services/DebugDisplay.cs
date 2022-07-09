using Godot;

namespace DiveRollPlatformer
{
    public interface IDebugDisplay
    {
        void ShowValue(string name, object value);
    }

    public class DebugDisplay : Node, IDebugDisplay
    {
        public void ShowValue(string name, object value)
        {
            // TODO: Add this message to a panel
            GD.Print($"{name}: {value}");
        }
    }
}
