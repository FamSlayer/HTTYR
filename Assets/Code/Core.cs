using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Singleton<Core> {

    public GameObject mouse;

    public float starting_dial_rotation = 225f;
    MouseMove mm;

    void Awake ()
    {
        mm = mouse.GetComponent<MouseMove>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!mm)
            {
                mm = FindObjectOfType<MouseMove>();
            }
            if(mm.can_be_shocked())
            {
                Sound.global.PlayZap();
                mm.ElectricShockTherapy();
            }
        }
	}

    public void set_dial_rotation(float rotation)
    {
        starting_dial_rotation = rotation;
    }
}
