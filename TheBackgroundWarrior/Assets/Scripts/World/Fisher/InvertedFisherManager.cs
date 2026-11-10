using UnityEngine;

public class InvertedFisherManager : MonoBehaviour
{
    [Header("Tilemap")]
    [SerializeField] Transform tilemap;

    [Header("Player")]
    [SerializeField] Transform _spriteFisher;
    [SerializeField] Transform _playerFisher;

    [Space(10)]
    [SerializeField] Transform _startFisherPos;
    [SerializeField] Transform _invertedFisherPos;
    

    private Vector3 startTilemapScale;
    private Vector2 startFisherScale;



    private void OnDestroy()
    {
        SettingsManager.Instance.OnInvertedFishingSpotChange -= SetInverted;
    }

    private void Awake()
    {
        startTilemapScale = tilemap.transform.localScale;

        startFisherScale = _spriteFisher.transform.localScale;

        SettingsManager.Instance.OnInvertedFishingSpotChange += SetInverted;

        SetInverted(SettingsManager.Instance.IsInvertedFishingSpot);
    }

    private void SetInverted(bool isOn)
    {
        if (isOn)
        {
            tilemap.localScale = new Vector3(-startTilemapScale.x, startTilemapScale.y, startTilemapScale.z);

            _playerFisher.position = _invertedFisherPos.position;

            _spriteFisher.transform.localScale = new Vector2(-startFisherScale.x, startFisherScale.y);
        }
        else
        {
            tilemap.localScale = startTilemapScale;

            _playerFisher.position = _startFisherPos.position;

            _spriteFisher.transform.localScale = startFisherScale;
        }
    }
}
