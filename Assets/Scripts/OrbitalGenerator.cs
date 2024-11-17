using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalGenerator : MonoBehaviour
{
    // Random number generator
    private System.Random random = new System.Random();
    float max_rho = 7f;
    List<List<float>> basis = new List<List<float>>{new List<float>{0, 0, 0, 2.709498091f}, new List<float>{0, 0, 0, 1.012151084f}, new List<float>{1, 0, 0, 1.759666885f}, new List<float>{0, 1, 0, 1.759666885f}, new List<float>{0, 0, 1, 1.759666885f}};
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
    //     def get_normalization_constant(n):
    //     i, j, k, alpha = basis[n]
    //     leading_term = (2 * alpha / np.pi)  (3 / 4) * (4 * alpha)  ((i + j + k) / 2)
    //     factorial_term = np.prod(np.arange(2 * i - 1, 0, -2) * np.arange(2 * j - 1, 0, -2) * np.arange(2 * k - 1, 0, -2))
    //     return leading_term / np.sqrt(factorial_term)
    // normalisation = [get_normalization_constant(i) for i in range(NUM_BASIS)]



    // def get_gaussian_orbital(x, y, z, i, j, k, alpha):
    //     r2 = x * x + y * y + z * z
    //     return np.exp(-alpha * r2) * x  i * y  j * z ** k

    public int double_factorial(int n)
    {
        int total = 1;
        for (int i=n; n>0; n-=2)
        {
            total *= i;
        }   
        return total;
    }

    public float GetNormalizationConstant(int n)
    {
        int i = (int)basis[n][0];
        int j = (int)basis[n][1];
        int k = (int)basis[n][2];
        float alpha = basis[n][3];
        float leading_term = (2 * alpha / Mathf.PI)  * (3 / 4) * (4 * alpha)  * ((i + j + k) / 2);
        float factorial_term = (float)(double_factorial(2 * i - 1) * double_factorial(2 * j - 1) * double_factorial(2 * k - 1));
        return leading_term / Mathf.Sqrt(factorial_term);
    }
    public float GetGaussianOrbital(float x, float y, float z, int n) {
        int i = (int)basis[n][0];
        int j = (int)basis[n][1];
        int k = (int)basis[n][2];
        float alpha = basis[n][3];
        float r2 = x * x + y * y + z * z;
        return Mathf.Exp(-alpha * r2) * Mathf.Pow(x, i) * Mathf.Pow(y, j) * Mathf.Pow(z, k);
    }

    public float GetNeon(float x, float y, float z) {
        
        List<List<float>> P = new List<List<float>> {
            new List<float>{ 6.66151996e+00f, -4.47463509e+00f, -4.51935227e-02f, -4.51935227e-02f, -4.51935227e-02f},
            new List<float>{-4.47463509e+00f,  4.84450024e+00f,  9.27408624e-02f,  9.27408624e-02f, 9.27408624e-02f},
            new List<float>{-4.51935227e-02f,  9.27408624e-02f,  2.42302703e-03f,  2.42302703e-03f, 2.42302703e-03f},
            new List<float>{-4.51935227e-02f,  9.27408624e-02f,  2.42302703e-03f,  2.42302703e-03f, 2.42302703e-03f},
            new List<float>{-4.51935227e-02f,  9.27408624e-02f,  2.42302703e-03f,  2.42302703e-03f, 2.42302703e-03f}};

        float density = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                density += P[j][i] * GetGaussianOrbital(x, y, z, i) * GetGaussianOrbital(x, y, z, j);
            }
        }
        return density;
    }

    public float GetGeneralPsi(float rho, float theta, float phi, int n, int l, int m)
    {
        float total = Mathf.Exp(-rho) * Mathf.Pow(rho, l);

        // For each n and l, calculate the psi with different orbital equations.
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
            // TODO: Add each orbital rotations (this one works fine)
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
        int n = GameManager.instance.n;
        int l = GameManager.instance.l;
        int m = GameManager.instance.m;

        n -= Mathf.Max(l - 1, 0);

        max_rho *= (0.5f + n/2f);

        List<List<float>> results = new List<List<float>>();
        float targetProb;
        if (GameManager.instance.neonMode)
            targetProb = GetNeon(0, 0, 0);
        else
            targetProb = GetGeneralPsi(0, 0, 0, n, l, m) * GetGeneralPsi(0, 0, 0, n, l, m);
        
        int cnt = 0;

        while (cnt < num)
        {
            // Transform the spherical coordinates into 3D coordinates (x, y, z)
            float x = (float)(random.NextDouble() * 2 * max_rho - max_rho);
            float y = (float)(random.NextDouble() * 2 * max_rho - max_rho);
            float z = (float)(random.NextDouble() * 2 * max_rho - max_rho);

            float rho = Mathf.Sqrt(x * x + y * y + z * z);
            float theta = Mathf.Acos(z / rho);
            float phi = Mathf.Atan2(y, x);

            float prob;
            if (GameManager.instance.neonMode)
                prob = GetNeon(x, y, z) * x * x;
            else
            {
                float psi = GetGeneralPsi(rho, theta, phi, n, l, m);
                prob = psi * psi;
            }

            float alpha = prob / targetProb;
            float u = (float)random.NextDouble();

            // If the probability calculated at the point is close to the target, select the point
            if (u <= alpha)
            {
                targetProb = prob;
                x = rho * Mathf.Sin(theta) * Mathf.Cos(phi);
                y = rho * Mathf.Sin(theta) * Mathf.Sin(phi);
                z = rho * Mathf.Cos(theta);

                results.Add(new List<float> {x, y, z, prob});
                cnt++;
            }

            // continue to iterate until we have the good amout of data
        }

        return results;
    }

    public List<List<float>> ExtractNeonDensities()
    {
        var result = new List<List<float>>();
        string filePath = "Assets/Scripts/NeonDensities.txt";

        foreach (var line in System.IO.File.ReadLines(filePath))
        {
            var values = line.Split(new[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 4)
            {
                var point = new List<float>();

                foreach (var value in values)
                {
                    Debug.Log(value);
                    if (float.TryParse((string)value.Replace(".", ","), out var floatValue))
                    {
                        point.Add(floatValue);
                        Debug.Log(floatValue);
                    }
                    else
                    {
                        Debug.LogWarning($"Invalid float value found: {value} in line: {line}");
                    }
                }
                

                Debug.Log($"{point[0]}, {point[1]}, {point[2]}");

                result.Add(point);
            }
        }

        return result;
    }
}
