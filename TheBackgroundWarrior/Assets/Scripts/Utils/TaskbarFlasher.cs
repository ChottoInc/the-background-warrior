using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TaskbarFlasher : MonoBehaviour
{
    #region Win32 interop

    [StructLayout(LayoutKind.Sequential)]
    private struct FLASHWINFO
    {
        public uint cbSize;
        public IntPtr hwnd;
        public uint dwFlags;
        public uint uCount;
        public uint dwTimeout;
    }

    [DllImport("user32.dll")]
    private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern bool GetForegroundWindow_(); // unused placeholder, kept for clarity

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    // Flags for dwFlags
    private const uint FLASHW_STOP = 0;      // Stop flashing
    private const uint FLASHW_CAPTION = 1;   // Flash the title bar
    private const uint FLASHW_TRAY = 2;      // Flash the taskbar button
    private const uint FLASHW_ALL = 3;       // Flash both
    private const uint FLASHW_TIMER = 4;     // Flash continuously until stopped
    private const uint FLASHW_TIMERNOFG = 12; // Flash until the window comes to foreground

    #endregion

    private IntPtr _windowHandle = IntPtr.Zero;

    private void Awake()
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        // Cache the handle to this process's main window.
        _windowHandle = GetActiveWindow();
 
        // Fallback: if GetActiveWindow returns nothing useful at startup
        // (can happen depending on when Awake runs), try FindWindow with
        // the product name as the window title as a backup.
        if (_windowHandle == IntPtr.Zero)
        {
            _windowHandle = FindWindow(null, Application.productName);
        }
#endif
    }

    /// <summary>
    /// Flashes the taskbar icon a fixed number of times, then stops on its own
    /// </summary>
    public void FlashTaskbarIcon(uint flashCount = 5)
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        if (_windowHandle == IntPtr.Zero) return;
        if (Application.isFocused) return;
 
        FLASHWINFO fwi = new FLASHWINFO
        {
            hwnd = _windowHandle,
            dwFlags = FLASHW_TRAY, // just the taskbar icon, not the title bar
            uCount = flashCount,
            dwTimeout = 0 // 0 = use the default cursor blink rate
        };
        fwi.cbSize = (uint)Marshal.SizeOf(fwi);
 
        FlashWindowEx(ref fwi);
#endif
    }

    /// <summary>
    /// Flashes indefinitely until the user clicks back into the game window
    /// </summary>
    public void FlashTaskbarIconUntilFocused()
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        //Debug.Log("Flashing...");
        if (_windowHandle == IntPtr.Zero) 
        {
            Debug.Log("Window handle is null");
            return;
        }
        if (Application.isFocused) return;
        FLASHWINFO fwi = new FLASHWINFO
        {
            hwnd = _windowHandle,
            dwFlags = FLASHW_TIMERNOFG,
            uCount = uint.MaxValue,
            dwTimeout = 0
        };
        fwi.cbSize = (uint)Marshal.SizeOf(fwi);
 
        FlashWindowEx(ref fwi);
#endif
    }

    /// <summary>
    /// Manually stop any ongoing flash (usually not needed, since
    /// FLASHW_TIMERNOFG auto-stops once the window regains focus).
    /// </summary>
    public void StopFlashing()
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        if (_windowHandle == IntPtr.Zero) return;
 
        FLASHWINFO fwi = new FLASHWINFO
        {
            hwnd = _windowHandle,
            dwFlags = FLASHW_STOP,
            uCount = 0,
            dwTimeout = 0
        };
        fwi.cbSize = (uint)Marshal.SizeOf(fwi);
 
        FlashWindowEx(ref fwi);
#endif
    }
}
