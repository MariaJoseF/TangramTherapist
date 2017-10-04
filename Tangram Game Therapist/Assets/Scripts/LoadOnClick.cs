using UnityEngine;
using System.Collections;
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

	void Start() 
	{
        if (AudioListener.pause && (Application.loadedLevelName == "Main" || Application.loadedLevelName == "Start"))
        {
			soundButton.image.sprite = muteSprite;
			muted = true;
		}

        if (Application.loadedLevelName == "Name")
        {
            Therapist.Instance.ratingsFeedback.form_Feedback.Show();
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

        /*NEW*/

        if (Therapist.Instance.ratingsFeedback.feedback_val == -2.0f)
        {
            Therapist.Instance.ratingsFeedback.FileHeader();
            Therapist.Instance.ratingsFeedback.WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + Therapist.Instance.AlgorithmEXP3.Action + ";" + "-;home quit");
        }

        Therapist.Instance.SetPrompts("");

        /*NEW*/

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