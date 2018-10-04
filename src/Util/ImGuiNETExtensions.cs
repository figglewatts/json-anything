using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace JsonAnything.Util
{
    using ImGuiNET;

    public static class ImGuiNETExtensions
    {
        public static bool InputText(string label, ref string text, uint maxLength = 1024, InputTextFlags flags = InputTextFlags.Default, TextEditCallback callback = null)
        {
            bool state;
            byte[] textArray = new byte[maxLength];
            Encoding.Default.GetBytes(text, 0, text.Length, textArray, 0);
            byte[] labelArray = Encoding.UTF8.GetBytes(label);
            unsafe
            {
                fixed (byte* txtPtr = textArray)
                {
                    state = ImGuiNative.igInputText(labelArray, new IntPtr(txtPtr), maxLength, flags, callback,
                        IntPtr.Zero.ToPointer());
                }
            }

            text = System.Text.Encoding.Default.GetString(textArray).TrimEnd('\0');
            return state;
        }

        public static unsafe Font AddFontFromFileTTF(this FontAtlas atlas, string fileName, float pixelSize, FontConfig config, char[] glyphRanges)
        {
            NativeFontAtlas* atlasPtr = ImGui.GetIO().GetNativePointer()->FontAtlas;
            IntPtr cfgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(config));
            Marshal.StructureToPtr(config, cfgPtr, false);
            fixed (char* glyphPtr = &glyphRanges[0])
            {
                NativeFont* fontPtr =
                    ImGuiNative.ImFontAtlas_AddFontFromFileTTF(atlasPtr, fileName, pixelSize, cfgPtr, glyphPtr);
                return new Font(fontPtr);
            }
        }

        public static float CalcTextWidth(string text)
        {
            byte[] textArray = new byte[text.Length];
            Encoding.Default.GetBytes(text, 0, text.Length, textArray, 0);
            unsafe
            {
                fixed (byte* textPtr = textArray)
                {
                    ImGuiNative.igCalcTextSize(out Vector2 size, (char*)textPtr, (char*)textPtr + text.Length, false, 50);
                    return size.X;
                }
            }
            
        }
    }
}
