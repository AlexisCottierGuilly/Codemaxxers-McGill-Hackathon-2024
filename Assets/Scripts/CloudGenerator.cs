using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public GameObject electronsParent;
    public GameObject electronPrefab;

    float ProbabilityFunction(float x, float y, float z)
    {
        return Mathf.Pow(Mathf.Exp(1), Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f) + Mathf.Pow(z, 2f));
    }

    float RandomFloat(System.Random rng, float mn, float mx)
    {
        float nb = (float)rng.Next() / System.Int32.MaxValue;
        return mn + (mx - mn) * nb;
    }

    void SpawnElectrons(int iterations)
    {
        System.Random rng = new System.Random();
        Vector2 range = new Vector2(-5f, 5f);

        for (int i=0; i < iterations; i++)
        {
            float x = RandomFloat(rng, range.x, range.y);
            float y = RandomFloat(rng, range.x, range.y);
            float z = RandomFloat(rng, range.x, range.y);
            float prb = ProbabilityFunction(x, y, z);
            
            GameObject electron = Instantiate(electronPrefab, new Vector3(x, y, z), Quaternion.identity, electronsParent.transform);
        }
    }

    void Start()
    {
        SpawnElectrons(100);
    }
}
