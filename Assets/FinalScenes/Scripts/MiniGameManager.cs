using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MiniGame
{
    SoundLocalization = 0,
    PitchIdentification = 1,
    SoundSeparation = 2
}

public class MiniGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject rightHandController;
    private GameObject _rightHandComponent;
    private GameObject _miniGamePrefab;
    public IMiniGame CurrentMiniGame;
    
    private static MiniGameManager _instance;

    public static MiniGameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MiniGameManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SwitchMiniGame(1);
    }

    public void SwitchMiniGame(int miniGameId)
    {

        CurrentMiniGame = MiniGameCreator((MiniGame) miniGameId);
        
        if(_miniGamePrefab != null) Destroy(_miniGamePrefab);
        if(_rightHandComponent != null) Destroy(_rightHandComponent);

        _miniGamePrefab = Instantiate(Resources.Load<GameObject>($"MiniGamePrefabs/{CurrentMiniGame.MiniGamePrefabPath}"));
        _rightHandComponent = AttachRightHandControllerComponent(Resources.Load<GameObject>($"RigPrefabs/{CurrentMiniGame.RigPrefab}"));
    }

    private IMiniGame MiniGameCreator(MiniGame miniGame)
    {
        return miniGame switch
        {
            MiniGame.SoundSeparation => new SoundSeparationMiniGame(),
            MiniGame.PitchIdentification => new PitchIdentificationMiniGame(),
            _ => null
        };
    }

    private GameObject AttachRightHandControllerComponent(GameObject prefab) => Instantiate(prefab, rightHandController.transform);

}
