using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Singleton<Core> {


    
    void Awake ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            Sound.global.PlayZap();
        }
	}
}
