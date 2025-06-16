using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
        itemManager = this.GetComponent<ItemManager>();
    }

    public void StartGame()
    {
        Debug.Log("Iniciando o jogo...");
        // lógica para iniciar o jogo
    }

    public void ReturnToMenu()
    {
        Debug.Log("Voltando ao menu...");
        // lógica para voltar ao menu
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}
