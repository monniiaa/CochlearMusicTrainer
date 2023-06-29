using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OutsideMapChecker : MonoBehaviour
{
    [SerializeField] private float m_checkIntervalsSeconds = 5;
    [SerializeField] private BoxCollider m_allowedBounds;
    private Dictionary<XRBaseInteractable, Vector3> _interactables = new();
    void OnEnable()
    {
        foreach (var interactable in FindObjectsOfType<XRGrabInteractable>())
        {
            _interactables.Add(interactable, interactable.transform.position);
        }
        StartCoroutine(CheckForOutsideBounds(m_checkIntervalsSeconds));
    }

    private void OnDisable()
    {
        StopCoroutine(CheckForOutsideBounds(m_checkIntervalsSeconds));
        _interactables = new Dictionary<XRBaseInteractable, Vector3>();
    }

    IEnumerator CheckForOutsideBounds(float checkIntervalSeconds)
    {
        WaitForSeconds waitTime = new WaitForSeconds(checkIntervalSeconds);
        while (true)
        {
            foreach (var interactable in _interactables)
            {
                if (!m_allowedBounds.bounds.Contains(interactable.Key.transform.position))
                {
                    interactable.Key.gameObject.transform.position = interactable.Value;
                    interactable.Key.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
            yield return waitTime;
        }
    }
}
