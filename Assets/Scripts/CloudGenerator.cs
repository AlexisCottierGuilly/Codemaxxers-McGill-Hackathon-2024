using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public GameObject electronsParent;
    public GameObject electronPrefab;

    float ProbabilityFunction(float x, float y, float z)
    {
        return Mathf.Pow(Mathf.Exp(1), -(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f) + Mathf.Pow(z, 2f)));
    }

    float RandomFloat(System.Random rng, float mn, float mx)
    {
        float nb = (float)rng.Next() / System.Int32.MaxValue;
        return mn + (mx - mn) * nb;
    }

    void SpawnElectrons(int iterations)
    {
        float T = 0.5f;
        System.Random rng = new System.Random();
        Vector2 range = new Vector2(-1f, 1f);

        for (int i=0; i < iterations; i++)
        {
            float x = RandomFloat(rng, range.x, range.y);
            float y = RandomFloat(rng, range.x, range.y);
            float z = RandomFloat(rng, range.x, range.y);
            float prb = ProbabilityFunction(x, y, z);

            float scaling = 5f;

            if (prb > T)
            {
                // To get cool graphics
                prb = Mathf.Pow(prb, 4f);

                GameObject electron = Instantiate(electronPrefab, new Vector3(x * scaling, y * scaling, z * scaling), Quaternion.identity, electronsParent.transform);
                MeshRenderer renderer = electron.GetComponent<MeshRenderer>();
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, prb);
            }
            Debug.Log(i);
        }
    }

    void Start()
    {
        SpawnElectrons(5000);
    }
}
