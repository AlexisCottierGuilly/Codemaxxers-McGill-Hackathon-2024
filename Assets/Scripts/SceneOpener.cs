using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneOpener : MonoBehaviour
{
    public string sceneName = "Menu";
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { DidClick(); });
    }

    void DidClick()
    {
        GameManager.instance.OpenScene(sceneName);
    }
}