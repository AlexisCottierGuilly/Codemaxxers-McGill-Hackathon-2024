using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalEnergiesGraphManager : MonoBehaviour
{
    public float lineWidth = 2f;

    public GameObject anchor;
    public GameObject verticalAxis;

    public GameObject orbitalPrefab;
    public GameObject orbitalsParent;

    public List<DataInfo> data = new List<DataInfo>();
    List<GameObject> orbitals = new List<GameObject>();

    private float maxValue;
    private float maxPosition;

    void Start()
    {
        UpdateMaxValues();
        InitializeGraph();
    }

    void InitializeGraph()
    {
        // Get the data into the graph

        foreach (DataInfo point in data)
        {
            float yPosition = EnergyToYCoordinate(point.energy);
            float xPosition = anchor.transform.position.x + verticalAxis.transform.localScale.x * 0.1f;
            
            GameObject orbital = Instantiate(orbitalPrefab, new Vector2(xPosition, yPosition), Quaternion.identity, orbitalsParent.transform);
            float xScale = orbital.transform.localScale.x * 100f * point.length;
            float newXPosition = xPosition + xScale / 2f;
            orbital.transform.position = new Vector3(
                newXPosition,
                orbital.transform.position.y,
                orbital.transform.position.z
            );
            orbital.SetActive(true);

            orbital.GetComponent<OrbitalInfo>().name.text = point.name;
        }
    }

    float EnergyToYCoordinate(float energy)
    {
        // Transform a data value into a UI coordinate

        float percentageOfMax = energy / maxValue;
        float distanceZeroMax = maxPosition - anchor.transform.position.y;
        return anchor.transform.position.y - percentageOfMax * distanceZeroMax / 0.45f;
    }

    void UpdateMaxValues()
    {
        maxValue = FindCorrectScaling();
        maxPosition = verticalAxis.transform.localScale.y;
    }

    float FindCorrectScaling()
    {
        // Find the scaling of the data depending on the data values given

        float scale = 0f;

        foreach (DataInfo point in data)
        {
            if (point.energy > scale)
                scale = point.energy;
        }

        return scale;
    }
}


[System.Serializable]
public class DataInfo
{
    // Little class to store the data points
    public string name = "1s";
    public float energy = 0f;
    public float length = 1f;
}
