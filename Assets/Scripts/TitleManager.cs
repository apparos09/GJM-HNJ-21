using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// title manager
public class TitleManager : MonoBehaviour
{
    // screen size selector
    public TMPro.TMP_Dropdown screenSizeSel;

    // Start is called before the first frame update
    void Start()
    {
        // sets to current screen size.
        if(screenSizeSel != null)
        {
            // if in full-screen, use option 0.
            if(Screen.fullScreen)
            {
                screenSizeSel.value = 0;
            }
            else // specifics screen size.
            {
                // checks current screen size.
                int screenY = Screen.height;

                // checks screen size to see default value.
                switch (screenY)
                {
                    case 1080: // big
                        screenSizeSel.value = 1;
                        break;

                    case 720: // medium
                        screenSizeSel.value = 2;
                        break;

                    case 480: // small
                        screenSizeSel.value = 3;
                        break;
                }
            }
            
        }
    }

    // called by dropdown.
    public void OnScreenSizeDropdownChange()
    {
        // get screen size from dropdown
        if(screenSizeSel != null)
            ChangeScreenSize(screenSizeSel.value);
    }

    // called when the screen size changes.
    public void ChangeScreenSize(int option)
    {
        switch (option)
        {
            case 0: // Full Screen
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.fullScreen = true;
                break;

            case 1: // 1920 X 1080
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                Screen.fullScreen = false;
                break;

            case 2: // 1280 X 720
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                Screen.fullScreen = false;
                break;

            case 3: // 854 X 480 (854 rounded up from 853.333)
                Screen.SetResolution(854, 480, FullScreenMode.Windowed);
                Screen.fullScreen = false;
                break;
        }
    }

    // plays the game
    public void PlayGame()
    {
        // loads the round scene.
        SceneManager.LoadScene("GameScene");
    }

    // exits the game
    public void ExitApplication()
    {
        Application.Quit();
    }
}
