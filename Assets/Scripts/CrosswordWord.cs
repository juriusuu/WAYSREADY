using UnityEngine;

[System.Serializable]
public class CrosswordWord
{
    public string word;            // The word itself
    public string clue;            // The clue text
    public int startRow;           // Starting row
    public int startCol;           // Starting column
    public bool isHorizontal;      // Direction: true = across, false = down

    public int clueNumber;         // ✅ Number used for both UI and grid cell
    public GameObject clueObject;  // ✅ UI object to destroy when solved
}

