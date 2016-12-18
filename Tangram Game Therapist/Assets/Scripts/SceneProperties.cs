using UnityEngine;
using System.Collections;
using System;

public class SceneProperties : MonoBehaviour {

	private static SceneProperties instance = null;
	public string puzzle;
	public SolutionManager.Difficulty difficulty;
	public ReadJSON json = ReadJSON.Instance;
	public string allData;
	public enum RotationMode {button, finger, simple};	
	public RotationMode rotationMode;
	public float distanceThreshold;

	public static SceneProperties Instance {
		get { 
			return instance; 
		}
	}
	
	void Awake () {
		DontDestroyOnLoad(gameObject);

		//Check if instance already exists
		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject);
	}

	public string Puzzle {
		get {
			return puzzle; 
		}
		set 
		{
			puzzle = value; 
		}
	}

	public SolutionManager.Difficulty Difficulty {
		get {
			return difficulty; 
		}
		set 
		{
			difficulty = value; 
		}
	}

	public string AllData {
		get {
			return allData; 
		}
		set 
		{
			allData = value; 
		}
	}
}
