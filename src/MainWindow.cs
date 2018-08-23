using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace JsonAnything
{
    public sealed class MainWindow : GameWindow
    {
        private const int WINDOW_WIDTH = 1280;
        private const int WINDOW_HEIGHT = 720;
        private const string WINDOW_TITLE = "JsonAnything";
        private const int GL_MAJOR_VERSION = 4;
        private const int GL_MINOR_VERSION = 0;
        
        public MainWindow()
            : base(WINDOW_WIDTH, WINDOW_HEIGHT,
                GraphicsMode.Default,
                WINDOW_TITLE,
                GameWindowFlags.Default,
                DisplayDevice.Default,
                GL_MAJOR_VERSION, GL_MINOR_VERSION,
                GraphicsContextFlags.ForwardCompatible)
        {
        }


    }
}
