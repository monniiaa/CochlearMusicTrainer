using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private Material greenButtonMaterial, redButtonMaterial;
    private Renderer CIbiButton, CIlButton, CIrButton, CInButton;

    private void Awake() 
    {
        //Assign button materials
        greenButtonMaterial = Resources.Load("Resource Materials/Green Button Material", typeof(Material)) as Material;
        redButtonMaterial = Resources.Load("Resource Materials/Red Button Material", typeof(Material)) as Material;

        CIbiButton = GameObject.FindWithTag("CIbiButton").GetComponent<MeshRenderer>();
        CIlButton = GameObject.FindWithTag("CIlButton").GetComponent<MeshRenderer>();;
        CIrButton = GameObject.FindWithTag("CIrButton").GetComponent<MeshRenderer>();;
        CInButton = GameObject.FindWithTag("CInButton").GetComponent<MeshRenderer>();;
    }

    public void SetCondition(int conditionIndex)
    {
        GameManager.Instance.currentCondition = conditionIndex;
        GameManager.Instance.SetCondition(GameManager.Instance.currentCondition);
    }

    public void SettingsButtonsColors(int buttonIndex)
    {
            switch (buttonIndex)
            {
                case 0:
                    CIbiButton.material = redButtonMaterial;
                    CIlButton.material = redButtonMaterial;
                    CIrButton.material = redButtonMaterial;
                    CInButton.material = greenButtonMaterial;
                break;

                case 1:
                    CIbiButton.material = redButtonMaterial;
                    CIlButton.material = greenButtonMaterial;
                    CIrButton.material = redButtonMaterial;
                    CInButton.material = redButtonMaterial;
                break;

                case 2:
                    CIbiButton.material = redButtonMaterial;
                    CIlButton.material = redButtonMaterial;
                    CIrButton.material = greenButtonMaterial;
                    CInButton.material = redButtonMaterial;
                break;

                case 3:
                    CIbiButton.material = greenButtonMaterial;
                    CIlButton.material = redButtonMaterial;
                    CIrButton.material = redButtonMaterial;
                    CInButton.material = redButtonMaterial;
                break;
            }
    }
}
