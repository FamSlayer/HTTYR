using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeLevelTrigger : MonoBehaviour {

    public string next_level_name;
    public GameObject confetti_prefab;
    public float wait_time = 15.0f;
    public float fade_black_time = 3.0f;
    //public Image back_box;
    public Canvas black_screen;
    Image black_image;

    public void loadNextLevel()
    {
        confetti_prefab.SetActive(true);
        Invoke("startFadeBlack", wait_time - fade_black_time);
        Invoke("startNext", wait_time);
    }

    void startFadeBlack()
    {
        black_screen.gameObject.SetActive(true);
        black_image = black_screen.GetComponentInChildren<Image>();
        black_image.CrossFadeAlpha(0.0f, 0.0f, false);
        black_image.CrossFadeAlpha(1.0f, fade_black_time, false);
    }

    void startNext()
    {
        SceneManager.LoadScene(next_level_name);
    }
}
