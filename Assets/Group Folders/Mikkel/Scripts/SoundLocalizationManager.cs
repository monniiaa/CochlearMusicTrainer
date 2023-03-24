using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLocalizationManager : MonoBehaviour
{
    [SerializeField]
    GameObject Prefab;
    GameObject Speaker;
    private int CurrentLevel;
    private int CurrentScore;
    private string path = "SoundLocalization";
    GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData = DataManager.ReadJson(path);
        CurrentLevel = gameData.level;
        Speaker = GameObject.FindGameObjectWithTag("InteractablePrefabTag");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndRound()
    {
        CurrentScore = ShootInteraction.points;
        Speaker.GetComponent<DeletusMaximus>().Destroy();
        Instantiate(Prefab);

    }
}
