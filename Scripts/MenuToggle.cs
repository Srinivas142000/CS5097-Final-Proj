using UnityEngine;

// Menu Toggle script to manage the visibility of a menu in Unity. This script toggles the active state of the GameObject it is attached to when the ToggleMenu method is called. It can be used to show or hide menus in a game, such as an options menu or inventory screen.

public class MenuToggle : MonoBehaviour
{
    private bool isMenuActive = false;

    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        gameObject.SetActive(isMenuActive);
    }
}
