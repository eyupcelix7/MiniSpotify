using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniSpotify
{
    public static class Win32
    {
        public const int DWMWINDOWCORNERPREFERENCE = 33;
        public const int DWMCROUND = 2;
        public const int DWMCDEFAULT = 1;


        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttr, int cbAttr);

        public static void SetRoundedCorner(IntPtr hwnd)
        {
            int cornerPreference = DWMCROUND;
            DwmSetWindowAttribute(hwnd,DWMWINDOWCORNERPREFERENCE,ref cornerPreference, sizeof(int));
        }
    }
}
