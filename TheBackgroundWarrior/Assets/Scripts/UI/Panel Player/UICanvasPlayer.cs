using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Canvas))]
public class UICanvasPlayer : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        // Assign UI camera to render the player canvas
        if(Camera.main.TryGetComponent(out UniversalAdditionalCameraData camData))
        {
            if (camData.cameraStack != null && camData.cameraStack.Count > 0)
            {
                Camera firstOverlayCamera = camData.cameraStack[0];
                canvas.worldCamera = firstOverlayCamera;
            }
        }
    }
}
