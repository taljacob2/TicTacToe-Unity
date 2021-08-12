using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;

    private string playerSide;

    private byte moveCount;

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        gameOverPanel.SetActive(false);
        moveCount = 0;
    }

    private void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>()
                .SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        checkWin();
        changeSides();
        checkDraw();
    }

    private void checkDraw()
    {
        if (moveCount < 9)
        {
            return;
        }

        SetGameOverText("It's a draw!");
    }

    private void checkWin()
    {
        checkWinByRows();
        checkWinByColumns();
        checkWinByDiagonals();
    }

    private void checkWinByDiagonals()
    {
        if (buttonList[0].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[8].text == playerSide)
        {
            gameOver();
        }

        if (buttonList[2].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[6].text == playerSide)
        {
            gameOver();
        }
    }

    private void checkWinByColumns()
    {
        if (buttonList[0].text == playerSide &&
            buttonList[3].text == playerSide &&
            buttonList[6].text == playerSide)
        {
            gameOver();
        }

        if (buttonList[1].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[7].text == playerSide)
        {
            gameOver();
        }

        if (buttonList[2].text == playerSide &&
            buttonList[5].text == playerSide &&
            buttonList[8].text == playerSide)
        {
            gameOver();
        }
    }

    private void checkWinByRows()
    {
        if (buttonList[0].text == playerSide &&
            buttonList[1].text == playerSide &&
            buttonList[2].text == playerSide)
        {
            gameOver();
        }

        if (buttonList[3].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[5].text == playerSide)
        {
            gameOver();
        }

        if (buttonList[6].text == playerSide &&
            buttonList[7].text == playerSide &&
            buttonList[8].text == playerSide)
        {
            gameOver();
        }
    }

    private void gameOver()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }

        SetGameOverText(playerSide + " Wins!");
    }

    private void changeSides()
    {
        playerSide =
            playerSide == "X"
                ? "O"
                : "X"; // Note: Capital Letters for "X" and "O"
        moveCount++;
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }
}
