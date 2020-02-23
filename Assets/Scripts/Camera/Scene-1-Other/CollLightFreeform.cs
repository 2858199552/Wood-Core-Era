using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollLightFreeform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
