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
                GameManager.Instance.StartGame();
                break;
            case ButtonType.ReturnToMenu:
                GameManager.Instance.ReturnToMenu();
                break;
            case ButtonType.QuitGame:
                GameManager.Instance.QuitGame();
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