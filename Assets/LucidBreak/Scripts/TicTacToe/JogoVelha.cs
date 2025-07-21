using UnityEngine;

public class JogoVelha : MonoBehaviour
{
    private bool playerInRange = false;
    public KeyCode interactKey = KeyCode.E;
    public GameObject ticTacToePanel; // painel com o jogo da velha

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            OpenGame();
        }
    }

    private void OpenGame()
    {
        ticTacToePanel.SetActive(true);
        Time.timeScale = 0f; // pausa o jogo principal
    }

    public void AcceptGame()
    {
        ticTacToePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DenyGame()
    {
        ticTacToePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
