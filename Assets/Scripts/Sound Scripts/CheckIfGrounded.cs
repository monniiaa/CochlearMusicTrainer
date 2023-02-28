using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckIfGrounded : MonoBehaviour // See if player stands on ground
{
    public Collider playerCollider;

    public bool isGrounded;
    public bool isOnTerrain;
    public bool isOnSand;
    public bool isOnFloor;

    public RaycastHit hit; // Save data from the raycast that fires from playercontroller


    void Update()
    {
        isGrounded = PlayerGrounded(); // bool whether grounded or not
        isOnSand = CheckIfOnSand();
        isOnFloor = CheckIfOnFloor();
    }

    bool PlayerGrounded(){ // set direction of raycast downwards from origin, return hit data, and set max distance (bounds) for raycast + 5f
        return Physics.Raycast (transform.position, Vector3.down, out hit, 5);
    }

    bool CheckIfOnSand(){ // if collider is active and ray hits object tagged 'sand'
        if (hit.collider != null && hit.collider.tag == "sand")
            return true;
                else return false;
    }

    bool CheckIfOnFloor(){ // if collider is active and ray hits object tagged 'path'
        if (hit.collider != null && hit.collider.tag == "path")
            return true;
                else return false;
    }

    bool CheckIfInside(){ // if collider is active and ray hits object tagged 'path'
        if(SceneManager.GetActiveScene().name == "Inside_Scene")
            return true;
                else return false;
    }


}
