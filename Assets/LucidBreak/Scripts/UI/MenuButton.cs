using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public enum ButtonType
    {
        StartGame,
        ReturnToMenu,
        QuitGame
    }

    [SerializeField] private ButtonType buttonType;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.StartGame:
                GameManager.instance.StartGame(); 
                break;
            case ButtonType.ReturnToMenu:
                GameManager.instance.ReturnToMenu();
                break;
            case ButtonType.QuitGame:
                GameManager.instance.QuitGame();
                break;
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
} 