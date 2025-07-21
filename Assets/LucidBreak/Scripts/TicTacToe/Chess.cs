using UnityEngine;

public class TicTacToe : MonoBehaviour
{

    private bool playerInRange = false;
    public KeyCode interactKey = KeyCode.E;
    public GameObject question;
    public GameObject tictactoe;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            makeDeal();
        }
    }

    private void makeDeal()
    {
        question.SetActive(true);
    }

    public void accept()
    {
        question.SetActive(false);
        tictactoe.SetActive(true);

    }

    public void deny()
    {
        question.SetActive(false);
    }

    public void closeTicTacToe()
    {
        tictactoe.SetActive(false);
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
