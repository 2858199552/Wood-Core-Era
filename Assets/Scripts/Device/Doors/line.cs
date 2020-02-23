using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public LineRenderer linee;
    void Start()
    {
        linee = GetComponent<LineRenderer>();
        Color color1 = Color.blue;
        Color color2 = Color.red;
    }

    void Update()
    {
        Vector3[] sum = new Vector3[2];
        sum[0] = a.transform.position;
        sum[1] = b.transform.position;
        linee.SetPositions(sum);
    }
}
