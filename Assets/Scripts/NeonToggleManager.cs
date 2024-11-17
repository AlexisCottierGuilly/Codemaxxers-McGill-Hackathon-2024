using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class NeonToggleManager : MonoBehaviour
{
    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { DidToggle(); });
    }

    void DidToggle()
    {
        GameManager.instance.neonMode = !GameManager.instance.neonMode;
    }
}
