using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace JsonAnything.GUI.GUIComponents
{
    public class ApplicationArea : IImGuiComponent
    {
        private const int TITLEBAR_HEIGHT = 19;
        
        public void Render()
        {
            ImGui.SetNextWindowPos(new Vector2(0, TITLEBAR_HEIGHT), Condition.Always, Vector2.Zero);
            ImGui.SetNextWindowSize(ImGui.GetIO().DisplaySize, Condition.Always);
            ImGui.PushStyleVar(StyleVar.WindowRounding, 0);
            if (ImGui.BeginWindow("",
                WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoCollapse | WindowFlags.NoSavedSettings |
                WindowFlags.NoScrollbar | WindowFlags.NoTitleBar | WindowFlags.NoBringToFrontOnFocus))
            {
                for (int i = 0; i < 10; i++)
                {
                    ImGui.Button($"Button {i}");
                }
                
                ImGui.EndWindow();
            }
            ImGui.PopStyleVar();
        }
    }
}
