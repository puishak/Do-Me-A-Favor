using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButton : MonoBehaviour
{

    [SerializeField]
    Button rbMale;
    [SerializeField]
    Button rbFemale;

    public void Male()
    {
        rbMale.enabled = false;
        rbFemale.enabled = true;
    }

    public void Female()
    {
        rbMale.enabled = true;
        rbFemale.enabled = false;
    }
    
}
