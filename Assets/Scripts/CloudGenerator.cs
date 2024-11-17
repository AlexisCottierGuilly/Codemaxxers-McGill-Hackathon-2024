using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics

public class CloudGenerator : MonoBehaviour
{
    public GameObject electronsParent;
    public GameObject electronPrefab;

    public OrbitalGenerator orbitalGenerator;

    public int seed;
    public float maxRadius = 0f;

    void Start()
    {
        seed = Random.Range(0, 1000000);
        //SpawnElectrons(1000);
        LoadElectrons(5000);
    }

    int Factorial(int n)
    {
        int total = 1;
        for (int i=n; n>0; n--)
        {
            total *= i;
        }   
        return total;
    }

    float ProbabilityFunction2(float x, float y, float z)
    {
        //return Mathf.Pow(Mathf.Exp(1), -(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f) + Mathf.Pow(z, 2f)));
        return Mathf.Pow(Mathf.Exp(1), -(Mathf.Pow(x, 4f) + Mathf.Pow(y, 2f) + Mathf.Pow(z, 2f)));
    }

    float ProbabilityFunction(float theta, float phi, float radius)
    {
        //return 0.5f/radius;
        return Mathf.PerlinNoise(theta+seed, phi+seed) / Mathf.Exp(radius);

        //float a = 1f;

        //return (Mathf.Pow(radius, 2f)*Mathf.Exp(-radius/a)*Mathf.Pow(Mathf.Sin(theta), 2f))/(24*Mathf.Pow(a, 3f)*Mathf.PI);
    }

    float RandomFloat(System.Random rng, float mn, float mx)
    {
        float nb = (float)rng.Next() / System.Int32.MaxValue;
        return mn + (mx - mn) * nb;
    }

    void LoadElectrons(int iterations)
    {
        maxRadius = 0f;
        float scaling = 5f;

        List<List<float>> allCoords = orbitalGenerator.GenerateRandomValues(iterations);

        float maxProb = 0f;
        foreach (List<float> coords in allCoords)
        {
            if (coords[3] > maxProb)
                maxProb = coords[3];
            
            float dist = Mathf.Sqrt(Mathf.Pow(coords[0], 2f) + Mathf.Pow(coords[1], 2f) + Mathf.Pow(coords[2], 2f)) * scaling;
            if (dist > maxRadius)
                maxRadius = dist;
        }
        float probMultiplier = 8f / maxProb;

        foreach (List<float> coords in allCoords)
        {
            float x = coords[0];
            float y = coords[1];
            float z = coords[2];
            float prob = coords[3];

            GameObject electron = Instantiate(electronPrefab, new Vector3(x * scaling, y * scaling, z * scaling), Quaternion.identity, electronsParent.transform);
            MeshRenderer renderer = electron.GetComponent<MeshRenderer>();
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, prob * probMultiplier);
        }
    }

    void SpawnElectrons(int iterations)
    {
        System.Random rng = new System.Random();
        Vector2 range = new Vector2(-1f, 1f);

        int i = 0;
        while (i < iterations)
        {
            float x = RandomFloat(rng, range.x, range.y);
            float y = RandomFloat(rng, range.x, range.y);
            float z = RandomFloat(rng, range.x, range.y);

            float radius = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f) + Mathf.Pow(z, 2f));
            float theta = Mathf.Acos(z / radius);
            float phi = Mathf.Sign(y) * Mathf.Acos(x/(Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f))));

            //float prb = ProbabilityFunction(x, y, z);
            float prb = ProbabilityFunction(theta, phi, radius);

            float scaling = 5f;

            float T = (float)Random.Range(0, 100000) / 100000f;

            if (Mathf.Pow(prb, 6f) > T)
            {
                i++;
                // To get cool graphics
                prb = Mathf.Pow(prb, 3f);

                GameObject electron = Instantiate(electronPrefab, new Vector3(x * scaling, y * scaling, z * scaling), Quaternion.identity, electronsParent.transform);
                MeshRenderer renderer = electron.GetComponent<MeshRenderer>();
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, prb);
            }
        }
    }
}
