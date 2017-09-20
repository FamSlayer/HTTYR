using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMe : MonoBehaviour {

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    

    public void PlayThis()
    {
        source.Play();
    }
}
