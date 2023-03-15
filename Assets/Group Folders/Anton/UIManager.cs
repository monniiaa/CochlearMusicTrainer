using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
   
    public static bool GameIsPaused = false;
    
    void Update()
    {

    }

    void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("HovedmenuTEST");
        Debug.Log("Loadede Hovedmenu");
    }
}
