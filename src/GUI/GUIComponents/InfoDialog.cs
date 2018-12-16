using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK;

namespace JsonAnything.GUI.GUIComponents
{
    public class InfoDialog : ImGuiComponent
    {
        public enum DialogType
        {
            Info,
            Warning,
            Error
        }

        public DialogType Type { get; }
        public string Message { get; }

        // TODO: configurable buttons...

        public InfoDialog(DialogType type, string message, MainWindow window)
            : base(window)
        {
            Type = type;
            Message = message;
        }

        public override void Render()
        {
            ImGui.Text(Message);
        }
    }
}
