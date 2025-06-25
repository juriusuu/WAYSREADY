using TMPro;
using UnityEngine;

public class CrosswordCell : MonoBehaviour
{
    public TMP_Text letterText;    // Assign in prefab (for the main letter)
    public TMP_Text legendText;    // ✅ New: Assign in prefab (for clue number)

    private char correctChar = '\0';
    private int row;
    private int col;

    public bool IsUsed => correctChar != '\0';
    public bool IsRevealed => !string.IsNullOrEmpty(letterText.text);

    public void Init(int r, int c)
    {
        row = r;
        col = c;
        letterText.text = "";
        correctChar = '\0';
        if (legendText != null) legendText.text = ""; // clear legend on init
    }

    public void SetAnswerChar(char c)
    {
        if (correctChar == '\0')
        {
            correctChar = char.ToUpper(c);
        }
    }

    public string GetAnswerChar()
    {
        return correctChar == '\0' ? "" : correctChar.ToString();
    }

    public void RevealChar()
    {
        if (correctChar != '\0')
        {
            letterText.text = correctChar.ToString();
        }
    }

    public void RevealChar(char c)
    {
        letterText.text = char.ToUpper(c).ToString();
    }

    public void RevealCorrect()
    {
        RevealChar();
    }

    // ✅ New: Set legend number
    public void SetLegendNumber(int number)
    {
        if (legendText != null)
        {
            legendText.text = number.ToString();
        }
    }
}
