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
            ActivateDeactivateMenu();
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
    
    public void ActivateDeactivateMenu()
    {
        menu.SetActive(!menu.activeSelf);
        if(GameIsPaused)
        {
            ResumeTime();
        }
        else
        {
            PauseTime();
        }
    }
   
    void ResumeTime(){
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void PauseTime(){ 
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeTime();
    }
    // void Start() {
    //     ResumeTime();
    // }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Vælg Bane");
        Debug.Log("Loadede Hovedmenu");
    }

    public void LoadPitchTrainingScene()
    {
        Debug.Log("Loaded Pitch training");
    }
    public void LoadUIScene()
    {
        SceneManager.LoadScene("UI");
        Debug.Log("Loaded UI");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT Game!");
    }
}
