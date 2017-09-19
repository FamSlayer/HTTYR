using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject blackscreen;
    public GameObject yarn_logo;
    public GameObject silence_phones;
    public GameObject main_menu;

    public float screen_time;
    public float black_fade_in_time;
    public float black_fade_out_time;


    Image black_image;
    string next_level_name = "Test_Scene";


    void Awake()
    {
        black_image = blackscreen.GetComponent<Image>();
        yarn_logo.SetActive(false);
        silence_phones.SetActive(false);
        main_menu.SetActive(false);

        float current_time = 2f;

        // Fade into Silence Phones
        Invoke("ShowPhones", current_time);// ShowPhones();
        Invoke("FadeIn", current_time);
        current_time += black_fade_in_time + screen_time;
        Invoke("FadeOut", current_time);
        current_time += black_fade_out_time;
        Invoke("HidePhones", current_time);
        
        // Fade into Yarn Logo
        Invoke("ShowYarn", current_time);
        Invoke("FadeIn", current_time);
        current_time += black_fade_in_time + screen_time;
        Invoke("FadeOut", current_time);
        current_time += black_fade_out_time;
        Invoke("HideYarn", current_time);

        // Fade into Main Menu
        current_time += 2f;
        Invoke("ShowMenu", current_time);
        Invoke("FadeIn", current_time);
        current_time += black_fade_in_time;
        Invoke("HideBlackscreen", current_time);


    }


    public void LoadNextScene()
    {
        //Invoke("startFadeBlack", wait_time - fade_black_time);
        print("Loading the next scene...");
        blackscreen.SetActive(true);
        FadeOut();
        Invoke("Play", black_fade_out_time);
    }

    void Play()
    {
        SceneManager.LoadScene(next_level_name);
    }

    void FadeIn()
    { 
        black_image.CrossFadeAlpha(1.0f, 0.0f, false);
        black_image.CrossFadeAlpha(0.0f, black_fade_in_time, false);
    }
    
    void FadeOut()
    {
        black_image.CrossFadeAlpha(0.0f, 0.0f, false);
        black_image.CrossFadeAlpha(1.0f, black_fade_out_time, false);
    }
    

    void ShowYarn()
    {
        yarn_logo.SetActive(true);
    }

    void HideYarn()
    {
        yarn_logo.SetActive(false);
    }

    void ShowPhones()
    {
        silence_phones.SetActive(true);
    }

    void HidePhones()
    {
        silence_phones.SetActive(false);
    }

    void ShowMenu()
    {
        main_menu.SetActive(true);
    }

    void HideBlackscreen()
    {
        blackscreen.SetActive(false);
    }
}
