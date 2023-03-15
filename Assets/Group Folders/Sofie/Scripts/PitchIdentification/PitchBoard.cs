using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PitchBoard : MonoBehaviour
{
    private Camera camera;
    RaycastHit hit;

    public UIMenu pitchModeMenu;
    public string path = "PitchIdentification";
    GameData levelData;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        levelData = DataManager.ReadJson(path);
        
        for (int i = 1; i <= levelData.level; i++)
        {
            pitchModeMenu.ActivateLevel(i);
            pitchModeMenu.ShowLevelInfo(i, levelData.levelScore[i - 1]);
        }
        
    }

    void Update()
    {
        DetectObjectWithRaycast();
    }



    public void DetectObjectWithRaycast()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "UnlockedLevel")
            {
                Debug.Log("hit");
                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene("PitchIdentification");
                }
            }
        }

    }
}
