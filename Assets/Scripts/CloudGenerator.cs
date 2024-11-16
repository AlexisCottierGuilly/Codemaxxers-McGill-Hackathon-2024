using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics

public class CloudGenerator : MonoBehaviour
{
    public GameObject electronsParent;
    public GameObject electronPrefab;

    float ProbabilityFunction2(float x, float y, float z)
    {
        //return Mathf.Pow(Mathf.Exp(1), -(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f) + Mathf.Pow(z, 2f)));
        return Mathf.Pow(Mathf.Exp(1), -(Mathf.Pow(x, 4f) + Mathf.Pow(y, 2f) + Mathf.Pow(z, 2f)));
    }

    float ProbabilityFunction(float theta, float radius)
    {
        return 0.5f/radius;
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
            float prb = ProbabilityFunction(theta, radius);

            float scaling = 5f;

            if (prb > T)
            {
                i++;
                // To get cool graphics
                prb = Mathf.Pow(prb, 4f);

                GameObject electron = Instantiate(electronPrefab, new Vector3(x * scaling, y * scaling, z * scaling), Quaternion.identity, electronsParent.transform);
                MeshRenderer renderer = electron.GetComponent<MeshRenderer>();
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, prb);
            }
        }
    }

    void Start()
    {
        SpawnElectrons(1000);
    }

    Complex GetSlaterTypeOrbital(int, n, int l, int m, float r, float theta, float phi, float zeta)
    {
        return (AngularPart(l, m, theta, phi, zeta) * RadialPart(r * n * zeta))
    }

    Complex AngularPart(int l, int m, float theta, float phi, float zeta)
    {
        Complex exponentialPart = Complex.Exp(new Complex(0, m * phi))
        
        return (get_normalization_constant(zeta, n) * exponentialPart * P(n, l, Mathf.Cos(theta)))
    }

    float RadialPart(float r, int n, float zeta)
    {
        return (get_normalization_constant(zeta, n) * Mathf.Pow(r, n - 1) * Mathf.Exp(zeta * r * -1))
    }

    float P(int l, int m, float theta)
    {
        int sum_bound = ((l - Mathf.Abs(m)) / 2) + 1/4 * (Mathf.Pow(-1, l - Mathf.Abs(m)) - 1);
        int p_sum = 0;
        for (int k = 0; k < sum_bound; k++)
        {
            p_sum += Mathf.Pow(-1, k) * (Mathf.Factorial((2 * l) - (2 * k))/(Mathf.Factorial(k)* Mathf.Factorial(l - k) * Mathf.Factorial(l - Mathf.Abs(m) - 2 * k))) * Mathf.Pow(Mathf.Cos(theta), l - Mathf.Abs(m) - 2 * k);
        }
        float first_part = (1/Mathf.Pow(2, l)) * Mathf.Sqrt(((2 * l + 1) / 2) * Mathf.Factorial(l - Mathf.Abs(m))/Mathf.Factorial(l + Mathf.Abs(m))) * Mathf.Pow(Mathf.Pow(Mathf.Cos(theta), 2) + 1, Mathf.Abs(m) / 2);
        return (p_sum * first_part);
    }

    float get_normalization_constant(float zeta, int n)
    {
        float first_part = Mathf.Pow(2 * zeta, n);
        float second_part = Mathf.Sqrt((2 * zeta) / Mathf.Factorial(2 * n));
        return (first_part * second_part);
    }
}
