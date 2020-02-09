using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject SaveMenuBack;
    public GameObject SetUp;
    public GameObject Achiment;
    public GameObject HandBook;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
//暂停And开始
    public void pausegame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void BackGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
//存档界面
    public void SaveMenuBackGround()
    {
        SaveMenuBack.SetActive(true);
    }
    public void SaveMenuBackGroundToMenu()
    {
        SaveMenuBack.SetActive(false);
    }
//设置界面
    public void SetUpTrue()
    {
        SetUp.SetActive(true);
    }
    public void SetUpFlase()
    {
        SetUp.SetActive(false);
    }
//成就界面
    public void AchimentTrue()
    {
        Achiment.SetActive(true);
    }
    public void AchimentFalse()
    {
        Achiment.SetActive(false);
    }
//图鉴界面
    public void HandBookTrue()
    {
        HandBook.SetActive(true);
    }
    public void HandBookFalse()
    {
        HandBook.SetActive(false);
    }

    //Esc按钮（未设置）
    public void EscBottom()
    {
        if (Input.GetButton("Cancel"))
        {
            pausegame();
        }
    }
}
