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


    void Awake()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, min_angle);
        angle = min_angle;

        // dont leave me
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update ()
    {
        if(active)
        {
            print("I am a very active button");
            //Vector3 mousepos = cam.WorldToViewportPoint(Input.mousePosition);
            //mousepos = Input.mousePosition;
            //mousepos = cam.WorldToScreenPoint(Input.mousePosition);
            //print(mousepos);
            //Debug.DrawLine(transform.position, mousepos, Color.red);
            angle += turn_speed * Time.deltaTime;
            angle = Mathf.Clamp(angle, min_angle, max_angle);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
