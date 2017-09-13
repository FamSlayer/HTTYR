using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Singleton<Core> {

    public GameObject mouse;


    MouseMove mm;

    void Awake ()
    {
        mm = mouse.GetComponent<MouseMove>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            Sound.global.PlayZap();
            mm.ElectricShockTherapy();
        }
	}
}
