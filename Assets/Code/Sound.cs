using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

public class Sound : Singleton<Sound> {

    public AudioClip zap_sound;
    private AudioSource audio_source;

    [Range(.1f,1f)]
    public float pitch_min;
    [Range(.5f,2f)]
    public float pitch_max;

    [Range(.5f,1.5f)]
    public float vol_min;
    [Range(1f, 4f)]
    public float vol_max;

	void Awake ()
    {
        audio_source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayZap ()
    {

        float pitch = Random.Range(pitch_min, pitch_max);
        audio_source.pitch = pitch;

        //float pitch_range = ;

        float pitch_percentage = (pitch - pitch_min) / (pitch_max - pitch_min);

        float pitch_inverse = 1 - pitch_percentage;

        float playVol = vol_min + pitch_inverse * (vol_max - vol_min);

        //print("Pitch_percentage: "  + pitch_percentage + " \tplayVol percentage: " + ( (playVol - vol_min) / (vol_max - vol_min) ) );
        
        audio_source.PlayOneShot(zap_sound, playVol);

    }

}
