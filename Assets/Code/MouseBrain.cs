using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBrain : Singleton<MouseBrain>
{
    public int mouse_friendliness;

    public Dictionary<string, int> Rewards;


    
    void Awake ()
    {
        mouse_friendliness = 0;
        Rewards = new Dictionary<string, int>();

        for(int i=0; i < DialogueAudio.global.convo1_nodes.Count; i++)
        {
            Rewards["convo1:" + DialogueAudio.global.convo1_nodes[i]] = DialogueAudio.global.convo1_rewards[i];
        }
        for (int i = 0; i < DialogueAudio.global.convo2_nodes.Count; i++)
        {
            Rewards["convo2:" + DialogueAudio.global.convo2_nodes[i]] = DialogueAudio.global.convo2_rewards[i];
        }
        for (int i = 0; i < DialogueAudio.global.convo3_nodes.Count; i++)
        {
            Rewards["convo3:" + DialogueAudio.global.convo3_nodes[i]] = DialogueAudio.global.convo3_rewards[i];
        }
        for (int i = 0; i < DialogueAudio.global.convo4_nodes.Count; i++)
        {
            Rewards["convo4:" + DialogueAudio.global.convo4_nodes[i]] = DialogueAudio.global.convo4_rewards[i];
        }
    }
	

	void Update () {
		
	}
    

    public void AddMouseFriendliness(int amt)
    {
        mouse_friendliness += amt;
        print("New friendliness = " + mouse_friendliness);
    }


    public void CompletedNode(string node_name)
    {
        string key = "convo" + (Dialogue.global.conversation_number-1).ToString() + ":" + node_name;
        if(Rewards.ContainsKey(key))
            AddMouseFriendliness(Rewards[key]);
        else
        {
            print("Rewards contain no key for \"" + key + "\"");
        }
    }

}
