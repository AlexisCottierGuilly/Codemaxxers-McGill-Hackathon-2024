using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum OrbitalParameter
{
    N,
    L,
    M
}

[RequireComponent(typeof(Button))]
public class ModifyValueManager : MonoBehaviour
{
    public OrbitalParameter parameter = OrbitalParameter.N;
    public int modifier = 1;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { DidClick(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            int value = GameManager.instance.n;
            value += 1;
            value = Mathf.Max(1, value);
            GameManager.instance.n = value;
        }
    }

    void DidClick()
    {
        if (parameter == OrbitalParameter.N)
        {
            int value = GameManager.instance.n;
            value += modifier;
            value = Mathf.Max(1, value);
            GameManager.instance.n = value;
        }
        else if (parameter == OrbitalParameter.L)
        {
            int value = GameManager.instance.l;
            value += modifier;
            value = Mathf.Max(0, value);
            GameManager.instance.l = value;
        }
        else if (parameter == OrbitalParameter.M)
        {
            int value = GameManager.instance.m;
            value += modifier;
            value = Mathf.Max(-GameManager.instance.l, Mathf.Min(value, GameManager.instance.l));
            GameManager.instance.m = value;
        }
    }
}
