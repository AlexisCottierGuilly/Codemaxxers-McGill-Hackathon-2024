using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalGenerator : MonoBehaviour
{
    // Random number generator
    private System.Random random = new System.Random();
    float max_rho = 7f;

    void Start()
    {

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

    public float GetPsi(float rho, float theta, float phi, int n, int l)
    {
        if (n == 1 && l == 0)
            return Mathf.Exp(-rho) * Mathf.Pow(rho, l) * 1f;
        else if (n == 2 && l == 0)
            return Mathf.Exp(-rho) * Mathf.Pow(rho, l) * (-rho + 1f);
        else if (n == 2 && l == 1)
            return Mathf.Exp(-rho) * Mathf.Pow(rho, l) * (-rho + 1f) * Mathf.Cos(theta);
        else
            return 1;
    }

    public float GetGeneralPsi(float rho, float theta, float phi, int n, int l, int m)
    {
        float total = Mathf.Exp(-rho) * Mathf.Pow(rho, l);

        switch (n)
        {
            case 1:
                total *= 1f;
                break;
            case 2:
                total *= -rho + 1f;
                break;
            case 3:
                total *= 0.5f * (Mathf.Pow(rho, 2f) - 4f * rho + 2f);
                break;
            case 4:
                total *= (1/6f) * (-Mathf.Pow(rho, 3f) + 9f * Mathf.Pow(rho, 2f) - 18f * rho + 6f);
                break;
        }

        if (l == 1)
        {
            switch (m)
            {
                case -1:
                    total *= Mathf.Sin(theta) * Mathf.Cos(phi);
                    break;
                case 0:
                    total *= Mathf.Cos(theta);
                    break;
                case 1:
                    total *= Mathf.Sin(theta) * Mathf.Sin(phi);
                    break;
            }
        }
        else if (l == 2)
        {
            total *= 3 * Mathf.Pow(Mathf.Cos(theta), 2f) - 1;
        }
        else if (l == 3)
        {
            total *= 5f * Mathf.Pow(Mathf.Cos(theta), 3f) - 3f * Mathf.Cos(theta);
        }
        else if (l == 4)
        {
            total *= 35f * Mathf.Pow(Mathf.Cos(theta), 4f) - 30f * Mathf.Pow(Mathf.Cos(theta), 2f) + 3f;
        }

        return total;
    }

    // Function to generate random values for x, y, z, rho, theta, and phi
    public List<List<float>> GenerateRandomValues(int num)
    {
        int n = 1;
        int l = 3;
        int m = 0;

        max_rho *= (0.5f + n/2f);

        List<List<float>> results = new List<List<float>>();
        float targetProb = GetGeneralPsi(0, 0, 0, n, l, m) * GetGeneralPsi(0, 0, 0, n, l, m);

        int cnt = 0;

        while (cnt < num)
        {
            // float theta = (float)(random.NextDouble() * Mathf.PI);
            // float phi = (float)(random.NextDouble() * 2 * Mathf.PI);
            // float rho = (float)(random.NextDouble() * MAX_RHO);

            float x = (float)(random.NextDouble() * 2 * max_rho - max_rho);
            float y = (float)(random.NextDouble() * 2 * max_rho - max_rho);
            float z = (float)(random.NextDouble() * 2 * max_rho - max_rho);

            float rho = Mathf.Sqrt(x * x + y * y + z * z);
            float theta = Mathf.Acos(z / rho);
            float phi = Mathf.Atan2(y, x);

            float psi = GetGeneralPsi(rho, theta, phi, n, l, m);
            float prob = psi * psi;

            float alpha = prob / targetProb;
            float u = (float)random.NextDouble();

            if (u <= alpha)
            {
                targetProb = prob;
                x = rho * Mathf.Sin(theta) * Mathf.Cos(phi);
                y = rho * Mathf.Sin(theta) * Mathf.Sin(phi);
                z = rho * Mathf.Cos(theta);

                results.Add(new List<float> {x, y, z, prob});
                cnt++;
            }
        }

        return results;
    }
}
