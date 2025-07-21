using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public int finalCounter = 0;

    public Celula[,] cells = new Celula[3, 3];

    private string[,] board = new string[3, 3]; // "X", "O", ou ""
    private bool playerTurn = true;
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

    public void RegisterCell(Celula cell)
    {
        cells[cell.x, cell.y] = cell;
        board[cell.x, cell.y] = "";
    }

    public void PlayerMove(int x, int y)
    {
        if (!playerTurn || board[x, y] != "") return;

        MakeMove(x, y, "X");
        if (CheckGameOver()) return;

        playerTurn = false;
        Invoke(nameof(AIMove), 0.5f);
    }

    void AIMove()
    {
        Vector2Int move = GetBestMove();
        MakeMove(move.x, move.y, "O");
        if (CheckGameOver()) return;
        playerTurn = true;
    }

    void MakeMove(int x, int y, string symbol)
    {
        board[x, y] = symbol;
        cells[x, y].SetSymbol(symbol);
    }

    bool CheckGameOver()
    {
        string winner = GetWinner();
        if (winner != "")
        {
            if (winner == "X")
            {
                finalCounter = 5; 
            }

            Invoke(nameof(RestartGame), 2f);
            return true;
        }

        if (IsBoardFull())
        {
            Invoke(nameof(RestartGame), 2f);
            return true;
        }

        return false;
    }


    string GetWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != "" && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                return board[i, 0];
            if (board[0, i] != "" && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                return board[0, i];
        }

        if (board[0, 0] != "" && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            return board[0, 0];
        if (board[0, 2] != "" && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            return board[0, 2];

        return "";
    }

    bool IsBoardFull()
    {
        foreach (var val in board)
            if (val == "") return false;
        return true;
    }

    void RestartGame()
    {
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
            {
                board[x, y] = "";
                cells[x, y].ResetCell();
            }
        playerTurn = true;
    }

    // IA Minimax
    Vector2Int GetBestMove()
    {
        int bestScore = int.MinValue;
        Vector2Int bestMove = Vector2Int.zero;

        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y] == "")
                {
                    board[x, y] = "O";
                    int score = Minimax(board, 0, false);
                    board[x, y] = "";
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new Vector2Int(x, y);
                    }
                }
            }

        return bestMove;
    }

    int Minimax(string[,] board, int depth, bool isMaximizing)
    {
        string result = GetWinner();
        if (result == "O") return 10 - depth;
        if (result == "X") return depth - 10;
        if (IsBoardFull()) return 0;

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y] == "")
                {
                    board[x, y] = isMaximizing ? "O" : "X";
                    int score = Minimax(board, depth + 1, !isMaximizing);
                    board[x, y] = "";
                    bestScore = isMaximizing ? Mathf.Max(score, bestScore) : Mathf.Min(score, bestScore);
                }
            }

        return bestScore;
    }
}

