using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager> {

    public class Dialogue_Node
    {
        public string message;
        public List<string> player_responses;
        public Dialogue_Node(string txt = "", List<string> list = null)
        {
            message = txt;
            if (list == null)
                player_responses = new List<string>();
            else
                player_responses = list;
        }

        public void Print()
        {
            print("Message: " + message);
            if (player_responses.Count == 0)
                print("No possible responses.");
            else
            {
                print("Possible responses: ");
                for(int i=0; i<player_responses.Count; i++)
                {
                    print("\t[" + i.ToString() + "] - \"" + player_responses[i] + "\"");
                }
            }
        }
    }


    void Awake()
    {
        string opener = "Excuse me, Dr. Scientist, do you have a moment?";
        List<string> responses = new List<string>{ "Uh... Sure.", "...", "What?" };

        Dialogue_Node first = new Dialogue_Node(opener, responses);
        first.Print();

    }



    void Update () {
		
	}
}
