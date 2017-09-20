using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialTurner : MonoBehaviour
{
    public float turn_speed;
    bool active = false;
    float min_angle = 45f;
    float max_angle = 225f;
    float angle;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        //transform.rotation = Quaternion.Euler(0f, 0f, min_angle);
        //angle = min_angle;
        angle = Core.global.starting_dial_rotation;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    /*
    private void OnDestroy()
    {
        Core.global.set_dial_rotation(angle);
    }
    */

    void Update ()
    {
        //print("dial update??");
        if(active)
        {
            //print("I am a very active button");
            //Vector3 mousepos = cam.WorldToViewportPoint(Input.mousePosition);
            //mousepos = Input.mousePosition;
            //mousepos = cam.WorldToScreenPoint(Input.mousePosition);
            //print(mousepos);
            //Debug.DrawLine(transform.position, mousepos, Color.red);
            angle += turn_speed * Time.deltaTime;

            if (angle < max_angle && !source.isPlaying)
            {
                source.Play();
            }

            angle = Mathf.Clamp(angle, min_angle, max_angle);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            Core.global.set_dial_rotation(transform.rotation.eulerAngles.z);
            
        }
        else
        {
            if(source.isPlaying)
            {
                source.Stop();
            }
        }
		
	}

    
    public void Activate()
    {
        active = true;
        print("Activated the button!");
    }

    public void DeActivate()
    {
        active = false;
        print("DEACTIVATED the button");
    }
}
