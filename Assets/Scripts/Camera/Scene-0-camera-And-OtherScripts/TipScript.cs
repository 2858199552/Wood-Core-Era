using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipScript : MonoBehaviour
{
    public GameObject tipCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        { 
            Time.timeScale = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "MoveTip": tipCanvas.transform.GetChild(0).gameObject.SetActive(true); break;
            case "JumpTip": tipCanvas.transform.GetChild(1).gameObject.SetActive(true); break;
            case "DashTip": tipCanvas.transform.GetChild(2).gameObject.SetActive(true); Time.timeScale = 0f; break;
            case "ClimbTip": tipCanvas.transform.GetChild(3).gameObject.SetActive(true); break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "MoveTip": tipCanvas.transform.GetChild(0).gameObject.SetActive(false); break;
            case "JumpTip": tipCanvas.transform.GetChild(1).gameObject.SetActive(false); break;
            case "DashTip": tipCanvas.transform.GetChild(2).gameObject.SetActive(false); break;
            case "ClimbTip": tipCanvas.transform.GetChild(3).gameObject.SetActive(false); break;
        }
    }
}
