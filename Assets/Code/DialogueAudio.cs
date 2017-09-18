using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudio : Singleton<DialogueAudio> {

    public List<string> convo1_nodes;
    public List<AudioClip> convo1_clips;
    public List<int> convo1_rewards;

    public List<string> convo2_nodes;
    public List<AudioClip> convo2_clips;
    public List<int> convo2_rewards;

    public List<string> convo3_nodes;
    public List<AudioClip> convo3_clips;
    public List<int> convo3_rewards;

    public List<string> convo4_nodes;
    public List<AudioClip> convo4_clips;
    public List<int> convo4_rewards;


    public static Dictionary<string, AudioClip> convo1_dict;
    public static Dictionary<string, AudioClip> convo2_dict;
    public static Dictionary<string, AudioClip> convo3_dict;
    public static Dictionary<string, AudioClip> convo4_dict;
    

    void Start()
    {
        convo1_dict = new Dictionary<string, AudioClip>();
        convo2_dict = new Dictionary<string, AudioClip>();
        convo3_dict = new Dictionary<string, AudioClip>();
        convo4_dict = new Dictionary<string, AudioClip>();

        // LOAD CONVO1_DICT
        if (LoadConvoDict(convo1_dict, convo1_nodes, convo1_clips))
            print("Loaded convo1_dict.");
        else
            print("FAILED to load convo1_dict.");

        if (LoadConvoDict(convo2_dict, convo2_nodes, convo2_clips))
            print("Loaded convo2_dict.");
        else
            print("FAILED to load convo2_dict.");

        if (LoadConvoDict(convo3_dict, convo3_nodes, convo3_clips))
            print("Loaded convo3_dict.");
        else
            print("FAILED to load convo3_dict.");

        if (LoadConvoDict(convo4_dict, convo4_nodes, convo4_clips))
            print("Loaded convo4_dict.");
        else
            print("FAILED to load convo4_dict.");



    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.D))
        {
            DialogueImplementation.global.BeginDialogue();
        }
	}

    bool LoadConvoDict(Dictionary<string, AudioClip> dict, List<string> names, List<AudioClip> clips)
    {
        print("names.Count = " + names.Count);
        print("clips.Count = " + clips.Count);
        if(names.Count != clips.Count || names.Count == 0 || clips.Count == 0)
        {
            print("List of names and clips are empty or not equal! Quitting...");
            return false;
        }
        for (int i = 0; i < convo1_nodes.Count; i++)
        {
            convo1_dict[convo1_nodes[i]] = convo1_clips[i];
        }
        return true;
    }


    public void InterruptedMouse()
    {
        print("How rude! You interrupted the mouse!");
        MouseBrain.global.AddMouseFriendliness(-2);
    }


}
