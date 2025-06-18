using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private bool playerNearby = false;
    private GameObject player;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex == 0 ? 1 : 0;

            // Define onde o player deve aparecer na próxima cena
            if (nextSceneIndex == 1)
            {
                SceneTransition.playerNextPosition = new Vector3(4.447693f, 0.4476953f, 0f);
                SceneTransition.playerNextRotation = Quaternion.identity;
            }
            else
            {
                SceneTransition.playerNextPosition = new Vector3(-3.55105f, -0.6621178f, 0f);
                SceneTransition.playerNextRotation = Quaternion.Euler(0f, -180f, 0f);
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
        }
    }
}
