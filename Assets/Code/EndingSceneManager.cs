using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    public GameObject blackscreen;
    public GameObject first;
    public GameObject second;

    public float screen_time_1;
    public float screen_time_2;
    public float black_fade_in_time;
    public float black_fade_out_time;
    public float black_fade_out_time_slow;


    Image black_image;
    bool finished = false;

    void Awake()
    {
        first.SetActive(false);
        second.SetActive(false);
        black_image = blackscreen.GetComponent<Image>();


        float current_time = 2f;

        // Fade into First still
        Invoke("ShowFirst", current_time);// ShowPhones();
        Invoke("FadeIn", current_time);
        current_time += black_fade_in_time + screen_time_1;
        Invoke("FadeOut", current_time);
        current_time += black_fade_out_time;
        Invoke("HideFirst", current_time);

        // Fade into Second still
        Invoke("ShowSecond", current_time);
        Invoke("FadeIn", current_time);
        current_time += black_fade_in_time + screen_time_2;
        Invoke("FadeOut", current_time);
        current_time += black_fade_out_time_slow;
        Invoke("HideSecond", current_time);

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

    void FadeOutSlow()
    {
        black_image.CrossFadeAlpha(0.0f, 0.0f, false);
        black_image.CrossFadeAlpha(1.0f, black_fade_out_time_slow, false);
    }


    void ShowFirst()
    {
        first.SetActive(true);
    }

    void HideFirst()
    {
        first.SetActive(false);
    }

    void ShowSecond()
    {
        second.SetActive(true);
    }

    void HideSecond()
    {
        second.SetActive(false);
        finished = true;
    }



    // Allows jumping around between the ending scenes
    void Update()
    {
        if(finished)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                print("Loading scene \"MainMenu\"");
                SceneManager.LoadScene("MainMenu");
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                print("Loading scene \"EndingElope\"");
                SceneManager.LoadScene("EndingElope");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                print("Loading scene \"EndingEscape\"");
                SceneManager.LoadScene("EndingEscape");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                print("Loading scene \"EndingDeath\"");
                SceneManager.LoadScene("EndingDeath");
            }
        }
    }

}
