using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider2D coll = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("true");
        if(other.name == "Player")
        {
            print("sss");
        }
        if(other.tag == "Player")
        {
            print("sss");
        }
    }
}
