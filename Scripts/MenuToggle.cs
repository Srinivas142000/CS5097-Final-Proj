using UnityEngine;

// simple toggle script for showing/hiding a menu
// just call ToggleMenu() to flip visibility -might  remove this as well

public class MenuToggle : MonoBehaviour
{
    bool menuVisible = false;

    public void ToggleMenu()
    {
        menuVisible = !menuVisible;
        gameObject.SetActive(menuVisible);
    }
}
