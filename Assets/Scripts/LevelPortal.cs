//======= Copyright (c) ANDALACA Corporation, All rights reserved. ===============
//
// Purpose: Manages level and direction of portal. 
//
//=============================================================================

using UnityEngine;

public class LevelPortal : MonoBehaviour
{

    [Tooltip("Choose portal orientation.")]
    public PortalType portalType;

    private float initialYrotation;
    private float rotationDifference;
    private bool doorOpened = false;
    private Animation anim;

    private void Start()
    {
        initialYrotation = gameObject.transform.eulerAngles.y;
        anim = gameObject.GetComponent<Animation>();
    }

    public void DoorOpened() 
    {
        if(!doorOpened)
        {
            switch (portalType)
            {
                case LevelPortal.PortalType.ToOutside:
                    StartCoroutine(GameManager.Instance.LoadScene("Outside_Scene"));
                break;

                case LevelPortal.PortalType.ToInside:
                    StartCoroutine(GameManager.Instance.LoadScene("Inside_Scene"));
                break;
            }
            doorOpened = true;
        }
    }

    public void PlayDoorAnimation()
    {
        anim.Play();
    }
    
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {   
        Gizmos.DrawWireCube(gameObject.transform.position, gameObject.transform.lossyScale);
    }

    public enum PortalType
    {ToOutside, ToInside}

}
