using System.Linq;
using UnityEngine;

public class CropsPlantManager : MonoBehaviour
{
    [SerializeField] WorldCropSlot slot1;
    [SerializeField] WorldCropSlot slot2;
    [SerializeField] WorldCropSlot slot3;
    [SerializeField] WorldCropSlot slot4;

    public Transform[] Crop1Transforms => slot1.CropTransforms;
    public Transform[] Crop2Transforms => slot2.CropTransforms;
    public Transform[] Crop3Transforms => slot3.CropTransforms;
    public Transform[] Crop4Transforms => slot4.CropTransforms;

    [Space(10)]
    [SerializeField] PlayerFarmer player;


    private CropData cropSlot1;
    private CropData cropSlot2;
    private CropData cropSlot3;
    private CropData cropSlot4;

    private float timerCropGrowth = 1f;
    private float timerAutoSave = 60f;

    [Header("Lures")]
    [SerializeField] float ySpawn = -4f;
    [SerializeField] float minLureCooldown = 180f;
    [SerializeField] float maxLureCooldown = 600f;

    private bool isLuringSlot1;
    private bool isLuringSlot2;
    private bool isLuringSlot3;
    private bool isLuringSlot4;

    private float timerLureSlot1;
    private float timerLureSlot2;
    private float timerLureSlot3;
    private float timerLureSlot4;

    [Header("Cheats")]
    [SerializeField] bool reducedLureCooldownCheat;
    [SerializeField] bool reducedGrowthTimeCheat;


    public static CropsPlantManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (PlayerManager.Instance.PlayerFarmerData.Slot1CropData != null)
            SetCrop(0, PlayerManager.Instance.PlayerFarmerData.Slot1CropData, false);

        if (PlayerManager.Instance.PlayerFarmerData.Slot2CropData != null)
            SetCrop(1, PlayerManager.Instance.PlayerFarmerData.Slot2CropData, false);

        if (PlayerManager.Instance.PlayerFarmerData.Slot3CropData != null)
            SetCrop(2, PlayerManager.Instance.PlayerFarmerData.Slot3CropData, false);

