using System.Collections.Generic;
using Godot;

namespace DiveRollPlatformer
{
    public interface IDebugDisplay
    {
        void ShowValue(string name, object value);
    }

    public class DebugDisplay : Node, IDebugDisplay
    {
        [Export] public NodePath ShownValuesLabel;
        private Label _shownValuesLabel;

        private Dictionary<string, object> _shownValues = new Dictionary<string, object>();

        public override void _EnterTree()
        {
            _shownValuesLabel = GetNode<Label>(ShownValuesLabel);

            // Update the shown values _after_ all other nodes have been
            // processed.  This ensures that the displayed values are the
            // absolute latest.
            ProcessPriority = int.MaxValue;
        }

        public override void _Process(float delta)
        {
            UpdateShownValues();
        }

        public void ShowValue(string name, object value)
        {
            _shownValues[name] = value;
        }

        private void UpdateShownValues()
        {
            var builder = new System.Text.StringBuilder();

            foreach (var kvp in _shownValues)
                builder.AppendLine($"{kvp.Key}: {kvp.Value}");

            _shownValuesLabel.Text = builder.ToString();
        }
    }
}
