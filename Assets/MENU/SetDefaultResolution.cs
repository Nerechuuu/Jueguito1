using UnityEngine;

public class SetDefaultResolution : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("ResolutionIndex"))
        {
            PlayerPrefs.SetInt("ResolutionIndex", 0); // Guardar índice de resolución predeterminada
            Screen.SetResolution(1920, 1080, true);  // Cambiar a true para pantalla completa
            PlayerPrefs.Save();
            Debug.Log("Resolución predeterminada guardada: 1920x1080");
        }
    }
}