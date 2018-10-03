using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IconFonts;
using ImGuiNET;

namespace JsonAnything.GUI.GUIComponents
{
    public class ApplicationArea : IImGuiComponent
    {
        private const int TITLEBAR_HEIGHT = 19;

        public readonly JsonTree JsonTree = new JsonTree();

        private readonly IO _io;

        public ApplicationArea() { _io = ImGui.GetIO(); }
        
        public void Render()
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
