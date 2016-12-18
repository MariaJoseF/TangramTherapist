using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class TouchPiece : Piece {

	private SolutionManager sol;

	private bool rotatingMode = false;
	bool printedRotating = false;

	private float previousAngle;
	private float direction = 0; //1:right -1:left
	private float prevAngle;
	DateTime beginRotating;
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
			gameObject.transform.rotation = Quaternion.AngleAxis (sol.pieceSolutions [5].rotationy, Vector3.up); //trapezoid
			rotationy = gameObject.transform.eulerAngles.y;
			rotated = true;
		}

		#if UNITY_EDITOR

		if (Input.GetMouseButtonDown(0)) {

			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
			if(hit) {
				gameObjectTouched = hit.collider.gameObject;

				if (gameObject == gameObjectTouched && !isLocked && !draggingMode) {

					touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
					rotatingMode = true;
					previousAngle = transform.eulerAngles.z;
                    UtterancesManager.Instance.WriteJSON("TOUCHED " + gameObject.name);

				}
			}

		}

		if (Input.GetMouseButton(0)) {

			if (rotatingMode && gameObject == gameObjectTouched && !isLocked && !draggingMode) {
				Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
				Vector3 orig = touchWorldPos - transform.position;
				Vector3 actual = currentPosition - transform.position;
				orig.z = 0;
				actual.z = 0;
				
				Vector3 normal = Vector3.Cross(Vector3.forward, orig);
				float sign = Mathf.Sign(Vector3.Dot(actual, normal));
				if ((int)rotationy == 180) //Trapezoid
					sign = -sign;

				float angle = Vector3.Angle(actual, orig);
				Vector3 prevEuler = transform.eulerAngles;
				prevAngle = prevEuler.z;
				prevEuler.z = (sign*angle)+previousAngle;
				transform.eulerAngles = prevEuler;
				if(!printedRotating)
					beginRotating = DateTime.Now;

					//[DB] To know the direction of the rotation
				if(Math.Round(prevAngle) >= 3 && Math.Round(prevAngle) <= 357) { 
					if(!printedRotating){
                        UtterancesManager.Instance.WriteJSON("ROTATING " + gameObject.name);
                        printedRotating = true;
					}
						
					if(Math.Round(prevAngle) > Math.Round(transform.eulerAngles.z)) {
						if(direction < 0)
                            UtterancesManager.Instance.WriteJSON("CHANGING DIRECTION " + gameObjectTouched.name + " Right");
                        direction = 1;
					}
					else if (Math.Round(prevAngle) < Math.Round(transform.eulerAngles.z)) {
						if(direction > 0)
                            UtterancesManager.Instance.WriteJSON("CHANGING DIRECTION " + gameObjectTouched.name + " Left");
                        direction = -1;
					}
				}
			}			 
		}
		
		if (Input.GetMouseButtonUp(0)) {
			if (rotatingMode && gameObject == gameObjectTouched && !isLocked && !draggingMode){
				RotatePiece();
				direction = 0;
				printedRotating = false;
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

					if (gameObject == gameObjectTouched && !isLocked && !draggingMode) {
						touchWorldPos = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0));
						rotatingMode = true;
						previousAngle = transform.eulerAngles.z;
                        UtterancesManager.Instance.WriteJSON("TOUCHED " + gameObject.name);
                    }
				}

				break;

			case TouchPhase.Moved:
				if (rotatingMode && gameObject == gameObjectTouched && !isLocked && !draggingMode) {
					Vector3 currentPosition = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0));
					//Vector3 currentPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
					Vector3 orig = touchWorldPos - transform.position;
					Vector3 actual = currentPosition - transform.position;
					orig.z = 0;
					actual.z = 0;
					
					Vector3 normal = Vector3.Cross (Vector3.forward, orig);
					float sign = Mathf.Sign (Vector3.Dot (actual, normal));
					if ((int)rotationy == 180) //Trapezoid
						sign = -sign;

					float angle = Vector3.Angle (actual, orig);
					
					Vector3 prevEuler = transform.eulerAngles;
					prevAngle = prevEuler.z;
					prevEuler.z = (sign * angle) + previousAngle;
					transform.eulerAngles = prevEuler;
					if (!printedRotating)
						beginRotating = DateTime.Now;

					//[DB] To know the direction of the rotation
					if (Math.Round (prevAngle) >= 3 && Math.Round (prevAngle) <= 357) { 
						if (!printedRotating) {
                            UtterancesManager.Instance.WriteJSON("ROTATING " + gameObject.name);
                            printedRotating = true;
						}

						if (Math.Round (prevAngle) > Math.Round (transform.eulerAngles.z)) {
							if (direction < 0)
                                UtterancesManager.Instance.WriteJSON("CHANGING DIRECTION" + gameObjectTouched.name + " Right");
                            direction = 1;
						} else if (Math.Round (prevAngle) < Math.Round (transform.eulerAngles.z)) {
							if (direction > 0)
                                UtterancesManager.Instance.WriteJSON("CHANGING DIRECTION " + gameObjectTouched.name + " Left");
                            direction = -1;
						}
					}
				}

				break;

			case TouchPhase.Ended:
				if (rotatingMode && gameObject == gameObjectTouched && !isLocked && !draggingMode) {
					RotatePiece ();
					direction = 0;
					printedRotating = false;
				}

				break;
			}
		}
	}

	void RotatePiece() {
		
		float angle = gameObjectTouched.transform.eulerAngles.z;
		Quaternion qz;

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

		//Rotation in the z axis and in the y axis
		Quaternion qy = Quaternion.AngleAxis (rotationy, Vector3.up);
		Quaternion q = qy*qz;
		gameObjectTouched.transform.rotation = q;
		rotation = (float)Math.Round((double)gameObjectTouched.transform.localEulerAngles.z, 0, MidpointRounding.AwayFromZero);

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
		DateTime now = DateTime.Now;
		diffSeconds = (now - beginRotating).TotalSeconds;
		if (diffSeconds > 0.3) {
			int pieceRotation = (int)rotation;
            UtterancesManager.Instance.WriteJSON("ROTATED " + gameObjectTouched.name + " DIRECTION " + (direction == 1 ? "Right " : "Left ") + pieceRotation.ToString());
        }
        GameState.Instance.RotatedThePiece((Piece)this);
		rotatingMode = false;
		gameObjectTouched = null;
	}

	public void DragPiece(GameObject gameObjectToDrag) {
		if (gameObject == gameObjectToDrag && !isLocked) {
			
			touchWorldPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			offset = touchWorldPos - originalPosition;
			
			draggingMode = true;
			beginMoving = DateTime.Now;
		}
	}

	public void ContinueDragging(GameObject gameObjectToDrag) {
		if (gameObject == gameObjectToDrag && draggingMode && !isLocked) {
			
			touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			v3target = touchWorldPos - offset;
			float targetWidth = Mathf.Clamp(v3target.x, -maxWidth, maxWidth);
			float targetHeigth = Mathf.Clamp(v3target.y, -maxHeight, maxHeight);
			gameObjectToDrag.transform.position = new Vector3(targetWidth, targetHeigth, -1.0f);

			//if(RelativeDistance(originalPosition, gameObjectToDrag.transform.position) > 1 && !dragPrinted){
            if(!dragPrinted){
                UtterancesManager.Instance.WriteJSON("DRAGGING " + gameObject.name);
                dragPrinted = true;
				GameState.Instance.StartedMoving((Piece)this);
			}
		}
	}

	public void StopDragging(GameObject gameObjectToDrag) {
	
		if (gameObject == gameObjectToDrag && draggingMode && !isLocked) {
			draggingMode = false;

			PieceSolution ps = null;
			if(RelativeDistance(originalPosition, gameObjectToDrag.transform.position) > 0.5)
				ps = sol.FindClosestSolution(this);
			
			DateTime now = DateTime.Now;
			diffSeconds = (now - beginMoving).TotalSeconds;
			if (ps != null)
				sol.PlaceThePiece(gameObjectToDrag, ps, diffSeconds, this);
			else {
				gameObjectToDrag.transform.position = originalPosition;
			}
			dragPrinted = false;
		}
	}

	public void AndrDragPiece(GameObject gameObjectToDrag, Touch touch) {
		if (gameObject == gameObjectToDrag && !isLocked) {
			
			touchWorldPos = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0));
			offset = touchWorldPos - originalPosition;
			
			draggingMode = true;
			beginMoving = DateTime.Now;
		}
	}

	public void AndrContinueDragging(GameObject gameObjectToDrag, Touch touch) {
		if (draggingMode && gameObject == gameObjectToDrag && draggingMode && !isLocked) {
			touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
			v3target = touchWorldPos - offset;
			float targetWidth = Mathf.Clamp(v3target.x, -maxWidth, maxWidth);
			float targetHeigth = Mathf.Clamp(v3target.y, -maxHeight, maxHeight);
			gameObjectToDrag.transform.position = new Vector3(targetWidth, targetHeigth, -1.0f);

			//if(RelativeDistance(originalPosition, gameObjectToDrag.transform.position) > 1 && !dragPrinted){
            if(!dragPrinted){
                UtterancesManager.Instance.WriteJSON("DRAGGING " + gameObject.name);
                dragPrinted = true;
				GameState.Instance.StartedMoving((Piece)this);
			}
		}
	}
	
	public void AndrStopDragging(GameObject gameObjectToDrag, Touch touch) {

		if(gameObject == gameObjectToDrag && draggingMode && !isLocked){
			draggingMode = false;

			PieceSolution ps = null;
			if(RelativeDistance(originalPosition, gameObjectToDrag.transform.position) > 0.5)
				ps = sol.FindClosestSolution(this);			

			DateTime now = DateTime.Now;
			diffSeconds = (now - beginMoving).TotalSeconds;
			if (ps != null)
				sol.PlaceThePiece(gameObjectToDrag, ps, diffSeconds, this);
			else {
				gameObjectToDrag.transform.position = originalPosition;
			}
			dragPrinted = false;
		}
	}
}
