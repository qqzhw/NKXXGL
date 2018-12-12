using System;
using System.Runtime.InteropServices;

namespace ICIMS.Launcher.Utils
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsIconic(IntPtr hWnd);
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>      
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet =
         CharSet.Auto, SetLastError = true)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
    }
}
