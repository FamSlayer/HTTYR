using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEnding : Singleton<SignalEnding> {

    public ChangeLevelTrigger trigger;
	

    public void SetEnding(string ending_level)
    {
        trigger.next_level_name = ending_level;
    }


}
