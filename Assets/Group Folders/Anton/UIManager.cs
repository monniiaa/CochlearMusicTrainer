using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Text scoreText;
    public Button playButton;
    public Button quitButton;

    void Start()
    {
        menuPanel = GameObject.Find("MenuPanel");
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
    }

    public void SetScoreText(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void SetPlayButtonActive(bool isActive)
    {
        playButton.gameObject.SetActive(isActive);
    }

    public void SetQuitButtonActive(bool isActive)
    {
        quitButton.gameObject.SetActive(isActive);
    }
}
