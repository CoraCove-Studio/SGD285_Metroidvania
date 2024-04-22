///////////////////////////////////////////
/////// Script Contributors:
/////// Rachel Huggins
///////
///////
///////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _menuPanels = new();
    [SerializeField] private GameObject _activePanel;

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

    public void OnQuit()
    {
        Application.Quit();
    }
}
