using System;
using UnityEngine;
using System.Runtime.InteropServices;

public static class UtilsWindowsSO
{
    [StructLayout(LayoutKind.Sequential)]
    struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int left, top, right, bottom;
    }

    const int ABM_GETTASKBARPOS = 5;

    [DllImport("shell32.dll")]
    static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

    public static Rect GetTaskbarRect()
    {
        APPBARDATA data = new APPBARDATA();
        data.cbSize = Marshal.SizeOf(data);

        SHAppBarMessage(ABM_GETTASKBARPOS, ref data);
        return new Rect(data.rc.left, data.rc.top,
                        data.rc.right - data.rc.left,
                        data.rc.bottom - data.rc.top);
    }
    /*
    public static Vector2 GetUsableDesktopSize(int monitorIndex)
    {
#if UNITY_STANDALONE_WIN
        Rect taskbar = GetTaskbarRect();
        Debug.Log("Taskbar x,y: " + taskbar.x + ", " + taskbar.y);
        return new Vector2(Display.displays[monitorIndex].systemWidth, taskbar.y);

#elif UNITY_STANDALONE_OSX
        return UtilsMacOS.GetVisibleFrameSize();

#else
        Rect taskbar = GetTaskbarRect();
        return new Vector2(monitor.systemWidth, taskbar.y);
#endif
    }*/
}
