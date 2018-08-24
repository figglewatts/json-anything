using System;
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

            unsafe
            {
                fixed (byte* txtPtr = textArray)
                {
                    state = ImGuiNative.igInputText(label, new IntPtr(txtPtr), maxLength, flags, callback,
                        IntPtr.Zero.ToPointer());
                }
            }

            text = System.Text.Encoding.Default.GetString(textArray);
            return state;
        }
    }
}
