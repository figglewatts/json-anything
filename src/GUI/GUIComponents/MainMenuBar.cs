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
        private readonly FileDialog _openDialog;
        private readonly FileDialog _saveDialog;
        private JsonTree _jsonTree;

        private bool _openFileOpenDialog = false;
        private bool _openFileSaveDialog = false;

        public MainMenuBar(JsonTree jsonTree)
        {
            _openDialog = new FileDialog("", FileDialog.DialogType.Open);
            _saveDialog = new FileDialog("", FileDialog.DialogType.Save);
            _jsonTree = jsonTree;
        }
        
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

            if (_openFileOpenDialog)
            {
                _openDialog.Show(fileName => _jsonTree.LoadJson(fileName), "*.json");
                _openFileOpenDialog = false;
            }
            _openDialog.Render();

            if (_openFileSaveDialog)
            {
                _saveDialog.Show(fileName => _jsonTree.SaveJson(fileName), "*", ".json");
                _openFileSaveDialog = false;
            }
            _saveDialog.Render();
        }

        private void renderFileMenu()
        {
            if (ImGui.MenuItem("New", "Ctrl+N")) {}

            if (ImGui.MenuItem("Open", "Ctrl+O"))
            {
                _openFileOpenDialog = true;
            }

            if (ImGui.BeginMenu("Open Recent"))
            {
                ImGui.MenuItem("file1...");
                ImGui.MenuItem("file2...");
                ImGui.EndMenu();
            }

            ImGui.Separator();

            if (ImGui.MenuItem("Save", "Ctrl+S")) {}

            if (ImGui.MenuItem("Save As...", "Ctrl+Shift+S"))
            {
                _openFileSaveDialog = true;
            }
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
