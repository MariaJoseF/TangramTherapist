using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimplePiece : Piece {
	
	private SolutionManager sol;

	public Dragable center;

	bool dragPrinted = false;
	
	// Use this for initialization
	void Start () {
		sol = SolutionManager.Instance;
		
		Vector3 upperCorner = new Vector3 (Screen.width, Screen.height, 0.0f);
		Vector3 targetPosition = Camera.main.ScreenToWorldPoint (upperCorner);
		pieceWidth = GetComponent<Renderer> ().bounds.extents.x;
		pieceHeight = GetComponent<Renderer>().bounds.extents.y;
		maxWidth = targetPosition.x - pieceWidth;
		maxHeight = targetPosition.y - pieceHeight;
		
		Destroy(rotatable);
		Destroy (center.gameObject);

		RotatePiece ();
		anchorSound = GetComponent<Piece> ().anchorSound;

		animator = GetComponent<Piece> ().animator;		
		animatorName = GetComponent<Piece> ().animatorName;
	}
	
	
	// Update is called once per frame
	void Update ()
	{
        if (gameObject.GetComponentInParent<Piece>().isLocked)
            isLocked = true;
        else
            isLocked = false;

		//[Hack] To rotate the trapezoid 180º in the y axis
		if (gameObject.name == "trapezoid" && !rotated) {
			gameObject.transform.rotation *= Quaternion.AngleAxis (sol.pieceSolutions [5].rotationy, Vector3.up); //trapezoid
			rotationy = gameObject.transform.eulerAngles.y;
			rotated = true;
		}
		
		#if UNITY_EDITOR
		
		if (Input.GetMouseButtonDown(0)) {
			
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
			if(hit) {
				
				gameObjectTouched = hit.collider.gameObject;
				
				if (gameObject == gameObjectTouched && !isLocked) {
					
					touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
					offset = touchWorldPos - originalPosition;
					draggingMode = true;

					beginMoving = DateTime.Now;
                    UtterancesManager.Instance.WriteJSON("TOUCHED " + gameObject.name);
                }
			}
			
		}
		
		if (Input.GetMouseButton(0)) {
			
			if (draggingMode && gameObject == gameObjectTouched && !isLocked) {
				
				touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
				v3target = touchWorldPos - offset;
				float targetWidth = Mathf.Clamp(v3target.x, -maxWidth, maxWidth);
				float targetHeigth = Mathf.Clamp(v3target.y, -maxHeight, maxHeight);
				gameObjectTouched.transform.position = new Vector3(targetWidth, targetHeigth, -1.0f);

				//if(RelativeDistance(originalPosition, gameObjectTouched.transform.position) > 2 && !dragPrinted){
                if(!dragPrinted){
                    UtterancesManager.Instance.WriteJSON("DRAGGING " + gameObject.name);
                    dragPrinted = true;
					GameState.Instance.StartedMoving((Piece)this);
				}
			}
			
		}
		
		if (Input.GetMouseButtonUp(0)) {
			
			draggingMode = false;
			
			if (gameObject == gameObjectTouched && !isLocked) {
				PieceSolution ps = sol.FindClosestSolution(this);
				DateTime now = DateTime.Now;
				diffSeconds = (now - beginMoving).TotalSeconds;
				
				if (ps != null)
					sol.PlaceThePiece(gameObjectTouched, ps, diffSeconds, this);
				else {
					gameObjectTouched.transform.position = originalPosition;
				}
				dragPrinted = false;
			}
		}
		
		#endif

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);			
			switch (touch.phase) {
				
			case TouchPhase.Began:
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
					
				if (hit) {
						
					gameObjectTouched = hit.collider.gameObject;
						
					if (gameObject == gameObjectTouched && !isLocked) {
							
						touchWorldPos = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0));
						offset = touchWorldPos - originalPosition;
						draggingMode = true;

						beginMoving = DateTime.Now;
                        UtterancesManager.Instance.WriteJSON("TOUCHED " + gameObject.name);
                    }
				}
					
				break;
					
			case TouchPhase.Moved:
				if (draggingMode && gameObject == gameObjectTouched && !isLocked) {
					touchWorldPos = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0));
					v3target = touchWorldPos - offset;
					float targetWidth = Mathf.Clamp (v3target.x, -maxWidth, maxWidth);
					float targetHeigth = Mathf.Clamp (v3target.y, -maxHeight, maxHeight);
					gameObjectTouched.transform.position = new Vector3 (targetWidth, targetHeigth, -1.0f);

					//if (RelativeDistance (originalPosition, gameObjectTouched.transform.position) > 2 && !dragPrinted) {
                    if(!dragPrinted){
                        UtterancesManager.Instance.WriteJSON("DRAGGING " + gameObject.name);
                        dragPrinted = true;
						GameState.Instance.StartedMoving((Piece)this);
					}
				}
					
				break;
					
			case TouchPhase.Ended:
				draggingMode = false;
					
				if (gameObject == gameObjectTouched && !isLocked) {
					PieceSolution ps = sol.FindClosestSolution (this);
					DateTime now = DateTime.Now;
					diffSeconds = (now - beginMoving).TotalSeconds;
						
					if (ps != null)
						sol.PlaceThePiece(gameObjectTouched, ps, diffSeconds, this);
					else {
						gameObjectTouched.transform.position = originalPosition;
					}
					dragPrinted = false;
				}
				
				break;
			}
		}
	}

	void RotatePiece() {

		PieceSolution piece = SolutionManager.Instance.pieceSolutions [SolutionManager.Instance.FindMatchId(gameObject.name)];
		float angle = piece.rotation;
		Quaternion qz;
		float sign = 1f;
		
		//Round the angle value 
		if(angle >= 337.5 || angle < 22.5)
			qz = Quaternion.AngleAxis(0, Vector3.back);
		else if(angle >= 22.5 && angle < 67.5)
			qz = Quaternion.AngleAxis(-45, Vector3.back);
		else if(angle >= 67.5 && angle < 112.5)
			qz = Quaternion.AngleAxis(-90, Vector3.back);
		else if(angle >= 112.5 && angle < 157.5)
			qz = Quaternion.AngleAxis(-135, Vector3.back);
		else if(angle >= 157.5 && angle < 202.5)
			qz = Quaternion.AngleAxis(180, Vector3.back);
		else if(angle >= 202.5 && angle < 247.5)
			qz = Quaternion.AngleAxis(135, Vector3.back);
		else if(angle >= 247.5 && angle < 292.5)
			qz = Quaternion.AngleAxis(90, Vector3.back);
		else
			qz = Quaternion.AngleAxis(45, Vector3.back);

		if ((int)piece.rotationy == 180) //Trapezoid
			sign = -sign;
		qz.z = sign * qz.z;

		//Rotation in the z axis and in the y axis
		Quaternion qy = Quaternion.AngleAxis (rotationy, Vector3.up);
		Quaternion q = qy*qz;
		gameObject.transform.rotation = q;
		rotation = (float)Math.Round((double)angle, 0, MidpointRounding.AwayFromZero);
        
		// [Hack] Changes piece attribute according each piece
		if (this.PieceType == Piece.Type.square) {
			if ((int)rotation % 10 == 0)
				rotation = 0f;
			else
				rotation = 315.0f;
		} 
		else if (this.PieceType == Piece.Type.trapezoid) {
			if ((int)rotation == 45)
				rotation = 225.0f;
			else if((int)rotation == 179)
				rotation = 0f;
			else if ((int)rotation == 135)
				rotation = 315f;
			else if ((int)rotation == 90)
				rotation = 270f;
		}
        this.GetComponent<Piece>().rotation = rotation;
	}
}
