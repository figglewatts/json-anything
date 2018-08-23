using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace JsonAnything.GUI.GUIComponents
{
    public class MainMenuBar : IImGuiComponent
    {
        public void Render()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    renderFileMenu();
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Edit"))
                {
                    renderEditMenu();
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Help"))
                {
                    renderHelpMenu();
                    ImGui.EndMenu();
                }
            }
            ImGui.EndMainMenuBar();
        }

        private void renderFileMenu()
        {
            if (ImGui.MenuItem("New", "Ctrl+N")) {}

            if (ImGui.MenuItem("Open", "Ctrl+O")) {}

            if (ImGui.BeginMenu("Open Recent"))
            {
                ImGui.MenuItem("file1...");
                ImGui.MenuItem("file2...");
                ImGui.EndMenu();
            }

            ImGui.Separator();

            if (ImGui.MenuItem("Save", "Ctrl+S")) {}

            if (ImGui.MenuItem("Save As...", "Ctrl+Shift+S")) {}
        }

        private void renderEditMenu()
        {
            if (ImGui.MenuItem("Undo", "Ctrl+Z")) {}

            if (ImGui.MenuItem("Redo", "Ctrl+Y")) {}

            ImGui.Separator();

            if (ImGui.MenuItem("Cut", "Ctrl+X")) {}

            if (ImGui.MenuItem("Copy", "Ctrl+C")) {}

            if (ImGui.MenuItem("Paste", "Ctrl+V")) {}

            if (ImGui.MenuItem("Select All", "Ctrl+A")) {}
        }

        private void renderHelpMenu()
        {
            if (ImGui.MenuItem("About JsonAnything")) {}
        }
    }
}
