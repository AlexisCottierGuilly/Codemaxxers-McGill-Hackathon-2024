using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonisationGraphManager : MonoBehaviour
{
    public float lineWidth = 2f;

    public GameObject anchor;
    public GameObject verticalAxis;
    public GameObject horizontalAxis;

    public GameObject linePrefab;
    public GameObject dotPrefab;
    public GameObject dotsParent;
    public GameObject linesParent;

    public List<Vector2> data = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();
    List<GameObject> lines = new List<GameObject>();

    private Vector2 maxValues;
    private Vector2 maxPosition;

    void Start()
    {
        UpdateMaxValues();
        InitializeGraph();
    }

    void InitializeGraph()
    {
        foreach (Vector2 point in data)
        {
            Vector2 position = PointToCoordinates(point);
            GameObject dot = Instantiate(dotPrefab, position, Quaternion.identity, dotsParent.transform);
            dots.Add(dot);
        }

        for (int i=0; i<data.Count-1; i++)
        {
            LineBetween(data[i], data[i+1]);
        }

        // https://chem.libretexts.org/Bookshelves/Physical_and_Theoretical_Chemistry_Textbook_Maps/Supplemental_Modules_(Physical_and_Theoretical_Chemistry)/Physical_Properties_of_Matter/Atomic_and_Molecular_Properties/Ionization_Energy
        // https://en.wikipedia.org/wiki/Molar_ionization_energies_of_the_elements
    }

    void UpdateMaxValues()
    {
        maxValues = FindCorrectScaling();
        maxPosition = new Vector2(
            horizontalAxis.transform.localScale.x,
            verticalAxis.transform.localScale.y
        );
    }

    void LineBetween(Vector2 point1, Vector2 point2)
    {
        Vector2 position1 = PointToCoordinates(point1);
        Vector2 position2 = PointToCoordinates(point2);

        float distance = Vector2.Distance(position1, position2) / 3.5f;
        Vector2 middle = new Vector2((position2.x + position1.x) / 2f, (position2.y + position1.y) / 2f);

        Vector2 scale = new Vector2(lineWidth, distance);
        Vector2 delta = new Vector2(position2.x - position1.x, position2.y - position1.y);
        float angle = Mathf.Atan(delta.y / delta.x) * Mathf.Rad2Deg;

        GameObject line = Instantiate(linePrefab, middle, Quaternion.identity, linesParent.transform);
        line.transform.Rotate(new Vector3(0f, 0f, angle + 90));
        line.transform.localScale = scale / 100f;
    }

    Vector2 PointToCoordinates(Vector2 point)
    {
        Vector2 percentageOfMax = new Vector2(
            point.x / maxValues.x,
            point.y / maxValues.y
        );
        Vector2 distanceZeroMax = new Vector2(
            maxPosition.x - anchor.transform.position.x,
            maxPosition.y - anchor.transform.position.y
        );

        return new Vector2(
            anchor.transform.position.x - percentageOfMax.x * distanceZeroMax.x,
            anchor.transform.position.y - percentageOfMax.y * distanceZeroMax.y / 0.65f
        );
    }

    Vector2 FindCorrectScaling()
    {
        Vector2 scaling = new Vector2(0f, 0f);

        foreach (Vector2 point in data)
        {
            if (point.x > scaling.x)
                scaling.x = (float)point.x;
            if (point.y > scaling.y)
                scaling.y = (float)point.y;
        }

        return scaling;
    }
}
