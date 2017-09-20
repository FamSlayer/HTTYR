using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudio : Singleton<DialogueAudio> {

    public List<AudioClip> mouse_squeaks;
    public List<AudioClip> personified_squeaks;
    public List<AudioClip> interrupt_mad;
    public List<AudioClip> interrupt_fine;

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


    public static Dictionary<string, AudioClip> convo_dict;
    
    AudioSource audio_source;

    public bool dialogue_playing = false;

    void Start()
    {
        convo_dict = new Dictionary<string, AudioClip>();

        // LOAD CONVO1_DICT
        if (LoadConvoDict(convo1_nodes, convo1_clips, 1)) ;
        //print("Loaded convo1_dict.");
        else
        //print("FAILED to load convo1_dict.");

        if (LoadConvoDict(convo2_nodes, convo2_clips, 2)) ;
        //print("Loaded convo2_dict.");
        else
        //print("FAILED to load convo2_dict.");

        if (LoadConvoDict(convo3_nodes, convo3_clips, 3)) ;
        //print("Loaded convo3_dict.");
        else
        //print("FAILED to load convo3_dict.");

        if (LoadConvoDict(convo4_nodes, convo4_clips, 4)) ;
        //print("Loaded convo4_dict.");
        else
            //print("FAILED to load convo4_dict.");

            audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.D))
        {
            DialogueImplementation.global.BeginDialogue();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySqueak();
        }



        if (dialogue_playing)
        {
            if(!audio_source.isPlaying)
            {
                //print("Audio source is done playing");
                dialogue_playing = false;
            }
        }
	}

    bool LoadConvoDict(List<string> names, List<AudioClip> clips, int convo_num)
    {
        //print("names.Count = " + names.Count);
        //print("clips.Count = " + clips.Count);
        if(names.Count != clips.Count || names.Count == 0 || clips.Count == 0)
        {
            print("List of names and clips are empty or not equal! Quitting...");
            return false;
        }
        for (int i = 0; i < names.Count; i++)
        {
            string key = "convo" + convo_num.ToString() + ":" + names[i];
            /*
            if (clips[i] != null)
            {
                print("Neat! key [" + key + "] is not null!");
            }
            */
            convo_dict[key] = clips[i];
        }
        return true;
    }


    public void PlayDialogue(string node_name)
    {
        string key = "convo" + (Dialogue.global.conversation_number - 1).ToString() + ":" + node_name;
        /*
        if (Dialogue.global.conversation_number == 1)
            key = "convo" + (Dialogue.global.conversation_number).ToString() + ":" + node_name;
        */
        //print("key = " + key);
        audio_source.clip = convo_dict[key];
        audio_source.Play();
        dialogue_playing = true;
        
    }


    public void PlaySqueak()
    {
        if(Dialogue.global.conversation_number == 1)
            audio_source.clip = mouse_squeaks[Random.Range(0, mouse_squeaks.Count)];
        else
            audio_source.clip = personified_squeaks[Random.Range(0, personified_squeaks.Count)];

        audio_source.Play();
        dialogue_playing = true;
    }


    void PlayInterrupted(bool angry = false)
    {
        if(angry)
            audio_source.clip = interrupt_mad[Random.Range(0, interrupt_mad.Count)];
        else
            audio_source.clip = interrupt_fine[Random.Range(0, interrupt_fine.Count)];
        audio_source.Play();
        dialogue_playing = true;
    }

    public void StopDialogue()
    {
        audio_source.Stop();
    }


    public void InterruptedMouse()
    {
        //print("How rude! You interrupted the mouse!");
        StopDialogue();
        MouseBrain.global.AddMouseFriendliness(-2);

        PlayInterrupted(MouseBrain.global.mouse_friendliness <= -4);

    }



}
