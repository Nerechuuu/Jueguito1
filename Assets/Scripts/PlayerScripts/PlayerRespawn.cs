using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 respawnPoint;
    private float checkPointPositionX, checkPointPositionY;

    public Transform respawnTransform;

    void Start()
    {
        respawnPoint = respawnTransform.position;
        transform.position = respawnPoint;
    }

    public void ReachedCheckPoint(float x, float y)
    {
        PlayerPrefs.SetFloat("checkPointPositionX", x);
        PlayerPrefs.SetFloat("checkPointPositionY", y);
    }

    public void PlayerDied()
    {
            checkPointPositionX = PlayerPrefs.GetFloat("checkPointPositionX");
            checkPointPositionY = PlayerPrefs.GetFloat("checkPointPositionY");
            transform.position = new Vector2(checkPointPositionX, checkPointPositionY);

    }
}
