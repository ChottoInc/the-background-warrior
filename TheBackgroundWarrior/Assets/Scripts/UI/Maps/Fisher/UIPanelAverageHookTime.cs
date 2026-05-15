using TMPro;
using UnityEngine;

public class UIPanelAverageHookTime : MonoBehaviour
{
    [SerializeField] TMP_Text textAverage;

    [SerializeField] PlayerFisher player;

    private bool isInitialized;

    private void OnDestroy()
    {
        player.OnStatChange -= CheckStatChange;
    }

    private void Awake()
    {
        player.OnStatChange += CheckStatChange;
    }

    private void Update()
    {
        if (isInitialized) return;

        if(player.PlayerData != null)
        {
            UpdateUI();

            isInitialized = true;
        }

    }

    private void UpdateUI()
    {
        float average = FishSpawnManager.Instance.AverageHookTime;
        textAverage.text = string.Format("Average time: ~{0}m{1}s", 
            Mathf.FloorToInt(average / 60f),
            Mathf.FloorToInt(average % 60f));
    }

    private void CheckStatChange(int id, int amount)
    {
        if(id == UtilsPlayer.ID_FISHER_CALMNESS)
        {
            UpdateUI();
        }
    }
}
