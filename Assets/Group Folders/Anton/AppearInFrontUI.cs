using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AppearInFrontUI : MonoBehaviour
{
    public GameObject movingCanvas;
    
    public Transform playerHead;
    public float distanceFromPlayer = 2;

    private Vector3 relativePosition;

    void Update()
    {
        /*
            if (movingCanvas.activeSelf)
            {
                movingCanvas.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * distanceFromPlayer;
            }
        */

        movingCanvas.transform.LookAt(new Vector3(playerHead.position.x, movingCanvas.transform.position.y, playerHead.position.z));
        movingCanvas.transform.forward *= -1;
    }


    public void OnEnable() 
    {
        movingCanvas.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * distanceFromPlayer;
    }
    //make an on enable function that sets the position of the canvas to the player's head
     



    
}

