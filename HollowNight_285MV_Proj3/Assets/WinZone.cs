///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// 
///////////////////////

using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] private MainMenuManager menuManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER))
        {
            menuManager.DisplayWinMessage();
        }
    }
}
