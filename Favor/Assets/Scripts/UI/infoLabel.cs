using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class infoLabel : MonoBehaviour
{
    [SerializeField]
    string key;
    [SerializeField]
    string value;
    [SerializeField]
    
    TextMeshProUGUI t;

    private void Start()
    {
        
        t = gameObject.GetComponent<TextMeshProUGUI>();
        refresh();
    }


    public void updateValue(string _value)
    {
        value = _value;
        refresh();
    }

    public void updateValue(int _value)
    {
        updateValue(_value.ToString().PadRight(4));
    }
    public void updateKey(string _key)
    {
        key = _key;
        refresh();
    }
    
    void refresh()
    {
        string output = key + ": " + value;
        t.SetText(output);
    }

}
