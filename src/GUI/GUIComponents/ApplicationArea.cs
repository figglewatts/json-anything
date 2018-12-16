using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IconFonts;
using ImGuiNET;
using OpenTK;
using Vector2 = System.Numerics.Vector2;

namespace JsonAnything.GUI.GUIComponents
{
    public class ApplicationArea : ImGuiComponent
    {
        private const int TITLEBAR_HEIGHT = 19;

        public readonly JsonTree JsonTree;

        private readonly IO _io;

        public ApplicationArea(MainWindow window) : base(window)
        {
            _io = ImGui.GetIO();
            JsonTree = new JsonTree(window);
        }

        public override void Render()
        {
            ImGui.SetNextWindowPos(new Vector2(0, TITLEBAR_HEIGHT), Condition.Always, Vector2.Zero);
            ImGui.SetNextWindowSize(new Vector2(_io.DisplaySize.X, _io.DisplaySize.Y - TITLEBAR_HEIGHT), Condition.Always);
            ImGui.PushStyleVar(StyleVar.WindowRounding, 0);
            if (ImGui.BeginWindow("",
                WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoCollapse | WindowFlags.NoSavedSettings |
                WindowFlags.NoTitleBar | WindowFlags.NoBringToFrontOnFocus))
            {                
                JsonTree.Render();
                ImGui.EndWindow();
            }
            ImGui.PopStyleVar();
        }
    }
}
