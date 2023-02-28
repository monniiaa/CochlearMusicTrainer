//======= Copyright (c) ANDALACA Corporation, All rights reserved. ===============
//
// Purpose: Indicates spawn position using a gizmo mesh and box. Arrow can choose color for ease.  
//
//=============================================================================

using UnityEngine;

public class LevelSpawnIndicator : MonoBehaviour
{

    public ArrowColor arrowColor;
    private Vector3 spawnIndicatorSize = new Vector3 (1, 2, 1);
    private Vector3 modifiedCubePosition;
    private Vector3 modifiedArrrowPosition;
    private GameObject arrowGameObject;

     private void OnDrawGizmos()
    {
        modifiedCubePosition = transform.position;
        modifiedCubePosition.y = modifiedCubePosition.y - 1;   
        Gizmos.DrawWireCube(modifiedCubePosition, spawnIndicatorSize);

        arrowGameObject = Resources.Load("Resource Meshes/VR Headset Model") as GameObject;   

        switch (arrowColor)
        {
            case ArrowColor.Red:
                Gizmos.color = Color.red;
            break;

            case ArrowColor.Green:
                Gizmos.color = Color.green;
            break;

            case ArrowColor.Blue:
                Gizmos.color = Color.blue;
            break;

            case ArrowColor.Black:
                Gizmos.color = Color.black;
            break;
        }
        
        modifiedArrrowPosition = transform.position;
        modifiedArrrowPosition.y = modifiedArrrowPosition.y - 0.75f;  
        Gizmos.DrawMesh(arrowGameObject.GetComponent<MeshFilter>().sharedMesh ,modifiedArrrowPosition, transform.rotation);
      
    }

    public enum ArrowColor
    {Red, Blue, Green, Black}


}
