using UnityEngine;

public class DiaryItem : MonoBehaviour
{
    // Verifique esta linha
    public string correctCode = "159";

    // E verifique PRINCIPALMENTE esta linha
    [TextArea(10, 20)]
    public string diaryText = "Este é o segredo do diário...";
}