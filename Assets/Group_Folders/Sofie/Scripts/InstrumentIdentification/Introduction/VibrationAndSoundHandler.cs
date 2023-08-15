using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VibrationAndSoundHandler : MonoBehaviour
{
    public delegate void Vibration(bool on);
    public event Vibration VibrationEvent;
    
    public delegate void Sound(bool on);
    public event Sound SoundEvent;

    [SerializeField] private Image soundImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Image SoundDiagonalImage;
    
    [SerializeField] private Image vibrationImage;
    [SerializeField] private Sprite vibrationOnSprite;
    [SerializeField] private Sprite vibrationOffSprite;
    [SerializeField] private Image vibrationDiagonalImage;

    private bool soundOn = true;
    private bool vibrationOn = true;
    
    [SerializeField] private Button soundButton;
    [SerializeField] private Button vibrationButton;

    [SerializeField] private TextMeshProUGUI vibrationfeedbackText;
    [SerializeField] private TextMeshProUGUI soundfeedbackText;
    private void Start()
    {
        soundButton.onClick.AddListener(SoundButtonClicked);
        vibrationButton.onClick.AddListener(VibrationButtonClicked);
        
        soundImage.sprite = soundOnSprite;
        vibrationImage.sprite = vibrationOnSprite;
        SoundDiagonalImage.enabled = false;
        vibrationDiagonalImage.enabled = false;
    }

    private void OnEnable()
    {
        vibrationfeedbackText.gameObject.SetActive(false);
        soundfeedbackText.gameObject.SetActive(false);
    }

    private void VibrationButtonClicked()
    {
        vibrationOn = !vibrationOn;
        vibrationfeedbackText.gameObject.SetActive(false);
        if (vibrationOn)
        {
            vibrationImage.sprite = vibrationOnSprite;
            vibrationDiagonalImage.enabled = false;
            vibrationfeedbackText.text = "Vibrationer tændt";
            vibrationfeedbackText.gameObject.SetActive(true);
        }
        else
        {
            vibrationImage.sprite = vibrationOffSprite;
            vibrationDiagonalImage.enabled = true;
            vibrationfeedbackText.text = "Vibrationer slukket";
            vibrationfeedbackText.gameObject.SetActive(true);
        }
        
        if(VibrationEvent != null) VibrationEvent(vibrationOn);
    }

    private void SoundButtonClicked()
    {
        soundfeedbackText.gameObject.SetActive(false);
        soundOn = !soundOn;
        if (soundOn)
        {
            soundImage.sprite = soundOnSprite;
            SoundDiagonalImage.enabled = false;
            soundfeedbackText.text = "Lyd tændt";
            soundfeedbackText.gameObject.SetActive(true);
        }
        else
        {
            soundImage.sprite = soundOffSprite;
            SoundDiagonalImage.enabled = true;
            soundfeedbackText.text = "Lyd slukket";
            soundfeedbackText.gameObject.SetActive(true);
        }
        if(SoundEvent != null) SoundEvent(soundOn);
    }
    
}
