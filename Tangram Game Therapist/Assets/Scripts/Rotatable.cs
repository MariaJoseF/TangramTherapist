using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Rotatable : MonoBehaviour {

	GameObject rotatableObject;
	public ButtonPiece piece;

	Vector3 initialPosition;
	Quaternion initialRotation;

	void Update () {

		#if UNITY_EDITOR
		
		if (Input.GetMouseButtonDown(0)) {
			
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
			if(hit) {
				rotatableObject = hit.collider.gameObject;
				
				if (gameObject == rotatableObject)
					RotatePiece();

			}
		}

		#endif

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				
				if (hit) {
					
					rotatableObject = hit.collider.gameObject;
					
					if (gameObject == rotatableObject)
						RotatePiece ();
				}
			}
		}
	}

	public void Hide () {

		GetComponent<Renderer> ().enabled = false;
	}

	public void Show () {
		
		GetComponent<Renderer> ().enabled = true;
	}

	void RotatePiece () {
		initialPosition = gameObject.transform.position;
		initialRotation = gameObject.transform.rotation;

		// Rotates the piece 45º in the z axis
		if((int)piece.rotationy == 180)
			piece.transform.rotation *= Quaternion.AngleAxis(45.0f, Vector3.forward);
		else piece.transform.rotation *= Quaternion.AngleAxis(-45.0f, Vector3.forward);

		Vector3 vec = piece.transform.eulerAngles;
		vec.z = Mathf.RoundToInt(vec.z);
		piece.transform.eulerAngles = vec;

		// [Hack] Changes piece attribute according each piece
		if (piece.PieceType == Piece.Type.square) {
			if (piece.Rotation == 0)
				piece.Rotation = 315.0f;
			else
				piece.Rotation = 0f;
		} 
		else if (piece.PieceType == Piece.Type.trapezoid && (int)piece.rotationy == 0) {
			if (piece.Rotation == 0)
				piece.Rotation = 315.0f;
			else if (piece.Rotation == 315)
				piece.Rotation = 270f;
			else if (piece.Rotation == 270)
				piece.Rotation = 225f;
			else piece.Rotation = 0f;
		}
		else if (piece.PieceType == Piece.Type.trapezoid && (int)piece.rotationy == 180) {
			if (piece.Rotation == 0)
				piece.Rotation = 225.0f;
			else if (piece.Rotation == 315)
				piece.Rotation = 0f;
			else if (piece.Rotation == 270)
				piece.Rotation = 315f;
			else piece.Rotation = 270f;
		}
		else piece.Rotation = Mathf.Round(piece.transform.eulerAngles.z);


		piece.Rotation = (float)Math.Round((double)piece.Rotation, 0, MidpointRounding.AwayFromZero);
        piece.GetComponent<Piece>().rotation = piece.Rotation;
		int pieceRotation = (int)piece.Rotation;
        UtterancesManager.Instance.WriteJSON("ROTATED " + piece.name + " RIGHT " + pieceRotation.ToString());
        GameState.Instance.RotatedThePiece(piece);

		// The object rotatable follows the piece but does not rotate
		gameObject.transform.position = initialPosition;
		gameObject.transform.rotation = initialRotation;
	}
}
