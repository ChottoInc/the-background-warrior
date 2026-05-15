using UnityEngine;

public static class UtilsTime
{
    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
    }
}
