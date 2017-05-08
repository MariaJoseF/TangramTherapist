using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets.Scripts.Learning;

public class GameManager : MonoBehaviour {
	private static GameManager instance = null;
	public List<string> puzzles = new List<string>();
	public List<string> usedPuzzles;
	string currentPuzzle;
	public int numberOfGames;
	public int closeTries;
    public bool quit = false;

    float distanceThreshold, averagePlacedTime, previousGameTime;
	SolutionManager.Difficulty difficulty;
	SceneProperties.RotationMode rotationMode;
    public string playerName;
    public DateTime beginGameTime;

	public Dictionary<string, float> piecePlacedTimes = new Dictionary<string, float> ();
	int nMediumTimes;
	int nButtonGames;
	int nSimpleGames;

	public static GameManager Instance {
		get { 
			return instance; 
		}
	}


    /// <summary>
    /// /////////////////
    /// </summary>
    /// 


    public string CurrentPuzzle
    {
        get
        {
            return currentPuzzle;
        }

    }

    public float DistanceThreshold
    {
        get
        {
            return distanceThreshold;
        }

    }

    public string RotationMode_
    {
        get
        {
            return rotationMode.ToString();
        }

    }

    public string Difficulty_
    {
        get
        {
            return difficulty.ToString();
        }

    }


    /// <summary>
    /// ///////////////////
    /// </summary>
    /// 


