using UnityEngine;
using UnityEngine.SceneManagement;

public class SetDefaultResolution : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(0);
        // Verificar si ya se ha guardado el índice de resolución
        if (!PlayerPrefs.HasKey("ResolutionIndex"))
        {
            // Guardar el índice de resolución predeterminado
            PlayerPrefs.SetInt("ResolutionIndex", 0);
            // Cambiar a resolución 1920x1080 en modo pantalla completa
            Screen.SetResolution(1920, 1080, true);
            // Guardar los cambios
            PlayerPrefs.Save();
            Debug.Log("Resolución predeterminada guardada: 1920x1080");
        }
    }
}