        if (PlayerManager.Instance.PlayerFarmerData.Slot4CropData != null)
            SetCrop(3, PlayerManager.Instance.PlayerFarmerData.Slot4CropData, false);
    }

    private void Update()
    {
        HandleUpdateGrowth();

        HandleUpdateSave();

        if (cropSlot1 != null)
            HandleLureSlot1();

        if (cropSlot2 != null)
            HandleLureSlot2();

        if (cropSlot3 != null)
            HandleLureSlot3();

        if (cropSlot4 != null)
            HandleLureSlot4();
    }

    private void HandleLureSlot1()
    {
        if (!isLuringSlot1)
        {
            if (cropSlot1.IsFullyGrown)
            {
                isLuringSlot1 = true;

                // get random timer to lure a companion
                float finalMaxLureCooldown = maxLureCooldown - (maxLureCooldown * PlayerManager.Instance.PlayerFarmerData.CurrentKindness);

                if (reducedLureCooldownCheat && SettingsManager.Instance.AreCheatsEnabled)
                {
                    timerLureSlot1 = 30f;
                    //Debug.Log("started lure slot 1");
                }
                else
                {
                    timerLureSlot1 = UtilsGeneral.GetRandomValueBtwValues(minLureCooldown, finalMaxLureCooldown);
                }
            }
        }
        else
        {
            if (timerLureSlot1 <= 0)
            {
                // get random companion from possible of crops and spawn
                int randCompanionIndex = Random.Range(0, cropSlot1.CropSO.AttractedCompanions.Length);
                SpawnCompanion(0, cropSlot1.CropSO.AttractedCompanions[randCompanionIndex], slot1);

                // reset lure so the timer resets
                isLuringSlot1 = false;
            }
            else
            {
                timerLureSlot1 -= Time.deltaTime;
            }
        }
    }

    private void HandleLureSlot2()
    {
        if (!isLuringSlot2)
        {
            if (cropSlot2.IsFullyGrown)
            {
                isLuringSlot2 = true;

                // get random timer to lure a companion
                float finalMaxLureCooldown = maxLureCooldown - (maxLureCooldown * PlayerManager.Instance.PlayerFarmerData.CurrentKindness);

                if (reducedLureCooldownCheat && SettingsManager.Instance.AreCheatsEnabled)
                {
                    timerLureSlot2 = 30f;
                    //Debug.Log("started lure slot 2");
                }
                else
                {
                    timerLureSlot2 = UtilsGeneral.GetRandomValueBtwValues(minLureCooldown, finalMaxLureCooldown);
                }
            }
        }
        else
        {
            if (timerLureSlot2 <= 0)
            {
                // get random companion from possible of crops and spawn
                int randCompanionIndex = Random.Range(0, cropSlot2.CropSO.AttractedCompanions.Length);
                SpawnCompanion(1, cropSlot2.CropSO.AttractedCompanions[randCompanionIndex], slot2);

                // reset lure so the timer resets
                isLuringSlot2 = false;
            }
            else
            {
                timerLureSlot2 -= Time.deltaTime;
            }
        }
    }

    private void HandleLureSlot3()
    {
        if (!isLuringSlot3)
        {
            if (cropSlot3.IsFullyGrown)
            {
                isLuringSlot3 = true;

                // get random timer to lure a companion
                float finalMaxLureCooldown = maxLureCooldown - (maxLureCooldown * PlayerManager.Instance.PlayerFarmerData.CurrentKindness);

                if (reducedLureCooldownCheat && SettingsManager.Instance.AreCheatsEnabled)
                {
                    timerLureSlot3 = 30f;
                    //Debug.Log("started lure slot 3");
                }
                else
                {
                    timerLureSlot3 = UtilsGeneral.GetRandomValueBtwValues(minLureCooldown, finalMaxLureCooldown);
                }
            }
        }
        else
        {
            if (timerLureSlot3 <= 0)
            {
                // get random companion from possible of crops and spawn
                int randCompanionIndex = Random.Range(0, cropSlot3.CropSO.AttractedCompanions.Length);
                SpawnCompanion(2, cropSlot3.CropSO.AttractedCompanions[randCompanionIndex], slot3);

                // reset lure so the timer resets
                isLuringSlot3 = false;
            }
            else
            {
                timerLureSlot3 -= Time.deltaTime;
            }
        }
    }

    private void HandleLureSlot4()
    {
        if (!isLuringSlot4)
        {
            if (cropSlot4.IsFullyGrown)
            {
                isLuringSlot4 = true;

                // get random timer to lure a companion
                float finalMaxLureCooldown = maxLureCooldown - (maxLureCooldown * PlayerManager.Instance.PlayerFarmerData.CurrentKindness);

                if (reducedLureCooldownCheat && SettingsManager.Instance.AreCheatsEnabled)
                {
                    timerLureSlot4 = 30f;
                    //Debug.Log("started lure slot 4");
                }
                else
                {
                    timerLureSlot4 = UtilsGeneral.GetRandomValueBtwValues(minLureCooldown, finalMaxLureCooldown);
                }
            }
        }
        else
        {
            if (timerLureSlot4 <= 0)
            {
                // get random companion from possible of crops and spawn
                int randCompanionIndex = Random.Range(0, cropSlot4.CropSO.AttractedCompanions.Length);
                SpawnCompanion(3, cropSlot4.CropSO.AttractedCompanions[randCompanionIndex], slot4);

                // reset lure so the timer resets
                isLuringSlot4 = false;
            }
            else
            {
                timerLureSlot4 -= Time.deltaTime;
            }
        }
    }

    private void SpawnCompanion(int slotIndex, CompanionSO so, WorldCropSlot worldSlot)
    {
        // get prefab from so
        GameObject companionPrefab = so.Prefab;

        float offset = 200f;
        Vector2 spawnPos = Vector2.zero;

        // random spawn right or left
        if(Random.value < 0.5f)
        {
            // -4f y
            spawnPos = new Vector2(InitializerManager.Instance.GetScreenOffsetBound() - offset, ySpawn);
        }
        else
        {
            spawnPos = new Vector2(InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound() + offset, ySpawn);
        }

        spawnPos = Camera.main.ScreenToWorldPoint(spawnPos);
        
        // spawn
        GameObject prefab = Instantiate(companionPrefab, spawnPos, Quaternion.identity);
        prefab.GetComponent<Companion>().SetupBefriend(so, worldSlot.CropTransforms.First().position, slotIndex);
    }

    private void HandleUpdateGrowth()
    {
        if (timerCropGrowth <= 0)
        {
            UpdateCropData(0, cropSlot1);
            UpdateCropData(1, cropSlot2);
            UpdateCropData(2, cropSlot3);
            UpdateCropData(3, cropSlot4);

            UpdateWorldCrop(slot1, cropSlot1);
            UpdateWorldCrop(slot2, cropSlot2);
            UpdateWorldCrop(slot3, cropSlot3);
            UpdateWorldCrop(slot4, cropSlot4);

            timerCropGrowth = 1f;
        }
        else
        {
            timerCropGrowth -= Time.deltaTime;
        }
    }

    private void HandleUpdateSave()
    {
        if (timerAutoSave <= 0)
        {
            PlayerManager.Instance.SaveFarmerData();

            timerAutoSave = 60f;
        }
        else
        {
            timerAutoSave -= Time.deltaTime;
        }
    }

    public void SetCrop(int slot, CropData data, bool animation)
    {
        WorldCropSlot selectedSlot = null;
        bool wasEmpty = true;

        switch (slot)
        {
            case 0:
                if (cropSlot1 != null)
                    wasEmpty = false;

                cropSlot1 = data;
                selectedSlot = slot1;
                isLuringSlot1 = false;
                break;

            case 1:
                if (cropSlot2 != null)
                    wasEmpty = false;

                cropSlot2 = data;
                selectedSlot = slot2;
                isLuringSlot2 = false;
                break;

            case 2:
                if (cropSlot3 != null)
                    wasEmpty = false;

                cropSlot3 = data;
                selectedSlot = slot3;
                isLuringSlot3 = false;
                break;

            case 3:
                if (cropSlot4 != null)
                    wasEmpty = false;

                cropSlot4 = data;
                selectedSlot = slot4;
                isLuringSlot4 = false;
                break;
        }

        if (!animation)
        {
            SetCropSprite(slot, data);
        }
        else
        {
            // if the crop was full, animate vfx disappear
            if (!wasEmpty)
            {
                selectedSlot.PlayEmptyVFX();
            }

            selectedSlot.SetSprite(null);
            selectedSlot.SetCanGrow(false);

            CropSlotData cropSlotData = new CropSlotData(data, slot);

            player.AddSow(cropSlotData, selectedSlot.CropTransforms);
        }
    }

    public void SetCropSprite(int slot, CropData data)
    {
        Sprite currentSprite = data.GetCurrentSprite();
        WorldCropSlot selectedSlot = null;

        switch (slot)
        {
            case 0:
                cropSlot1 = data;
                selectedSlot = slot1;
                break;

            case 1:
                cropSlot2 = data;
                selectedSlot = slot2;
                break;

            case 2:
                cropSlot3 = data;
                selectedSlot = slot3;
                break;

            case 3:
                cropSlot4 = data;
                selectedSlot = slot4;
                break;
        }

        if (selectedSlot != null && currentSprite != null)
        {
            selectedSlot.SetSprite(currentSprite);
            selectedSlot.SetCanGrow(true);
        }
    }

    private void UpdateCropData(int slot, CropData data)
    {
        if (data == null) return;

        if (data.IsFullyGrown) return;

        // reduce growth cheat
        if (reducedGrowthTimeCheat && SettingsManager.Instance.AreCheatsEnabled)
        {
            data.AddGrowth(20);
        }
        else
        {
            data.AddGrowth(1);
        }

        //Debug.Log("slot " + slot + ", crop growth: " + data.CurrentGrowth);
        PlayerManager.Instance.PlayerFarmerData.UpdateCropToSlot(data, slot);
    }

    private void UpdateWorldCrop(WorldCropSlot slot, CropData data)
    {
        if (data == null) return;

        if (!slot.CanGrow) return;

        Sprite currentSprite = data.GetCurrentSprite();
        if (slot != null && currentSprite != null)
            slot.SetSprite(currentSprite);
    }
}
