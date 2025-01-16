using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TriggerChangeScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "InicialMenu";

    [SerializeField]
    private float delay = 3.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChangeSceneWithDelay());
        }
    }

    private IEnumerator ChangeSceneWithDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
