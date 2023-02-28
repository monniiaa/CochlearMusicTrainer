using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTerrainTexture : MonoBehaviour // set coordinates to match between player and terrain ground
{
    public Transform playerTransform;
    public Terrain terrainObject;

    public int posX;
    public int posZ;
    public float[] textureValues;

    void Start()
    {
        terrainObject = Terrain.activeTerrain; //assign objects
        playerTransform = gameObject.transform;
    }

    public void GetTerrainTexture(){
        UpdatePosition ();
        CheckTexture();
    }

    void UpdatePosition() // subtract terrain position from player position, removing offset and give a world position over terrain objects (alpha map position)
    { // then take that new position, divide by width and height of terrain object in world space units.
        Vector3 terrainPosition = playerTransform.position - terrainObject.transform.position; // pos on terrain in world space units
        Vector3 mapPosition = new Vector3(terrainPosition.x / terrainObject.terrainData.size.x, 0, terrainPosition.z / terrainObject.terrainData.size.z);
        // this gives x+z coords in percentages across terrain object (standing in center = 0,5+0,5)

        float xCoord = mapPosition.x * terrainObject.terrainData.alphamapWidth; // multiply by width and height of alpha map
        float zCoord = mapPosition.z * terrainObject.terrainData.alphamapHeight; // gives total number of pixel cells by that modifier
        posX = (int)xCoord; // main vars
        posZ = (int)zCoord; // = coords!
    }

    void CheckTexture(){
        float[,,] splatMap = terrainObject.terrainData.GetAlphamaps(posX,posZ,1,1);
        textureValues[0] = splatMap[0,0,0];
        textureValues[1] = splatMap[0,0,1];
        textureValues[2] = splatMap[0,0,2];
        textureValues[3] = splatMap[0,0,3];
    }
}
