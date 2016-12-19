using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Assets.Scripts.Exp3;

public class LoadOnClick : MonoBehaviour {
	
    bool muted = false;

	public Button soundButton;
	public Sprite muteSprite;
	public Sprite soundSprite;
    public Button playButton;
    public InputField inputName;

    int previousLevel = 1;
    string playerName;

	void Start() 
	{
        if (AudioListener.pause && (Application.loadedLevelName == "Main" || Application.loadedLevelName == "Start"))
        {
			soundButton.image.sprite = muteSprite;
			muted = true;
		}

	}

    void Update() {
        if (Application.loadedLevelName == "Start" && GameState.Instance.playButtonInteractable)
            playButton.interactable = true;
        else if (Application.loadedLevelName == "Start" && !GameState.Instance.playButtonInteractable)
            playButton.interactable = false;
    }

	public void LoadScene(int level)
	{
        UtterancesManager.Instance.WriteJSON("HOME QUIT after " + (float)(DateTime.Now - GameManager.Instance.beginGameTime).TotalSeconds + " seconds");

        Application.LoadLevel(previousLevel);
        Therapist.Instance.Quit();
	}

    public void LoadStartScene()
    {
        GameManager.Instance.playerName = playerName;
        Application.LoadLevel(1);
        Therapist.Instance.BeginFirstGame();
    }

    public void SavePlayerName()
    {
        playerName = inputName.text;
    }

	public void MuteListener(Button button) 
	{
		muted = !muted;
		if (muted) {
			AudioListener.pause = true;
			AudioListener.volume = 0;
			soundButton.image.sprite = muteSprite;
		}
		else {
			AudioListener.pause = false;
			AudioListener.volume = 1F;
			soundButton.image.sprite = soundSprite;
		}

	}

    public void PlayButton() { 
        GameManager.Instance.BeginGame(muted);
        previousLevel = 1;


        Console.WriteLine("----------------- Call EXP3 algorithm -----------------");


        Exp3 teste = new Exp3();

        //reward [0;1]
        //float[] rewards = { 0.0f, 0.0f, 1.0f, 0.0f, 0.1f, 1.0f, 0.5f, 1.0f, 0.0f };
        //teste.RunExp3(9, rewards, 0.07f, 200);

        Application.LoadLevel(2);
    }
}