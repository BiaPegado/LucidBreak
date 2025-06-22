using UnityEngine;
using TMPro;

public class DiaryUIManager : MonoBehaviour
{
    [Header("Painéis de UI")]
    public GameObject codeInputPanel;
    public GameObject diaryDisplayPanel;

    [Header("Elementos Interativos")]
    public TMP_InputField codeInputField;
    public TextMeshProUGUI diaryContentText;

    [Header("Dados do Item")]
    // Em vez de receber o diário, ele já vai conhecer o prefab
    public DiaryItem diaryPrefab;

    // Esta função agora não precisa de parâmetros
    public void OpenDiaryInteraction()
    {
        // Se não tiver um prefab conectado, não faz nada
        if (diaryPrefab == null)
        {
            Debug.LogError("Prefab do diário não foi conectado no DiaryUIManager!");
            return;
        }

        codeInputPanel.SetActive(true);
        codeInputField.text = "";
    }

    public void OnSubmitCode()
    {
        // Ele usa os dados do prefab que já conhece
        if (codeInputField.text == diaryPrefab.correctCode)
        {
            codeInputPanel.SetActive(false);
            diaryDisplayPanel.SetActive(true);
            diaryContentText.text = diaryPrefab.diaryText;
        }
        else
        {
            Debug.Log("PERSONAGEM DIZ: 'Humm, não parece ser isso...'");
            codeInputField.text = "";
        }
    }

    public void OnCloseDiaryDisplay()
    {
        diaryDisplayPanel.SetActive(false);
    }

    public void OnCancelCodeInput()
    {
        codeInputPanel.SetActive(false);
    }
}