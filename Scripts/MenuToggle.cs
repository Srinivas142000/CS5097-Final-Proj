using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    private bool isMenuActive = false;

    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        gameObject.SetActive(isMenuActive);
    }
}
