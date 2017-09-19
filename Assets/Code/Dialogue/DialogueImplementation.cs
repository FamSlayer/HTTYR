using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogueImplementation : Singleton<DialogueImplementation>
{
	[HideInInspector]
	public int currentOption;
	Dialogue dialogue;
	public UnityEngine.UI.Text uiText;
	public GameObject[] optionButtons;
    public TextAsset[] Conversations;
	public TextAsset defaultDialogue;
	bool scrolling;

    const float timePerChar = .05f;

    // this is the text that is actually run by the dialogue manager!
    string textToRun = "";

    void Awake()
	{
		dialogue = GetComponent<Dialogue>();

		foreach (var gameObject in optionButtons)
		{
			gameObject.SetActive(false);
		}

        /*
		if (defaultDialogue != null)
		{
			textToRun = defaultDialogue.text;
		}
        */

        textToRun = Conversations[Dialogue.global.conversation_number - 1].text;
	}

	public string Parse(string characterName, string line)
	{
		return line;
	}

	public IEnumerator Say(string characterName, string text)
	{
        string raw_info = Dialogue.global.visitedNodes[Dialogue.global.visitedNodes.Count - 1];
        string[] pieces = raw_info.Split(new char[] { ':' });
        string node_name = pieces[1];
        print("Current node = " + node_name);

        bool interupted = false;
		uiText.text = "";
		string textToScroll = text;
        if(characterName == "")
        {
            textToScroll = text;
        }
        else
        {
            uiText.text = characterName + ": ";
        }
		//CharacterData characterData = Global.constants.GetCharacterData(characterName);
		//Global.textbox.Say(characterData, text);
		float accumTime = 0f;
		int c = 0;

        
        DialogueAudio.global.PlayDialogue(node_name);

		while (!interupted && c < textToScroll.Length)
		{
			yield return null;

            if(Interrupt())
            {
                interupted = true;
                yield return null;
            }
			accumTime += Time.deltaTime;
			while (accumTime > timePerChar)
			{
				accumTime -= timePerChar;
				if (c < textToScroll.Length)
					uiText.text += textToScroll[c];
				c++;
			}
		}

        if(!interupted)
        {
            if (characterName == "")
                uiText.text = textToScroll;
            else
            {
                uiText.text = characterName + ": " + textToScroll;
            }

            // COMPLETED DIALOGUE TEXT, CALL A FUNCTION THAT STORES IT AND STUFF
            print("Completed dialogue text without being interrupted!");
            
            MouseBrain.global.CompletedNode(node_name);

        }
        else
        {
            // END THE CONVERSATION!
            // TELL THE MOUSE BRAIN THAT'S KEEPING TRACK OF ITS FEELINGS TOWARDS YOU HOW IT ENDED!
            print("Whoa we got super interrupted! Quit talking!");
            dialogue.Stop(true);
        }

		while (InputNext()) yield return null;

		//while (!InputNext()) yield return null;
	}

	public bool InputNext()
	{
		return Input.GetMouseButtonDown(0);
	}

    public bool Interrupt()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public void ClearText()
    {
        uiText.text = "";
        print("Cleared UI text!");
    }

	public IEnumerator EndText()
	{
		//Global.textbox.Hide();
		uiText.text = "";
		yield break;
	}

	public void SelectOption00()
	{
		currentOption = 0;
	}

	public void SelectOption01()
	{
		currentOption = 1;
	}

	public void SelectOption02()
	{
		currentOption = 2;
	}

	public void SelectOption03()
	{
		currentOption = 3;
	}

	public IEnumerator RunOptions(List<Dialogue.Option> options)
	{
		dialogue.SetCurrentOption(0);

		yield return null;

		int index = 0;
		foreach (var option in options)
		{
			optionButtons[index].SetActive(true);
			optionButtons[index].GetComponentInChildren<UnityEngine.UI.Text>().text = option.text;
			index++;
		}
		
		/*
		List<OptionButton> optionButtons = new List<OptionButton>();
		int index = 0;
		foreach (var option in options)
		{
			var optionButton = (OptionButton)Instantiate(prefabOptionButton);
			optionButton.index = index;
			optionButton.transform.position = new Vector3(3.375f, 4f, 0f) + Vector3.down * index * 1.5f;
			optionButton.SetText(option.text);
			optionButtons.Add(optionButton);
			index++;
		}
		*/

		currentOption = -1;
		do { yield return null; } while (currentOption == -1);

		//Global.textbox.Say(null, "");

		/*
		for (int i = 0; i < optionButtons.Count; i++)
		{
			if (i != currentOption)
				optionButtons[i].Hide();
		}
		*/

		//yield return new WaitForSeconds(.71f);

		foreach (var gameObject in optionButtons)
		{
			gameObject.SetActive(false);
		}

		dialogue.SetCurrentOption(currentOption);
	}

	public IEnumerator RunCommand(string line)
	{
		string[] tokens = line.Split(' ');
		if (tokens.Length > 0)
		{
			if (IsString(tokens[0], "wait"))
			{
				float timeToWait = (float)Convert.ToDouble(tokens[1]);
				yield return new WaitForSeconds(timeToWait);
			}
			else if (IsString(tokens[0], "tell"))
			{
				GameObject gameObject = GameObject.Find(tokens[1]);
				if (gameObject != null)
				{
					int methodToken = 2;
					if (IsString(tokens[2], "to"))
						methodToken = 3;
					
					string sendData = "";
					if (tokens.Length > methodToken+1)
						sendData = tokens[methodToken+1];
					
					gameObject.SendMessage(tokens[3], sendData, SendMessageOptions.DontRequireReceiver);
				}
			}

		}
		yield break;
	}

	bool ReadBool(string token)
	{
		return IsString(token, "on") || IsString(token, "1");
	}

	bool IsString(string strA, string strB)
	{
		return string.Compare(strA, strB, System.StringComparison.InvariantCultureIgnoreCase) == 0;
	}

	public void SetInteger(string varName, int varValue)
	{
		Continuity.instance.SetVar(varName, varValue);
	}

	public int GetInteger(string varName)
	{
		return Continuity.instance.GetVar(varName);
	}

	public void AddToInteger(string varName, int addAmount)
	{
		Continuity.instance.SetVar(varName, Continuity.instance.GetVar(varName) + addAmount);
	}

	public void SetString(string varName, string varValue)
	{
		// TODO: write this!
	}

	// called when node not found
	public void NodeFail()
	{

	}

	public bool IsPaused()
	{
		return false;
	}

	public bool EvaluateIfChunk(string chunk, ref bool result)
	{
		return false;
	}



    public void BeginDialogue()
    {
        if (dialogue.running)
        {
            print("Dialogue is running!");
            return;

        }

        if (Dialogue.global.conversation_number > Conversations.Length)
        {
            print("We've already gone through all the dialogue in the game!");
            return;
        }


        print("STARTING CONVERSATION NUMBER " + Dialogue.global.conversation_number + "...");
        textToRun = Conversations[Dialogue.global.conversation_number - 1].text;
        Dialogue.global.conversation_number += 1;

        dialogue.Run(textToRun);


    }

    // THIS IS THE ORIGINAL WAY THE FUNCTIONS ARE ALL CALLED!
    /*
	void OnGUI()
	{
		if (!dialogue.running)
		{
			textToRun = GUI.TextArea(new Rect(0, 0, 600, 350), textToRun);
			if (GUI.Button(new Rect(610, 0, 100, 50), "Test Run"))
			{
				dialogue.Run(textToRun);
			}
			if (GUI.Button(new Rect(610, 60, 100, 50), "Clear"))
			{
				textToRun = "";
			}
		}
	}
    */
}
