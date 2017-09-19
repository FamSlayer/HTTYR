using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeLevelTrigger : MonoBehaviour {
    public string next_level_name;

    public void loadNextLevel()
    {
        SceneManager.LoadScene(next_level_name);
    }
}
