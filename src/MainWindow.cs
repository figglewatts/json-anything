using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using JsonAnything.GUI;
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

        private bool opened = true;
        
        public MainWindow()
            : base(WINDOW_WIDTH, WINDOW_HEIGHT,
                GraphicsMode.Default,
                WINDOW_TITLE,
                GameWindowFlags.Default,
                DisplayDevice.Default,
                GL_MAJOR_VERSION, GL_MINOR_VERSION,
                GraphicsContextFlags.Default)
        {
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

            ImGuiRenderer.Init();
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

            ImGuiNative.igShowDemoWindow(ref opened);

            ImGuiRenderer.EndFrame();
            
            // draw stuff

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
