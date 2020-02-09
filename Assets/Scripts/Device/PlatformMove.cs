using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float SpeedMove = 4;
    private Vector3[] platform;
    private int count = 0, num;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        num = this.transform.childCount;
        platform = new Vector3[num];
        for (int i = 0; i < num; i++)
        {
            platform[i] = this.transform.GetChild(i).position;
        }
    }
    private void Update()
    {
        MovePt();
    }
    void MovePt()
    {
        if (Vector3.Distance(rb.position, platform[count]) <= 0.1f)
        {
            if (count == num - 1)
            {
                count = 0;
            }
            else
                count++;
        }
        rb.transform.position += Vector3.Normalize(platform[count] - this.transform.position) * SpeedMove * Time.deltaTime;

    }
}