    void Awake () {
		DontDestroyOnLoad(gameObject);

		//Check if instance already exists
		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject); 
	}

	void Start () {
		numberOfGames = 0;
		nMediumTimes = 0;
		nButtonGames = 0;
        nSimpleGames = 0;
		usedPuzzles = new List<string> ();
	}

	public void BeginGame(bool muted){
		InitializeAll ();

        SceneProperties.Instance.difficulty = difficulty;
        SceneProperties.Instance.puzzle = currentPuzzle;
        SceneProperties.Instance.rotationMode = rotationMode;
        SceneProperties.Instance.distanceThreshold = distanceThreshold;
        beginGameTime = DateTime.Now;

        UtterancesManager.Instance.WriteJSON("-------------------- PLAYER: " + playerName + " PUZZLE: " + currentPuzzle + " DIFICULDADE: " + difficulty.ToString() + " MODO ROTAÇAO: " + rotationMode.ToString() + " THRESHOLD: " + distanceThreshold + " --------------------");
    }

	void InitializeAll(){
		if (puzzles.Count == 0)
			puzzles = ReadJSON.Instance.GetListOfPuzzles ();
		
		if (numberOfGames == 0) {
			currentPuzzle = RandomValue (puzzles);
			distanceThreshold = 4;
			difficulty = SolutionManager.Difficulty.easy;
			rotationMode = SceneProperties.RotationMode.simple;
			usedPuzzles.Add (CurrentPuzzle);
			nSimpleGames++;

		} else {
            //The average time it takes to put a piece in the right place
            averagePlacedTime = piecePlacedTimes.Sum(x => x.Value) / piecePlacedTimes.Count;
            //The time it takes to complete the previous puzzle
            previousGameTime = (float)(DateTime.Now - beginGameTime).TotalSeconds;

            //Already played with all puzzles
            if (usedPuzzles.Count == puzzles.Count)
            {
                List<string> usablePuzzles = puzzles.Where((c, i) => c != CurrentPuzzle).ToList();
                currentPuzzle = RandomValue(usablePuzzles);
            }
            else
            {
                currentPuzzle = RandomValue(puzzles.Except(usedPuzzles).ToList());
                usedPuzzles.Add(CurrentPuzzle);
            }

            if (quit)
                Quit();
            else
            {
                distanceThreshold = DecideThreshold();
                difficulty = DecideDifficulty();
                rotationMode = DecideRotationMode();
            }
		}

        piecePlacedTimes = new Dictionary<string, float>();
		numberOfGames++;
		closeTries = 0;
	}

	float DecideThreshold(){
		float threshold;
        if (closeTries > 20 && DistanceThreshold < 4)
            threshold = DistanceThreshold + 1;
        else if (closeTries <= 15 && DistanceThreshold > 1)
            threshold = DistanceThreshold - 1;
		else
            threshold = DistanceThreshold;

		return threshold;
	}

    public SolutionManager.Difficulty DecideDifficulty()
    {
        SolutionManager.Difficulty currentDifficulty;

        if (difficulty == SolutionManager.Difficulty.easy)
        {
            if (previousGameTime < 80 && nSimpleGames > 1)
            {
                currentDifficulty = SolutionManager.Difficulty.medium;
                nMediumTimes++;
            }
            else
                currentDifficulty = SolutionManager.Difficulty.easy;
        }
        else if (difficulty == SolutionManager.Difficulty.medium)
        {
            if (previousGameTime < 70 || (previousGameTime < 90 && nMediumTimes >= 1)
                || (previousGameTime < 110 && nMediumTimes >= 2))
                currentDifficulty = SolutionManager.Difficulty.hard;
            else if (previousGameTime > 240)
                currentDifficulty = SolutionManager.Difficulty.easy;
            else
            {
                currentDifficulty = SolutionManager.Difficulty.medium;
                nMediumTimes++;
            }
        }
        else
        {
            if (previousGameTime > 240)
            {
                currentDifficulty = SolutionManager.Difficulty.medium;
                nMediumTimes++;
            }
            else
                currentDifficulty = SolutionManager.Difficulty.hard;
        }

        return currentDifficulty;
    }

    SceneProperties.RotationMode DecideRotationMode()
    {
        SceneProperties.RotationMode rotation;

        if (rotationMode == SceneProperties.RotationMode.simple)
        {
            if (averagePlacedTime <= 15)
            {
                rotation = SceneProperties.RotationMode.button;
                Therapist.Instance.firstTimeButton = 0;
                nButtonGames++;
            }
            else
            {
                rotation = SceneProperties.RotationMode.simple;
                nSimpleGames++;
            }
        }
        else if (rotationMode == SceneProperties.RotationMode.button)
        {
            if (averagePlacedTime <= 11 && nButtonGames >= 1)
            {
                rotation = SceneProperties.RotationMode.finger;
                Therapist.Instance.firstTimeFinger = 0;
            }
            else if (averagePlacedTime > 40 && nButtonGames >= 1)
            {
                rotation = SceneProperties.RotationMode.simple;
                nSimpleGames = 0;
            }
            else
            {
                rotation = SceneProperties.RotationMode.button;
                nButtonGames++;
            }
        }
        else
        {
            if (averagePlacedTime > 40)
            {
                rotation = SceneProperties.RotationMode.button;
                Therapist.Instance.firstTimeButton = 0;
                nButtonGames = 0;
            }
            else
                rotation = SceneProperties.RotationMode.finger;
        }
        return rotation;
    }

	/*public SolutionManager.Difficulty DecideDifficulty(){
		SolutionManager.Difficulty currentDifficulty;

		if (difficulty == SolutionManager.Difficulty.easy) {
			if (averagePlacedTime < 20) {
				currentDifficulty = SolutionManager.Difficulty.medium;
				nMediumTimes++;
			} else 				
				currentDifficulty = SolutionManager.Difficulty.easy;
		} else if (difficulty == SolutionManager.Difficulty.medium) {
			if (averagePlacedTime < 15 && nMediumTimes > 2)
				currentDifficulty = SolutionManager.Difficulty.hard;
			else if (averagePlacedTime > 40 && nMediumTimes == 1)
				currentDifficulty = SolutionManager.Difficulty.easy;
			else {
				currentDifficulty = SolutionManager.Difficulty.medium;
				nMediumTimes++;
			}
		} else {
			if (averagePlacedTime > 40){
				currentDifficulty = SolutionManager.Difficulty.medium;
				nMediumTimes++;			
			}
			else
				currentDifficulty = SolutionManager.Difficulty.hard;
		}

		return currentDifficulty;
	}

	SceneProperties.RotationMode DecideRotationMode(){
		SceneProperties.RotationMode rotation;

		if (rotationMode == SceneProperties.RotationMode.simple) {
			if (previousGameTime < 80 && nSimpleGames > 1) {
				rotation = SceneProperties.RotationMode.button;
                Therapist.Instance.firstTimeButton = 0;
				nButtonGames++;
			} else {
				rotation = SceneProperties.RotationMode.simple;
				nSimpleGames++;
			}
		} else if (rotationMode == SceneProperties.RotationMode.button) {
            if (previousGameTime < 70 || (previousGameTime < 90 && nButtonGames > 2)) {
                rotation = SceneProperties.RotationMode.finger;
                Therapist.Instance.firstTimeFinger = 0;
            }
            else if (previousGameTime > 240)
            {
                rotation = SceneProperties.RotationMode.simple;
                nSimpleGames = 0;
            }
            else
            {
                rotation = SceneProperties.RotationMode.button;
                nButtonGames++;
            }
		} else {
			if (previousGameTime > 240){
				rotation = SceneProperties.RotationMode.button;
                Therapist.Instance.firstTimeButton = 0;
				nButtonGames = 0;
			}
			else
				rotation = SceneProperties.RotationMode.finger;
		}
		return rotation;
	}*/

    void Quit() {
        quit = false;
        nMediumTimes = 0;
        nButtonGames = 0;
        nSimpleGames = 0;

        //Threshold
        if (distanceThreshold < 4)
            distanceThreshold += 1;

        //Difficulty
        if (difficulty == SolutionManager.Difficulty.hard) {
		    difficulty = SolutionManager.Difficulty.medium;
			nMediumTimes++;
		} else 				
			difficulty = SolutionManager.Difficulty.easy;

        //Rotation
        if (rotationMode == SceneProperties.RotationMode.finger)
        {
            rotationMode = SceneProperties.RotationMode.button;
            Therapist.Instance.firstTimeButton = 0;
            Therapist.Instance.firstTimeFinger = 0;
            nButtonGames++;
        }
        else
        {
            rotationMode = SceneProperties.RotationMode.simple;
            Therapist.Instance.firstTimeButton = 0;
            nSimpleGames++;
        }
    }

    public void AddPiecePlacedTime(string piece, float seconds)
    {
        if (seconds < 10000)
        {
            piecePlacedTimes[piece] = seconds;
            UtterancesManager.Instance.WriteJSON("PLACED " + piece + " in " + seconds + " seconds");
        }
        else
        {
            UtterancesManager.Instance.WriteJSON("PLACED " + piece);
        }
    }

	public string RandomValue(List<string> list){
		System.Random rand = new System.Random();
		int r = rand.Next(list.Count);
		return list[r];
	}
}
