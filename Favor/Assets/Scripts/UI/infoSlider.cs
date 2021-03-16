using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class infoSlider : MonoBehaviour
{

    [Header("Display")]
    [SerializeField]
    string key;
    [SerializeField]
    [Range(0, 100)]
    int value;

    [Header("Components")]
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    Slider slider;
    

    private void Start()
    {
        refresh();
    }

    public void updateKey(string _key)
    {
        key = _key;
        refresh();
    }

    public void updateValue(int _value)
    {
        value = _value;
        refresh();
    }


    void refresh()
    {
        text.SetText(key + ": ");
        slider.value = value;
    }
}
