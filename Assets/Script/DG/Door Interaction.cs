using System;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject obj;

    private void OnTriggerEnter2D(Collider2D other)
    {
        obj.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        obj.SetActive(false);
    }
}
