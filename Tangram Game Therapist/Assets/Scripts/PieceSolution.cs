using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceSolution : MonoBehaviour {
	public Piece.Type pieceType;
	public Vector3 centerPosition;
	public float rotation;
	public float rotationy;
	public string hardImage;
	public Vector3 solutionCenterPosition;
	public Dictionary<int,object[]> edgesToCut = new Dictionary<int, object[]> ();
	bool rotationChanged = false;
	
	public GameObject back;
	public List<GameObject> borders = new List<GameObject>();
	public GameObject whiteBorder;

	public struct RelativePosition {
		public string pos1; //direita, esquerda, centro
		public string pos2; //cima, baixo, centro
		public Dictionary<int,string> adjacentPieces;
	}

	public RelativePosition relPos;

	void Update() {
		if (!rotationChanged && gameObject.name == "Square") {
			if (rotation == 0 || rotation % 10 == 0)
				rotation = 0;
			else
				rotation = 315;
			rotationChanged = true;
		}
	}

	//Colors the sprites
	public void EasyDifSprite() {
		
		if(pieceType == Piece.Type.bigTriangle){
			if(gameObject.name == "Big Triangle 1")
				back.GetComponent<SpriteRenderer>().color = new Color(1f, 0.388f, 0.278f, 0.25f);
			else
				back.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, 0.25f);
		}
		if(pieceType == Piece.Type.littleTriangle){
			if(gameObject.name == "Little Triangle 1")
				back.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.25f);
			else
				back.GetComponent<SpriteRenderer>().color = new Color(0.541f, 0.169f, 0.886f, 0.25f);
		}
		if(pieceType == Piece.Type.mediumTriangle)
			back.GetComponent<SpriteRenderer>().color = new Color(1f, 0.078f, 0.576f, 0.25f);
		if(pieceType == Piece.Type.square)
			back.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.25f);
		if(pieceType == Piece.Type.trapezoid)
			back.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 0.25f);
	}
	
	//Shows the sprite with only borders 
	public void MediumDifSprite() {
		back.GetComponent<Renderer> ().enabled = false;
	}
	
	public void HardDifSprite() {
		back.GetComponent<Renderer> ().enabled = false;
		
		if (pieceType == Piece.Type.bigTriangle || pieceType == Piece.Type.littleTriangle || pieceType == Piece.Type.mediumTriangle)
			HardTriangle ();
		if (pieceType == Piece.Type.square)
			HardSquare ();
		if (pieceType == Piece.Type.trapezoid)
			HardTrapezoid ();
	}
	
	public void HardTriangle(){
		bool tbase = false;
		bool tleft = false;
		bool tright = false;
		
		if (hardImage == "0") {
			tbase = true;
			tleft = true;
			tright = true;
		}
		if (hardImage == "1b"){
			tleft = true;
			tright = true;
		}
		if (hardImage == "1l"){
			tbase = true;
			tright = true;
		}
		if (hardImage == "1r"){
			tleft = true;
			tbase = true;
		}
		if (hardImage == "2c")
			tbase = true;
		if (hardImage == "2l")
			tright = true;
		if (hardImage == "2r")
			tleft = true;
		
		foreach (GameObject border in borders) {
			foreach(KeyValuePair<int, object[]> kvp in edgesToCut) {
				if(kvp.Value[0].ToString() == border.name) {
					border.transform.localScale = new Vector3((float)kvp.Value[3], (float)kvp.Value[4], 0f);
					border.transform.localPosition = new Vector3((float)kvp.Value[1], (float)kvp.Value[2], 0f);
				}
			}
			
			if ((border.name == "base" && tbase) || (border.name == "left" && tleft) || (border.name == "right" && tright))
				border.GetComponent<Renderer> ().enabled = false;
		}
	}
	
	public void HardSquare(){		
		bool stop = false;
		bool sbase = false;
		bool sleft = false;
		bool sright = false;
		
		if (hardImage == "0"){
			stop = true;
			sbase = true;
			sleft = true;
			sright = true;
		}
		if (hardImage == "1b"){
			stop = true;
			sleft = true;
			sright = true;
		}
		if (hardImage == "1t"){
			sbase = true;
			sleft = true;
			sright = true;
		}
		if (hardImage == "1l"){
			stop = true;
			sbase = true;
			sright = true;
		}
		if (hardImage == "1r"){
			stop = true;
			sleft = true;
			sbase = true;
		}
		if (hardImage == "2c"){
			sbase = true;
			sright = true;
		}
		if (hardImage == "2p") {
			sright = true;
			sleft = true;
		}
		if (hardImage == "3")
			sbase = true;
		
		foreach (GameObject border in borders) {

			foreach(KeyValuePair<int, object[]> kvp in edgesToCut) {
				if(kvp.Value[0].ToString() == border.name) {
					border.transform.localScale = new Vector3((float)kvp.Value[3], (float)kvp.Value[4], 0f);
					border.transform.localPosition = new Vector3((float)kvp.Value[1], (float)kvp.Value[2], 0f);
				}
			}

			if ((border.name == "top" && stop) || (border.name == "base" && sbase) || (border.name == "left" && sleft) || (border.name == "right" && sright))
				border.GetComponent<Renderer> ().enabled = false;
		}
	}
	public void HardTrapezoid(){
		bool stop = false;
		bool sbase = false;
		bool sleft = false;
		bool sright = false;
		
		if (hardImage == "0"){
			stop = true;
			sbase = true;
			sleft = true;
			sright = true;
		}
        if (hardImage == "1b")
        {
            stop = true;
            sleft = true;
            sright = true;
        }
        if (hardImage == "1t")
        {
            sbase = true;
            sleft = true;
            sright = true;
        }
        if (hardImage == "1l")
        {
            stop = true;
            sbase = true;
            sright = true;
        }
        if (hardImage == "1r")
        {
            stop = true;
            sleft = true;
            sbase = true;
        }
        if (hardImage == "2tl")
        {
            sbase = true;
            sright = true;
        }
        if (hardImage == "2tr")
        {
            sbase = true;
            sleft = true;
        }
        if (hardImage == "2bl")
        {
            stop = true;
            sright = true;
        }
        if (hardImage == "2br")
        {
            stop = true;
            sleft = true;
        }
        if (hardImage == "2hp")
        {
            sright = true;
            sleft = true;
        }
        if (hardImage == "2vp")
        {
            stop = true;
            sbase = true;
        }
        if (hardImage == "3b")
            sbase = true;
        if (hardImage == "3l")
            sleft = true;
        if (hardImage == "3r")
            sright = true;
		
		foreach (GameObject border in borders) {

			foreach(KeyValuePair<int, object[]> kvp in edgesToCut) {
				if(kvp.Value[0].ToString() == border.name) {
					border.transform.localScale = new Vector3((float)kvp.Value[3], (float)kvp.Value[4], 0f);
					border.transform.localPosition = new Vector3((float)kvp.Value[1], (float)kvp.Value[2], 0f);
				}
			}

			if ((border.name == "top" && stop) || (border.name == "base" && sbase) || (border.name == "left" && sleft) || (border.name == "right" && sright))
				border.GetComponent<Renderer> ().enabled = false;
		}
		
	}
	
}
