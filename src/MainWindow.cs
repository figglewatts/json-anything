using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IconFonts;
using ImGuiNET;
using JsonAnything.GUI;
using JsonAnything.GUI.GUIComponents;
using JsonAnything.Json;
using JsonAnything.Util;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace JsonAnything
{
    public sealed class MainWindow : GameWindow
    {
        private const int WINDOW_WIDTH = 1280;
        private const int WINDOW_HEIGHT = 720;
        private const string WINDOW_TITLE = "JsonAnything";
        private const int GL_MAJOR_VERSION = 4;
        private const int GL_MINOR_VERSION = 0;

        private List<IImGuiComponent> _guiComponents;
        
        public MainWindow()
            : base(WINDOW_WIDTH, WINDOW_HEIGHT,
                GraphicsMode.Default,
                WINDOW_TITLE,
                GameWindowFlags.Default,
                DisplayDevice.Default,
                GL_MAJOR_VERSION, GL_MINOR_VERSION,
                GraphicsContextFlags.Default)
        {
            ImGuiRenderer.Init();
            
            _guiComponents = new List<IImGuiComponent>();

            ApplicationArea applicationArea = new ApplicationArea();

            _guiComponents.Add(new MainMenuBar(applicationArea.JsonTree));
            _guiComponents.Add(applicationArea);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            ImGuiRenderer.Resize(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = true;
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1);

            Logger.Log()(LogLevel.INFO, "Initialized JsonAnything");

            IO io = ImGui.GetIO();

            ImGui.GetIO().FontAtlas.AddDefaultFont();

            FontConfig cfg = new FontConfig
            {
                MergeMode = 1,
                PixelSnapH = 1
            };
            ImGuiRenderer.AddFontFromFileTTF("fonts/fa-solid-900.ttf", 16, cfg,
                new[] {(char)FontAwesome5.IconMin, (char)FontAwesome5.IconMax, (char)0});
        }

        protected override void OnUnload(EventArgs e)
        {
            ImGuiRenderer.Shutdown();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleInput();
        }

        private void HandleInput()
        {

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            ImGuiRenderer.BeginFrame(e.Time);

            foreach (var component in _guiComponents)
            {
                component.Render();
            }

            ImGuiRenderer.EndFrame();

            SwapBuffers();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            ImGuiRenderer.AddKeyChar(e.KeyChar);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            ImGuiRenderer.UpdateMousePos(e.X, e.Y);
        }
    }
}
