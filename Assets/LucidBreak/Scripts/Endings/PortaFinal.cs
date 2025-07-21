using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour
{
    private bool playerIsNear = false;
    private Inventory inventory;

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (inventory == null)
        {
            
            if (player != null)
            {
                inventory = player.GetComponent<Inventory>();
            }
        }

        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            player.SetActive(false);
            MusicManager.Instance.PlayEndingMusic();

            if (GameManager.instance.finalCounter < 2)
            {
                SceneManager.LoadScene("bad_ending_cutscene"); 
            } else {
                SceneManager.LoadScene("good_ending_cutscene");
            }
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
