using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private List<GameObject> subMenus; 
   
    private void DeactivateAllMenus()
    {
        mainMenu.SetActive(false);
        foreach (GameObject menu in subMenus)
        {
            menu.SetActive(false);
        }
    }

    public void ShowMenu(GameObject menuToShow)
    {
        DeactivateAllMenus(); 
        menuToShow.SetActive(true); 
    }

    public void ShowMainMenu()
    {
        DeactivateAllMenus(); 
        mainMenu.SetActive(true); 
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Ira Fase1-2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnBackButtonPressed()
    {
        ShowMainMenu(); 
    }
}
