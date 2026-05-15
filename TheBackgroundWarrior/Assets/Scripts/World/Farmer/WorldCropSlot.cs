using UnityEngine;

public class WorldCropSlot : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] cropRenderers;
    [SerializeField] WorldSingleCrop[] singleCrops;

    public Transform[] CropTransforms { get; private set; }

    public bool CanGrow { get; private set; }

    private void Start()
    {
        CropTransforms = new Transform[cropRenderers.Length];
        for (int i = 0; i < cropRenderers.Length; i++)
        {
            CropTransforms[i] = cropRenderers[i].transform;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        foreach(var renderer in cropRenderers)
        {
            renderer.sprite = sprite;
        }
    }

    public void SetCanGrow(bool canGrow)
    {
        CanGrow = canGrow;
    }

    public void PlayEmptyVFX()
    {
        foreach (var crop in singleCrops)
        {
            crop.PlayVFX();
        }
    }
}
