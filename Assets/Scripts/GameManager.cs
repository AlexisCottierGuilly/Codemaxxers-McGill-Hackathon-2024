using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;

    public int n = 1;
    public int l = 0;
    public int m = 0;

    public bool neonMode = false;
    
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        score = 0;
    }

    public void OpenScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
