using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    
    public Transform head;
    public float spawnDistance = 2;
    public InputActionProperty showMenuButton;

    private Vector3 relativePosition;

    void Update()
    {
        if (showMenuButton.action.WasPressedThisFrame())
        {
            Pause();
            Debug.Log("Menu Pressed!");

            if (menu.activeSelf)
            {
                menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
                
            }
        }

        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }
    //----------------------------------------

    public static bool GameIsPaused = false;
    
   
    void Resume()
    {
        menu.SetActive(menu.activeSelf);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        menu.SetActive(!menu.activeSelf);
        
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
