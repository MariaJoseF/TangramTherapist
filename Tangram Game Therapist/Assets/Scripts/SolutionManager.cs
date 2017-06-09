using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class SolutionManager : MonoBehaviour {

	private static SolutionManager instance = null;
	public List<int> keys = new List<int>();
	public List<PieceSolution> values = new List<PieceSolution>();
	public Dictionary<int,PieceSolution> pieceSolutions = new Dictionary<int,PieceSolution> ();
    public List<Piece> allPieces = new List<Piece>();

	string[] matchIds = {"bigtriangle1", "bigtriangle2", "mediumtriangle", "littletriangle1", "littletriangle2", "trapezoid", "square"};

	public ReadJSON json;
	public enum Difficulty {easy, medium, hard};	
	public Difficulty dif;
	private static SceneProperties scene = SceneProperties.Instance;
	public float distanceThreshold;
    public string puzzleNamept = null;

	public DateTime beginTime = DateTime.Now;
	public GameObject panel;
    public GameObject homeButton;
	public List<Sprite> imageSolutions = new List<Sprite> ();

	public AudioClip tadaSound;
	protected AudioSource source;

    public ParticleSystem fireworks;

	public static SolutionManager Instance {
		get { 
			return instance; 
		}
	}

	void Awake () {
		//Check if instance already exists
		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject); 

		for (int i = 0; i < keys.Count; i++) {
			pieceSolutions[i] = values[i];
		}

		json = ReadJSON.Instance;
		//Reads the puzzle
		pieceSolutions = json.GetPuzzle (scene.Puzzle, pieceSolutions);

		//Chose the difficulty
		dif = scene.Difficulty;

		DrawPuzzle ();
		source = GetComponent<AudioSource> ();

		distanceThreshold = scene.distanceThreshold;

        GameState.Instance.BeginGame(dif, SceneProperties.Instance.puzzle, SceneProperties.Instance.rotationMode, SceneProperties.Instance.distanceThreshold);
        fireworks.enableEmission = false;
    }

	//Finds the closest spot from the drop position
	public PieceSolution FindClosestSolution(Piece piece) {
		Vector3 centerPosition = piece.Position;
		Piece.Type type = piece.PieceType; 
		float rotation = piece.Rotation;
		double distance = 100, minDistance = 100, notFoundDistance = 100;
		KeyValuePair<int, PieceSolution> closestSolution = default(KeyValuePair<int, PieceSolution>);
		PieceSolution closest = null, notFoundPlace = null, incorrectAnglePlace = null;
		bool close = false, notFound = false, incorrectAngle = false;

		foreach(KeyValuePair<int, PieceSolution> kvp in pieceSolutions) {
			if (kvp.Value.pieceType == type) {
				if(kvp.Value.rotation > rotation-1 && kvp.Value.rotation < rotation+1) {
					//Calculate the distance between two points and compares with the minimum distance
					distance = DistanceBetweenPositions(centerPosition, kvp.Value.centerPosition);

					if(distance < minDistance && distance < 0.25 * distanceThreshold){
						closestSolution = kvp;
						minDistance = distance;
						close = true;
					}
					else {
						notFound = true;
						notFoundPlace = kvp.Value;
						notFoundDistance = distance;
					}
				}
				else {
					incorrectAngle = true;
					incorrectAnglePlace = kvp.Value;
				}
			}
		}
		if (close) {
			closest = closestSolution.Value;
			pieceSolutions.Remove (closestSolution.Key);
			return closest;
		} else if (notFound)
			GameState.Instance.NotFoundTheRightSpot(piece, notFoundPlace, notFoundDistance); 
		else if (incorrectAngle)
			GameState.Instance.IncorrectAngle(piece, incorrectAnglePlace);
		return closest;
	}

	//Place the piece in the right spot
	public void PlaceThePiece(GameObject dragPiece, PieceSolution place, double diffSeconds, Piece piece) {
		GameState.Instance.HideClue ();
		dragPiece.transform.position = place.centerPosition;
        GameState.Instance.DisableOnePiece(piece);
		int pieceId = FindMatchId (dragPiece.name);
		GameState.Instance.placedPieces[pieceId] = dragPiece.GetComponent<Piece> ();
		GameState.Instance.notPlacedPieces.Remove(pieceId);
		source.PlayOneShot(piece.GetComponent<Piece>().anchorSound,1F);
        piece.FinalStopCountingTime();
        if (pieceSolutions.Count > 0)
			GameState.Instance.FoundTheRightSpot (piece);
        IsGameFinished();
	}

	//Draws the solution puzzle with the solution sprites
	void DrawPuzzle() {

		foreach(KeyValuePair<int, PieceSolution> kvp  in pieceSolutions) {

			kvp.Value.transform.localPosition = kvp.Value.solutionCenterPosition;
			//Rotate in the y axis (trapezoid)
			kvp.Value.transform.rotation *= Quaternion.AngleAxis(kvp.Value.rotationy, Vector3.up);
			//Rotate in the z axis
			kvp.Value.transform.rotation *= Quaternion.AngleAxis(kvp.Value.rotation, Vector3.forward);

			if(dif == Difficulty.easy)
				kvp.Value.EasyDifSprite();
			else if(dif == Difficulty.medium)
				kvp.Value.MediumDifSprite();
			else kvp.Value.HardDifSprite();

		}

	}

	public void ShowHardClue(float seconds) {
		if (!GameState.Instance.showingHardClue) {
			GameState.Instance.beginHardClue = DateTime.Now;
			GameState.Instance.durationHardClue = seconds;

			foreach (Sprite s in imageSolutions) {
				if (s.name == SceneProperties.Instance.puzzle) {
					GameState.Instance.showingHardClue = true;
					gameObject.GetComponent<SpriteRenderer> ().sprite = s;
					gameObject.GetComponent<SpriteRenderer> ().enabled = true;
                    UtterancesManager.Instance.WriteJSON("SHOW HARD CLUE");
                }
			}
		}
	}

	public bool IsGameFinished() {
		//Checks if all spaces are ocupied by pieces
		if (pieceSolutions.Count == 0) {
			panel.SetActive (!panel.activeSelf);
			source.PlayOneShot (tadaSound, 1F);
            homeButton.SetActive(false);
            UtterancesManager.Instance.WriteJSON("FINISHED in " + (float)(DateTime.Now - GameManager.Instance.beginGameTime).TotalSeconds + " seconds | average time to place piece " + GameManager.Instance.piecePlacedTimes.Sum(x => x.Value) / GameManager.Instance.piecePlacedTimes.Count);
            fireworks.enableEmission = true;
			GameState.Instance.EndGame();
			return true;
		} else
			return false;
	}

	public int FindMatchId (string id) {
		int i;
		for (i = 0; i < matchIds.Length; i++) {
			if (matchIds [i].Equals (id))
				return i;
		}
		i = 7;
		return i;
	}

	public string FindMatchIdName (int id) {
		return matchIds [id];
	}

	public double DistanceBetweenPositions(Vector3 pos1, Vector3 pos2){
		return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));

	}

}