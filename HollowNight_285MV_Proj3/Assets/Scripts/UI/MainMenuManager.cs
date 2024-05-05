///////////////////////////////////////////
/////// Script Contributors:
/////// Rachel Huggins
/////// Zeb Baukhagen
///////
///////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _menuPanels = new();
    [SerializeField] private GameObject _activePanel;
    [SerializeField] private TextMeshProUGUI textObject;
    private readonly string winMessage = "You have prevailed!";
    private readonly string lossMessage = "You have expired.";

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickMenuButton(int covetedPanelIndex)
    {
        _activePanel.SetActive(false);
        _menuPanels[covetedPanelIndex].SetActive(true);
        _activePanel = _menuPanels[covetedPanelIndex];
    }

    public void OnClickMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void DisplayWinMessage()
    {
        textObject.text = winMessage;
        _activePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisplayLossMessage()
    {
        textObject.text = lossMessage;
        _activePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
