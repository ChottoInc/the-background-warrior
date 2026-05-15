using UnityEngine;

public abstract class AbstractPlayerJobData : ScriptableObject
{
    [SerializeField] UtilsPlayer.PlayerJob id;
    [SerializeField] string dataPath;

    public UtilsPlayer.PlayerJob Id => id;
    public string DataPath => dataPath;
}
