using UnityEngine;
using UnityEngine.SceneManagement;

public class SetDefaultResolution : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(0);
        // Verificar si ya se ha guardado el �ndice de resoluci�n
        if (!PlayerPrefs.HasKey("ResolutionIndex"))
        {
            // Guardar el �ndice de resoluci�n predeterminado
            PlayerPrefs.SetInt("ResolutionIndex", 0);
            // Cambiar a resoluci�n 1920x1080 en modo pantalla completa
            Screen.SetResolution(1920, 1080, true);
            // Guardar los cambios
            PlayerPrefs.Save();
            Debug.Log("Resoluci�n predeterminada guardada: 1920x1080");
        }
    }
}