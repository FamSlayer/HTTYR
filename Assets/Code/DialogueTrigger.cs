using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public bool activated = false;

    public void Activate()
    {
        activated = true;
        print("Activated Dialogue Trigger!");
        DialogueImplementation.global.BeginDialogue();
        // call the dialogue system to trigger the event
        // the dialogue system will do the animation, text, and audio
    }
}
