using UnityEngine;

public class SelfActiveAtAwake : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(true);
    }
}
