using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject controlsMenu;
    public void startGame()
    {
        SceneManager.LoadScene("nightmare_room");
    }

    public void showControls()
    {
        controlsMenu.SetActive(true);
    }

    public void hideControls()
    {
        controlsMenu.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }


}
