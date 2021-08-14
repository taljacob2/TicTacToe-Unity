using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class Player
{
    public Image m_Panel;
    public Text m_Text;
    [FormerlySerializedAs("button")] public Button m_Button;
}

[Serializable]
public class PlayerColor
{
    public Color m_PanelColor;
    public Color m_TextColor;
}

public class GameController : MonoBehaviour
{
    private byte m_MoveCount;
    private string m_PlayerSide;

    [FormerlySerializedAs("buttonList")] public Text[] m_ButtonList;

    [FormerlySerializedAs("gameOverPanel")]
    public GameObject m_GameOverPanel;

    [FormerlySerializedAs("gameOverText")] public Text m_GameOverText;

    [FormerlySerializedAs("restartButton")]
    public GameObject m_RestartButton;

    [FormerlySerializedAs("playerO")] public Player m_PlayerO;

    [FormerlySerializedAs("playerX")] public Player m_PlayerX;

    [FormerlySerializedAs("activePlayerColor")]
    public PlayerColor m_ActivePlayerColor;

    [FormerlySerializedAs("inactivePlayerColor")]
    public PlayerColor m_InactivePlayerColor;
    
    [FormerlySerializedAs("startInfo")] public GameObject m_StartInfo;


    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        m_GameOverPanel.SetActive(false);
        m_MoveCount = 0;
        m_RestartButton.SetActive(false);
        setPlayerColorsInactive();
    }

    private void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < m_ButtonList.Length; i++)
        {
            m_ButtonList[i].GetComponentInParent<GridSpace>()
                .SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return m_PlayerSide;
    }

    public void EndTurn()
    {
        m_MoveCount++;
        if (!isWin())
        {
            checkDraw();
        }
    }

    private void checkDraw()
    {
        if (m_MoveCount < 9)
        {
            changeSides();
            return;
        }

        gameOver("draw");
    }

    private bool isWin()
    {
        bool returnValue = false;
        if (isWinByRows())
        {
            gameOver(m_PlayerSide);
            returnValue = true;
        }
        else if (isWinByColumns())
        {
            gameOver(m_PlayerSide);
            returnValue = true;
        }
        else if (isWinByDiagonals())
        {
            gameOver(m_PlayerSide);
            returnValue = true;
        }

        return returnValue;
    }

    private bool isWinByDiagonals()
    {
        bool returnValue = false;

        if (m_ButtonList[0].text == m_PlayerSide &&
            m_ButtonList[4].text == m_PlayerSide &&
            m_ButtonList[8].text == m_PlayerSide)
        {
            returnValue = true;
        }
        else if (m_ButtonList[2].text == m_PlayerSide &&
                 m_ButtonList[4].text == m_PlayerSide &&
                 m_ButtonList[6].text == m_PlayerSide)
        {
            returnValue = true;
        }

        return returnValue;
    }

    private bool isWinByColumns()
    {
        bool returnValue = false;

        if (m_ButtonList[0].text == m_PlayerSide &&
            m_ButtonList[3].text == m_PlayerSide &&
            m_ButtonList[6].text == m_PlayerSide)
        {
            returnValue = true;
        }
        else if (m_ButtonList[1].text == m_PlayerSide &&
                 m_ButtonList[4].text == m_PlayerSide &&
                 m_ButtonList[7].text == m_PlayerSide)
        {
            returnValue = true;
        }
        else if (m_ButtonList[2].text == m_PlayerSide &&
                 m_ButtonList[5].text == m_PlayerSide &&
                 m_ButtonList[8].text == m_PlayerSide)
        {
            returnValue = true;
        }

        return returnValue;
    }

    private bool isWinByRows()
    {
        bool returnValue = false;

        if (m_ButtonList[0].text == m_PlayerSide &&
            m_ButtonList[1].text == m_PlayerSide &&
            m_ButtonList[2].text == m_PlayerSide)
        {
            returnValue = true;
        }
        else if (m_ButtonList[3].text == m_PlayerSide &&
                 m_ButtonList[4].text == m_PlayerSide &&
                 m_ButtonList[5].text == m_PlayerSide)
        {
            returnValue = true;
        }
        else if (m_ButtonList[6].text == m_PlayerSide &&
                 m_ButtonList[7].text == m_PlayerSide &&
                 m_ButtonList[8].text == m_PlayerSide)
        {
            returnValue = true;
        }

        return returnValue;
    }

    private void gameOver(string i_WinningPlayer)
    {
        setBoardButtonsInteractable(false);

        if (i_WinningPlayer.ToLower() == "draw")
        {
            setGameOverText("It's a Draw!");
            setPlayerColorsInactive();
        }
        else
        {
            setGameOverText(i_WinningPlayer + " Wins!");
        }

        m_RestartButton.SetActive(true);
    }

    private void changeSides()
    {
        // Note: Capital Letters for "X" and "O"
        switchPlayerSide(m_PlayerSide == "X" ? "O" : "X");
    }

    private void setGameOverText(string i_Value)
    {
        m_GameOverPanel.SetActive(true);
        m_GameOverText.text = i_Value;
    }

    public void RestartGame()
    {
        m_MoveCount = 0;
        m_GameOverPanel.SetActive(false);
        setBoardButtonsText("");
        m_RestartButton.SetActive(false);
        setPlayerButtonsInteractable(true);
        setPlayerColorsInactive();
        m_StartInfo.SetActive(true);
    }

    private void setBoardButtonsText(string i_Value)
    {
        for (int i = 0; i < m_ButtonList.Length; i++)
        {
            m_ButtonList[i].text = i_Value;
        }
    }

    private void setBoardButtonsInteractable(bool i_Toggle)
    {
        for (int i = 0; i < m_ButtonList.Length; i++)
        {
            m_ButtonList[i].GetComponentInParent<Button>().interactable =
                i_Toggle;
        }
    }

    private void setPlayerColors(Player o_NewPlayer, Player o_OldPlayer)
    {
        o_NewPlayer.m_Panel.color = m_ActivePlayerColor.m_PanelColor;
        o_NewPlayer.m_Text.color = m_ActivePlayerColor.m_TextColor;
        o_OldPlayer.m_Panel.color = m_InactivePlayerColor.m_PanelColor;
        o_OldPlayer.m_Text.color = m_InactivePlayerColor.m_TextColor;
    }

    public void SetStartingSide(string i_NewPlayerSide)
    {
        switchPlayerSide(i_NewPlayerSide);
        startGame();
    }

    private void switchPlayerSide(string i_NewPlayerSide)
    {
        m_PlayerSide = i_NewPlayerSide;
        if (m_PlayerSide == "X")
        {
            setPlayerColors(m_PlayerX, m_PlayerO);
        }
        else
        {
            setPlayerColors(m_PlayerO, m_PlayerX);
        }
    }

    private void startGame()
    {
        setBoardButtonsInteractable(true);
        setPlayerButtonsInteractable(false);
        m_StartInfo.SetActive(false);
    }

    private void setPlayerButtonsInteractable(bool i_Toggle)
    {
        m_PlayerX.m_Button.interactable = i_Toggle;
        m_PlayerO.m_Button.interactable = i_Toggle;
    }

    private void setPlayerColorsInactive()
    {
        m_PlayerX.m_Panel.color = m_InactivePlayerColor.m_PanelColor;
        m_PlayerX.m_Text.color = m_InactivePlayerColor.m_TextColor;
        m_PlayerO.m_Panel.color = m_InactivePlayerColor.m_PanelColor;
        m_PlayerO.m_Text.color = m_InactivePlayerColor.m_TextColor;
    }
}
