using UnityEngine;
using System.Collections;
using System;

public class ButtonPiece : Piece {

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

		rotatable = GetComponent<Piece> ().rotatable; //NAO SEI...
		Destroy(center.gameObject);

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

		//[Hack] To rotate the trapezoid 180º in the y axis NÂO SEI SE NÂO ALTERO ISTO [PROVISORIO]
		if (gameObject.name == "trapezoid" && !rotated) {
			Transform[] children = new Transform[this.transform.childCount];
			int i = 0;
			foreach(Transform child in this.transform)
			{
				children[i++] = child;
			}
			// Detach
			this.transform.DetachChildren();
			
			gameObject.transform.rotation = Quaternion.AngleAxis (sol.pieceSolutions [5].rotationy, Vector3.up); //trapezoid
			rotationy = gameObject.transform.eulerAngles.y;
			rotated = true;
			
			// Reparent
			foreach(Transform child in children)
			{
				child.parent = this.transform;
			}
			
			children = null;
			
		}
		
		#if UNITY_EDITOR
		
		if (Input.GetMouseButtonDown(0)) {
			
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
			if(hit) {
				
				gameObjectTouched = hit.collider.gameObject;
				
				if (gameObject == gameObjectTouched && !isLocked && !draggingMode) {

					touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
					offset = touchWorldPos - originalPosition;
					draggingMode = true;

					SetAllRotatablesOff();

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

				if ((DateTime.Now - beginMoving).TotalMilliseconds > 0.02)
					HideRotatable();

				//if(RelativeDistance(originalPosition, gameObjectTouched.transform.position) > 2 && !dragPrinted){
                if(!dragPrinted){
                    UtterancesManager.Instance.WriteJSON("DRAGGING " + gameObject.name);
                    dragPrinted = true;
					GameState.Instance.StartedMoving((Piece)this);
				}

			}
			
		}
		
		if (Input.GetMouseButtonUp(0)) {

			if (gameObject == gameObjectTouched && !isLocked && draggingMode) {
				PieceSolution ps = null;
				if(SolutionManager.Instance.DistanceBetweenPositions(originalPosition, gameObjectTouched.transform.position) > 0.4)
					ps = sol.FindClosestSolution(this);
				DateTime now = DateTime.Now;
				diffSeconds = (now - beginMoving).TotalSeconds;

				//Show the rotable
				SetAllRotatablesOff();
				ShowRotatable();

				if (ps != null) {
					HideRotatable ();
					Destroy (rotatable);
					sol.PlaceThePiece(gameObjectTouched, ps, diffSeconds, this);
				}
				else {
					gameObjectTouched.transform.position = originalPosition;
				}
				dragPrinted = false;
			}
			draggingMode = false;
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
						offset = touchWorldPos - originalPosition;
						draggingMode = true;
							
						SetAllRotatablesOff ();
							
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

					if ((DateTime.Now - beginMoving).TotalMilliseconds > 0.02)
						HideRotatable ();

					//if (RelativeDistance (originalPosition, gameObjectTouched.transform.position) > 2 && !dragPrinted) {
                    if(!dragPrinted){
                        UtterancesManager.Instance.WriteJSON("DRAGGING " + gameObject.name);
                        dragPrinted = true;
						GameState.Instance.StartedMoving((Piece)this);
					}
				}
					
				break;
					
			case TouchPhase.Ended:
				if (gameObject == gameObjectTouched && !isLocked && draggingMode) {
					PieceSolution ps = null;
					if(SolutionManager.Instance.DistanceBetweenPositions(originalPosition, gameObjectTouched.transform.position) > 0.4)
						ps = sol.FindClosestSolution (this);
					DateTime now = DateTime.Now;
					diffSeconds = (now - beginMoving).TotalSeconds;

					//Show the rotable
					SetAllRotatablesOff ();
					ShowRotatable ();

					if (ps != null) {
						sol.PlaceThePiece(gameObjectTouched, ps, diffSeconds, this);
						HideRotatable ();
						Destroy (rotatable);
					} else {
						gameObjectTouched.transform.position = originalPosition;
					}
					dragPrinted = false;
				}
				draggingMode = false;
				break;
			}
		}
	}
	
	public void SetAllRotatablesOff () {
		
		ButtonPiece[] pieces = (ButtonPiece[])UnityEngine.Object.FindObjectsOfType (typeof(ButtonPiece));
		foreach (ButtonPiece p in pieces) {
			if(p.isRotatableOn)
				p.HideRotatable();
		}
		
	}

	public void ShowRotatable () {
		if (rotatable) {
			rotatable.gameObject.layer = LayerMask.NameToLayer("Default");
			isRotatableOn = true;
			rotatable.Show ();
		}
	}
	
}
