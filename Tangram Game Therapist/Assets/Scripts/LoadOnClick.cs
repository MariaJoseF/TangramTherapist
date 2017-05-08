using UnityEngine;
using System;
using UnityEngine.UI;


public class LoadOnClick : MonoBehaviour {
	
    bool muted = false;

	public Button soundButton;
	public Sprite muteSprite;
	public Sprite soundSprite;
    public Button playButton;
    public InputField inputName;
    

    int previousLevel = 1;
    string playerName;
    bool loadForm = true;

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


        if (loadForm)
        {
            Therapist.Instance.RatingsFeedback.form_Feedback.Show();
            loadForm = false;
        }
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

        Application.LoadLevel(2);
       
    }
}