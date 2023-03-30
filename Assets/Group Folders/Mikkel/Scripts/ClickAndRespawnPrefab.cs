using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ClickAndRespawnPrefab : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public float scaleFactor = 0.5f; // The scale factor for the new prefab

    private GameObject spawnedPrefab; // The spawned instance of the prefab

    [SerializeField]
    private ActionBasedController controller;

    private void OnEnable()
    {
        controller.selectAction.action.performed += Shoot;
    }

    void Start()
    {
        // Spawn the initial prefab
        spawnedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }

    private void Shoot(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if mouse click hits the spawned prefab
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == spawnedPrefab)
        {
            // Destroy the spawned prefab
            Destroy(spawnedPrefab);

            // Spawn a new, smaller prefab
            spawnedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            spawnedPrefab.transform.localScale = spawnedPrefab.transform.localScale * scaleFactor;
        }
    }
}
