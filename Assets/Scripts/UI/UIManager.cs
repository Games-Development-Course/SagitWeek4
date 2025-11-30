// ==================== UIManager.cs ====================
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-100)] // ����� �-UIManager ���� �����
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI messageText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLevel(int level)
    {
        if (levelText != null)
            levelText.text = "Level: " + level;
        ClearMessage();
    }

    public void SetScore(int fallen)
    {
        if (scoreText != null)
            scoreText.text = "Fallen: " + fallen;
    }

    public void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;
    }

    public void ClearMessage()
    {
        ShowMessage("");
    }

    public void ShowWin()
    {
        ShowMessage("YOU WON!");
    }
}
