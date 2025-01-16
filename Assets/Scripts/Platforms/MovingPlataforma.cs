using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    [Header("Platform Spawn Settings")]
public GameObject[] platforms;          
public GameObject triggerObject;        
private bool isActivated = false;      

void Start()
{
    foreach (var platform in platforms)
    {
        platform.SetActive(false);
    }
}

void Update()
{
    if (isActivated)
    {
        SpawnPlatforms();
    }
}

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        isActivated = true;
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        isActivated = false; 
    }
}

void SpawnPlatforms()
{
    foreach (var platform in platforms)
    {
        if (!platform.activeInHierarchy)
        {
            platform.SetActive(true);
           
        }
    }
}

}
