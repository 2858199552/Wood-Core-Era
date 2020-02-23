using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterColl : MonoBehaviour
{
    int playernum;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playernum = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playernum)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
