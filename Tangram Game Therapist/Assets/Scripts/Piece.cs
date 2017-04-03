using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour {

	protected GameObject gameObjectTouched;
	public AudioClip anchorSound;
	protected AudioSource source;
	protected bool draggingMode = false;
	protected Vector3 offset; //vector between touchpoint to object position
	protected Vector3 v3target; //new position of dragobject without restriction
	protected Vector3 touchWorldPos; //touch position
	protected RaycastHit hit;

	protected ScreenOrientation previousOrientation;
	protected float maxWidth;
	protected float maxHeight;
	protected float pieceWidth;
	protected float pieceHeight;

	public enum Type {bigTriangle, mediumTriangle, littleTriangle, square, trapezoid};
	public Type type;

	public bool isLocked = false;
	
	public float rotation;
	public float rotationy;
	protected bool rotated = false;
	public Rotatable rotatable;
	protected bool isRotatableOn = false;
	
	protected DateTime beginMoving;
	protected Double diffSeconds;

	public Animator animator;
	public string animatorName;
	public Vector3 originalPosition;

    public DateTime movingTime;
    public float secondsMoving;

	// Use this for initialization
	void Awake () {


		if (SceneProperties.Instance.rotationMode == SceneProperties.RotationMode.finger)
			EnableTouchMode ();
		else if (SceneProperties.Instance.rotationMode == SceneProperties.RotationMode.button)
			EnableButtonMode ();
		else
			EnableSimpleMode ();

		previousOrientation = Screen.orientation;
		if(Screen.orientation == ScreenOrientation.Portrait)
			Screen.orientation = ScreenOrientation.LandscapeRight;

		HideRotatable ();
		source = GetComponent<AudioSource> ();

		originalPosition = gameObject.transform.position;
        secondsMoving = 0;
	}

	void Start(){
        if (this.isActiveAndEnabled)
        {
            GameState.Instance.InitializeNotPlacedPieces(this.GetComponentInParent<Piece>());
        }
    }
	
	// Update is called once per frame
	void Update () {

		//To set the tablet right landscape 
		if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.LandscapeRight)
			Screen.orientation = ScreenOrientation.LandscapeRight;
		else
			Screen.orientation = ScreenOrientation.LandscapeLeft;

		if (previousOrientation != Screen.orientation) {
			Vector3 upperCorner = new Vector3 (Screen.width, Screen.height, 0.0f);
			Vector3 targetPosition = Camera.main.ScreenToWorldPoint (upperCorner);
			pieceWidth = GetComponent<Renderer> ().bounds.extents.x;
			pieceHeight = GetComponent<Renderer>().bounds.extents.y;
			maxWidth = targetPosition.x - pieceWidth;
			maxHeight = targetPosition.y - pieceHeight;
		}
	}

	public void EnableTouchMode() {
		gameObject.GetComponent<TouchPiece> ().enabled = true;
	}

	public void EnableButtonMode() {
		gameObject.GetComponent<ButtonPiece> ().enabled = true;
	}

	public void EnableSimpleMode() {
		gameObject.GetComponent<SimplePiece> ().enabled = true;
	}

	public void HideRotatable () {
		if (rotatable) {
			rotatable.Hide ();
			rotatable.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			isRotatableOn = false;
		}
	}

	public double RelativeDistance(Vector3 initialPosition, Vector3 actualPosition) {
		return Mathf.Sqrt(Mathf.Pow(initialPosition.x - actualPosition.x, 2) + Mathf.Pow(initialPosition.y - actualPosition.y, 2));
		
	}

    public void StartCountingTime()
    {
        movingTime = DateTime.Now;
        print("STARTED MOVING " + this.name + " " + secondsMoving);
    }

    public void StopCountingTime()
    {
        secondsMoving += (float)(DateTime.Now - movingTime).TotalSeconds;
        print("STOP MOVING " + this.name + " " + secondsMoving);
    }

    public void FinalStopCountingTime()
    {
        secondsMoving += (float)(DateTime.Now - movingTime).TotalSeconds;
        GameManager.Instance.AddPiecePlacedTime(this.name, secondsMoving);
        print("Final STOP MOVING " + this.name + " " + secondsMoving);
    }

	public Type PieceType {
		get {
			return type; 
		}
	} 
	
	public float Rotation {
		get {
			return rotation; 
		}
		set 
		{
			rotation = value; 
		}
	}
	
	public Vector3 Position {
		get {
			return gameObjectTouched.transform.position; 
		}
	}
}
