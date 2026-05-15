using UnityEngine;

public class WorldSingleCrop : MonoBehaviour
{
    [SerializeField] ParticleSystem leavesVFX;

    public void PlayVFX()
    {
        if (!leavesVFX.isPlaying)
        {
            leavesVFX.Play();
        }
    }
}
