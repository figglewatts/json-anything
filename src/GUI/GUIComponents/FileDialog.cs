using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IconFonts;
using ImGuiNET;
using JsonAnything.Util;

namespace JsonAnything.GUI.GUIComponents
{
    public class FileDialog : IImGuiComponent
    {
        public enum DialogType
        {
            Open,
            Save
        }

        public DialogType Type { get; }
        public string FilePath { get; private set; }

        private bool _open = true;
        private string _currentDir;
        private int _selectedFile = -1;
        private string _bottomBarText = "";
        private readonly Vector2 _dialogStartSize = new Vector2(400, 300);
        private List<string> _directoriesInCurrentDir;
        private List<string> _filesInCurrentDir;

        public FileDialog(string dir, DialogType type)
        {
            FilePath = dir;
            Type = type;
            _currentDir = Directory.GetCurrentDirectory();
            _directoriesInCurrentDir = new List<string>();
            _filesInCurrentDir = new List<string>();
            updateFilesInCurrentDir();
        }

        public void Show()
        {
            ImGui.OpenPopup("Open file...");
            _open = true;
        }
        
        public void Render()
        {
            if (Type == DialogType.Open)
            {
                renderFileOpenDialog();
            }
            else
            {
                renderFileSaveDialog();
            }
        }

        private void renderFileOpenDialog()
        {
            // TODO: detect if closed
            
            ImGui.SetNextWindowSize(_dialogStartSize, Condition.FirstUseEver);
            if (ImGui.BeginPopupModal("Open file...", ref _open))
            {
                renderTopBar();
                renderFileList();
                renderBottomBar();
                
                ImGui.EndPopup();
            }
        }

        private void renderFileSaveDialog()
        {

        }

        private void refreshFileList()
        {
            _selectedFile = -1;
            updateDirectoriesInCurrentDir();
            updateFilesInCurrentDir();
        }

        private void goToParentDir()
        {
            DirectoryInfo parentDir = Directory.GetParent(_currentDir);
            _currentDir = parentDir?.ToString() ?? _currentDir;
            refreshFileList();
        }

        private void updateDirectoriesInCurrentDir()
        {
            _directoriesInCurrentDir.Clear();

            if (!Directory.Exists(_currentDir)) return;
            string[] dirs = Directory.GetDirectories(_currentDir, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in dirs)
            {
                _directoriesInCurrentDir.Add(Path.GetFileName(dir.TrimEnd(Path.DirectorySeparatorChar)));
            }
        }

        private void updateFilesInCurrentDir()
        {
            _filesInCurrentDir.Clear();

            if (!Directory.Exists(_currentDir)) return;
            string[] files = Directory.GetFiles(_currentDir);

            foreach (string file in files)
            {
                _filesInCurrentDir.Add(Path.GetFileName(file));
            }
        }

        private void renderTopBar()
        {
            Vector2 pos = ImGui.GetCursorScreenPos();
            ImGui.SetCursorScreenPos(new Vector2(pos.X, pos.Y + 5));
            ImGui.Text(FontAwesome5.FolderOpen);
            ImGui.SameLine();
            Vector2 posAfter = ImGui.GetCursorScreenPos();
            ImGui.SetCursorScreenPos(new Vector2(posAfter.X, pos.Y));
            ImGui.PushItemWidth(-40);
            bool modified = false;
            modified = ImGuiNETExtensions.InputText("##current-dir", ref _currentDir);
            ImGui.SameLine();
            if (ImGui.Button("Up", new Vector2(30, 0)))
            {
                goToParentDir();
            }

            if (modified)
            {
                refreshFileList();
            }
        }

        private void renderFileList()
        {
            // TODO: not selectable if in save mode
            
            ImGui.BeginChild("fileSelect", new Vector2(-1, -24), true, WindowFlags.Default);
            if (!Directory.Exists(_currentDir))
            {
                ImGui.Text("Directory does not exist!");
            }
            else if (_filesInCurrentDir.Count <= 0 && _directoriesInCurrentDir.Count <= 0)
            {
                ImGui.Text("Directory is empty!");
            }
            else
            {
                int i = 0;
                foreach (string dir in _directoriesInCurrentDir)
                {
                    ImGui.PushID(i);
                    if (ImGui.Selectable($"{FontAwesome5.Folder} {dir}", _selectedFile == i))
                    {
                        _selectedFile = i;
                    }
                    ImGui.PopID();
                    i++;
                }

                foreach (string file in _filesInCurrentDir)
                {
                    ImGui.PushID(i);
                    if (ImGui.Selectable($"{FontAwesome5.File} {file}", _selectedFile == i))
                    {
                        _selectedFile = i;
                    }
                    ImGui.PopID();
                    i++;
                }
            }
            ImGui.EndChild();
        }

        private void renderBottomBar()
        {
            ImGui.PushItemWidth(-50);
            ImGuiNETExtensions.InputText("##bottombar", ref _bottomBarText);
            ImGui.SameLine();
            if (Type == DialogType.Open)
            {
                if (ImGui.Button("Open", new Vector2(48, 0)))
                {
                    // TODO: open file
                }
            }
            else
            {
                if (ImGui.Button("Save", new Vector2(48, 0)))
                {
                    // TODO: save file
                }
            }
        }
    }
}
