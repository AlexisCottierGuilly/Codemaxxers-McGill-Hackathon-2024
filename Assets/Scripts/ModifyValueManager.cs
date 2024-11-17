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

    public bool addKeyEnabled = false;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { DidClick(); });
    }

    void Update()
    {
        if (addKeyEnabled)
        {
            int value = 0;
            if (Input.GetKeyDown(KeyCode.Equals))
                value += 1;    
            else if (Input.GetKeyDown(KeyCode.Minus))
                value -= 1;
            
            if (value != 0)
            {
                int lastValue = GameManager.instance.n;
                lastValue += value;
                lastValue = Mathf.Max(1, Mathf.Min(lastValue, GameManager.instance.l + 3));
                GameManager.instance.n = lastValue;

                if (lastValue <= GameManager.instance.l)
                    GameManager.instance.l = lastValue - 1;
            }
        }
    }

    void DidClick()
    {
        if (parameter == OrbitalParameter.N)
        {
            int value = GameManager.instance.n;
            value += modifier;
            value = Mathf.Max(1, Mathf.Min(value, GameManager.instance.l + 3));
            GameManager.instance.n = value;

            if (value <= GameManager.instance.l)
                GameManager.instance.l = value - 1;
        }
        else if (parameter == OrbitalParameter.L)
        {
            int value = GameManager.instance.l;
            value += modifier;
            value = Mathf.Max(0, Mathf.Min(value, 3));
            GameManager.instance.l = value;

            if (value >= GameManager.instance.n)
                GameManager.instance.n = value + 1;
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