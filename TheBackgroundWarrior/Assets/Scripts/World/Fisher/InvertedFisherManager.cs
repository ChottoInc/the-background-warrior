using UnityEngine;

public class InvertedFisherManager : MonoBehaviour
{
    [SerializeField] Transform tilemap;
    [SerializeField] Transform fisher;

    private Vector3 startTilemapScale;
    private Vector2 startFisherPos;
    private Vector2 startFisherScale;


    private void OnDestroy()
    {
        SettingsManager.Instance.OnInvertedFishingSpotChange -= SetInverted;
    }

    private void Awake()
    {
        startTilemapScale = tilemap.transform.localScale;

        startFisherPos = fisher.transform.position;
        startFisherScale = fisher.transform.localScale;

        SettingsManager.Instance.OnInvertedFishingSpotChange += SetInverted;

        SetInverted(SettingsManager.Instance.IsInvertedFishingSpot);
    }

    private void SetInverted(bool isOn)
    {
        if (isOn)
        {
            tilemap.localScale = new Vector3(-startTilemapScale.x, startTilemapScale.y, startTilemapScale.z);

            fisher.transform.position = new Vector2(-startFisherPos.x, startFisherPos.y);
            fisher.transform.localScale = new Vector2(-startFisherScale.x, startFisherScale.y);
        }
        else
        {
            tilemap.localScale = startTilemapScale;

            fisher.transform.position = startFisherPos;
            fisher.transform.localScale = startFisherScale;
        }
    }
}
