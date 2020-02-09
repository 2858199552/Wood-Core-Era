using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenArrow : MonoBehaviour
{
    [Header("基本设置")]
    public float offset = 0f;
    public float len = 20f;
    public LayerMask Player;
    public Rigidbody2D rbAr;

    [Header("监视")]
    public bool isRay;
    void Start()
    {

    }

    void Update()
    {
        isRay = false;
        RaycastHit2D hit = Raycast(new Vector2(offset, 0), Vector2.left + Vector2.up, len, Player);
        if (hit)
        {
            isRay = true;
        }

        Launch();
    }

    void Launch()
    {
        if (isRay)
        {
            rbAr.velocity = new Vector2(-10, 10);
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float len, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, len, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction * len, color);
        return hit;
    }
}
