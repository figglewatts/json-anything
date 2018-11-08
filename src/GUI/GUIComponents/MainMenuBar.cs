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
        private readonly FileDialog _openSchemaDialog;
        private readonly FileDialog _saveDialog;
        private JsonTree _jsonTree;

        private bool _openFileOpenDialog = false;
        private bool _openFileSaveDialog = false;
        private bool _openSchemaOpenDialog = false;

        public MainMenuBar(JsonTree jsonTree)
        {
            _openDialog = new FileDialog("", FileDialog.DialogType.Open);
            _openSchemaDialog = new FileDialog("", FileDialog.DialogType.Open);
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

            if (_openFileSaveDialog)
            {
                _saveDialog.Show(fileName => _jsonTree.SaveJson(fileName), "*", ".json");
                _openFileSaveDialog = false;
            }

            if (_openSchemaOpenDialog)
            {
                _openSchemaDialog.Show(fileName => _jsonTree.LoadSchema(fileName), "*.json");
                _openSchemaOpenDialog = false;
            }

            _openDialog.Render();
            _openSchemaDialog.Render();
            _saveDialog.Render();
        }

        private void renderFileMenu()
        {
            if (ImGui.MenuItem("New")) {}

            if (ImGui.MenuItem("Open"))
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

            if (ImGui.MenuItem("Load schema"))
            {
                _openSchemaOpenDialog = true;
            }

            ImGui.Separator();

            if (ImGui.MenuItem("Save")) {}

            if (ImGui.MenuItem("Save As..."))
            {
                _openFileSaveDialog = true;
            }
        }

        private void renderEditMenu()
        {
            if (ImGui.MenuItem("Undo")) {}

            if (ImGui.MenuItem("Redo")) {}

            ImGui.Separator();

            if (ImGui.MenuItem("Cut")) {}

            if (ImGui.MenuItem("Copy")) {}

            if (ImGui.MenuItem("Paste")) {}

            if (ImGui.MenuItem("Select All")) {}
        }

        private void renderHelpMenu()
        {
            if (ImGui.MenuItem("About JsonAnything")) {}
        }
    }
}
