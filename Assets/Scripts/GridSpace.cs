using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    [FormerlySerializedAs("button")] public Button m_Button;
    [FormerlySerializedAs("buttonText")] public Text m_ButtonText;

    private GameController m_GameController;

    public void SetGameControllerReference(GameController i_Controller)
    {
        m_GameController = i_Controller;
    }

    public void SetSpace()
    {
        m_ButtonText.text = m_GameController.GetPlayerSide();
        m_Button.interactable = false;
        m_GameController.EndTurn();
    }
}
