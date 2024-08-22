using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// title manager
public class TitleManager : MonoBehaviour
{
    // the volume slider.
    public Slider volumeSlider;

    // screen size selector
    public TMPro.TMP_Dropdown screenSizeSel;

    // The exit button.
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        // there should only be one in the scene.
        if (volumeSlider == null)
            volumeSlider = FindObjectOfType<Slider>(true);

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
                    case 1080: // big (1920 X 1080)
                        screenSizeSel.value = 1;
                        break;

                    case 720: // medium (1280 X 720)
                        screenSizeSel.value = 2;
                        break;

                    case 480: // small (1024 X 576)
                        screenSizeSel.value = 3;
                        break;
                }
            }
            
        }

        // If this is the WebGL platform.
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // Disable the size change.
            if (screenSizeSel != null)
                screenSizeSel.interactable = false;

            // Disable the exit button.
            if(exitButton != null)
                exitButton.interactable = false;
        }
        else // Not WebGL.
        {
            // Enable the size change.
            if (screenSizeSel != null)
                screenSizeSel.interactable = true;

            // Enable the exit button.
            if (exitButton != null)
                exitButton.interactable = true;
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
                Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow);
                Screen.fullScreen = false;
                break;

            case 2: // 1280 X 720
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                Screen.fullScreen = false;
                break;

            case 3: // 1024 X 576
                Screen.SetResolution(1024, 576, FullScreenMode.Windowed);
                Screen.fullScreen = false;
                break;
        }
    }


    // passes object
    public void EnableGameObject(GameObject go)
    {
        go.SetActive(true);
    }

    // passes object
    public void DisableGameObject(GameObject go)
    {
        go.SetActive(false);
    }

    // toggles if the game object is active or not.
    public void ToggleGameObjectActive(GameObject go)
    {
        go.SetActive(go.activeSelf);
    }

    // changes volume.
    public void OnVolumeSliderChange()
    {
        if (volumeSlider != null)
            AudioListener.volume = volumeSlider.value;
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
