using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject target;
    //public PlayerControl player;

    void Update()
    {
        //player = GetComponent<PlayerControl>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 tempVec = (collision.gameObject.transform.position - this.transform.position) * 1.5f;
        if (collision.gameObject.name == "Player (1)")
        {
            ShakeCamera.instance.isshakeCamera = true;
            //player.dashCounter = player.maxDashCount;
            collision.gameObject.transform.position = target.transform.position - tempVec;
        }
    }
}
