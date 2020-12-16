using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperManager
{
    public class Win32
    {
        //?-----FindWindow-----
        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static IntPtr FindWindow(string caption)
        {
            return FindWindow(String.Empty, caption);
        }

        //?-----SendMessageTimeout-----
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        //?-----EnumWindows-----
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        //?-----FindWindowEx-----
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        //?-----GetDCEx-----
        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, DeviceContextValues flags);

        /// <summary>Values to pass to the GetDCEx method.</summary>
        [Flags()]
        public enum DeviceContextValues : uint
        {
            /// <summary>DCX_WINDOW: Returns a DC that corresponds to the window rectangle rather
            /// than the client rectangle.</summary>
            Window = 0x00000001,
            /// <summary>DCX_CACHE: Returns a DC from the cache, rather than the OWNDC or CLASSDC
            /// window. Essentially overrides CS_OWNDC and CS_CLASSDC.</summary>
            Cache = 0x00000002,
            /// <summary>DCX_NORESETATTRS: Does not reset the attributes of this DC to the
            /// default attributes when this DC is released.</summary>
            NoResetAttrs = 0x00000004,
            /// <summary>DCX_CLIPCHILDREN: Excludes the visible regions of all child windows
            /// below the window identified by hWnd.</summary>
            ClipChildren = 0x00000008,
            /// <summary>DCX_CLIPSIBLINGS: Excludes the visible regions of all sibling windows
            /// above the window identified by hWnd.</summary>
            ClipSiblings = 0x00000010,
            /// <summary>DCX_PARENTCLIP: Uses the visible region of the parent window. The
            /// parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is
            /// set to the upper-left corner of the window identified by hWnd.</summary>
            ParentClip = 0x00000020,
            /// <summary>DCX_EXCLUDERGN: The clipping region identified by hrgnClip is excluded
            /// from the visible region of the returned DC.</summary>
            ExcludeRgn = 0x00000040,
            /// <summary>DCX_INTERSECTRGN: The clipping region identified by hrgnClip is
            /// intersected with the visible region of the returned DC.</summary>
            IntersectRgn = 0x00000080,
            /// <summary>DCX_EXCLUDEUPDATE: Unknown...Undocumented</summary>
            ExcludeUpdate = 0x00000100,
            /// <summary>DCX_INTERSECTUPDATE: Unknown...Undocumented</summary>
            IntersectUpdate = 0x00000200,
            /// <summary>DCX_LOCKWINDOWUPDATE: Allows drawing even if there is a LockWindowUpdate
            /// call in effect that would otherwise exclude this window. Used for drawing during
            /// tracking.</summary>
            LockWindowUpdate = 0x00000400,
            /// <summary>DCX_VALIDATE When specified with DCX_INTERSECTUPDATE, causes the DC to
            /// be completely validated. Using this function with both DCX_INTERSECTUPDATE and
            /// DCX_VALIDATE is identical to using the BeginPaint function.</summary>
            Validate = 0x00200000,
        }

        //?-----ReleaseDC-----
        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }

}
