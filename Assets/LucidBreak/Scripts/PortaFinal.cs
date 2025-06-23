using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour
{
    private bool playerIsNear = false;
    private Inventory inventory;

    void Update()
    {
        // Busca o player dinamicamente se ainda n�o tiver encontrado
        if (inventory == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                inventory = player.GetComponent<Inventory>();
            }
        }

        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            // Aqui voc� troca para a pr�xima cena, como "Cena2" ou "BadEnding"
            SceneManager.LoadScene("bad ending"); // <--- troque esse nome pela sua pr�xima cena
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
