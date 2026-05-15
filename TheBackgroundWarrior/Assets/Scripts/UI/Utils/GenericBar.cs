using UnityEngine;
using UnityEngine.UI;

public class GenericBar : MonoBehaviour
{
    [SerializeField] protected Slider slider;

    public float Value => slider.value;
    public float MaxValue => slider.maxValue;

    public void Setup(float maxValue, float currentValue)
    {
        slider.maxValue = maxValue;
        slider.value = currentValue;
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetCurrentValue(float value) 
    {
        slider.value = value;
    }

}
