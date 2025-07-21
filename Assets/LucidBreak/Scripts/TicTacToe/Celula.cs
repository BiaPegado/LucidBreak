using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Celula : MonoBehaviour
{
    public int x; // posição X da célula no tabuleiro
    public int y; // posição Y da célula no tabuleiro
    public TextMeshProUGUI text; // texto que exibe X ou O

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        GameManager.instance.RegisterCell(this);
    }

    public void OnClick()
    {
        Debug.Log("Clicou");
        GameManager.instance.PlayerMove(x, y);
    }

    public void SetSymbol(string symbol)
    {
        Debug.Log("No setsymbol");
        text.text = symbol;
        button.interactable = false;
    }

    public void ResetCell()
    {
        text.text = "";
        button.interactable = true;
    }
}
