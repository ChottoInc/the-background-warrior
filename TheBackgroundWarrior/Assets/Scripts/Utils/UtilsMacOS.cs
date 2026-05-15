using System;
using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_STANDALONE_OSX
using AOT;
using ObjCRuntime;
using Foundation;
#endif

public static class UtilsMacOS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NSRect
    {
        public double x, y, width, height;
    }

    [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
    private static extern IntPtr NSScreen_mainScreen();

    // Objective-C method signature for struct return
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend_stret")]
    private static extern void objc_msgSend_stret(out NSRect ret, IntPtr receiver, IntPtr selector);

    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "sel_registerName")]
    private static extern IntPtr sel_registerName(string name);

    /// <summary>
    /// Returns the usable screen area (excluding dock and menu bar) as a Vector2 (width, height)
    /// </summary>
    public static Vector2 GetVisibleFrameSize()
    {
        IntPtr mainScreen = NSScreen_mainScreen();
        if (mainScreen == IntPtr.Zero)
        {
            Debug.LogWarning("Could not get main screen. Falling back to Screen.currentResolution.");
            return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        }

        NSRect frame;
        IntPtr selector = sel_registerName("visibleFrame");
        objc_msgSend_stret(out frame, mainScreen, selector);

        return new Vector2((float)frame.width, (float)frame.height);
    }
}
