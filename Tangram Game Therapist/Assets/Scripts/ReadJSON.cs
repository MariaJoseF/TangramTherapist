using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using System.Globalization;
using System;
using System.Text;

public class ReadJSON : MonoBehaviour {
	private static ReadJSON instance = null;
	private string jsonString;
	private JsonData puzzleData;
	private string allData;
	TextAsset jsonFile;
	public string player;
	public Double timestamp;

	public static ReadJSON Instance {
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

        jsonFile = (TextAsset)Resources.Load("puzzles", typeof(TextAsset));
        puzzleData = JsonMapper.ToObject(jsonFile.text);
	}

	//Reads the JSON file and creates a list of PieceSolution
	public Dictionary<int,PieceSolution> GetPuzzle(string name, Dictionary<int,PieceSolution> solPieces) {
		JsonData piece;
		float x, y, sx, sy;
		int id, rot, roty, adjcId;
		string hardImage, adjcRelation = null;
		Dictionary<int,object[]> edgesToCut;
		Dictionary<int,string> adjacentPieces;

		for(int i = 0; i < puzzleData["Puzzles"].Count; i++) {
			if(puzzleData["Puzzles"][i]["name"].ToString() == name) {
                SolutionManager.Instance.puzzleNamept = puzzleData["Puzzles"][i]["namept"].ToString();
				for(int j = 0; j < puzzleData["Puzzles"][i]["Pieces"].Count; j++) {
					piece = puzzleData["Puzzles"][i]["Pieces"][j];
					id = (int)piece["id"];
					x = float.Parse(piece["posx"].ToString());
					y = float.Parse(piece["posy"].ToString());
					sx = float.Parse(piece["solposx"].ToString());
					sy = float.Parse(piece["solposy"].ToString());
					rot = (int)piece["rotation"];
					roty = (int)piece["rotationy"];
					hardImage = piece["hardImage"].ToString();

					//The edges that are not completely drew
					edgesToCut = new Dictionary<int, object[]> ();
					if(piece.Keys.Contains("edgesToCut")) {
						string edge;
						float edgex, edgey, scalex, scaley;
						for(int w = 0; w < piece["edgesToCut"].Count; w++) {
							edge = piece["edgesToCut"][w]["edge"].ToString();
							edgex = float.Parse(piece["edgesToCut"][w]["posx"].ToString());
							edgey = float.Parse(piece["edgesToCut"][w]["posy"].ToString());
							scalex = float.Parse(piece["edgesToCut"][w]["scalex"].ToString());
							scaley = float.Parse(piece["edgesToCut"][w]["scaley"].ToString());
							object[] edgeToCut = {edge, edgex, edgey, scalex, scaley};
							edgesToCut[w] = edgeToCut;
						}
					}

					//Adjacent pieces
                    string pos1 = null, pos2 = null;
					adjacentPieces = new Dictionary<int, string> ();
					if(piece.Keys.Contains("pos1"))
						pos1 = piece["pos1"].ToString();
					if(piece.Keys.Contains("pos2"))
						pos2 = piece["pos2"].ToString();
					for(int w = 0; w < piece["adjacent"].Count; w++){
						adjcId = (int)piece["adjacent"][w]["pieceid"];
						adjcRelation = piece["adjacent"][w]["relation"].ToString();
						adjacentPieces[adjcId] = adjcRelation;
					}
					
					if(id == 0 || id == 1)
						solPieces[id].pieceType = Piece.Type.bigTriangle;
					else if(id == 2)
						solPieces[id].pieceType = Piece.Type.mediumTriangle;
					else if(id == 3 || id == 4)
						solPieces[id].pieceType = Piece.Type.littleTriangle;
					else if(id == 5)
						solPieces[id].pieceType = Piece.Type.trapezoid;
					else
						solPieces[id].pieceType = Piece.Type.square;

					solPieces[id].centerPosition = new Vector3(x, y);
					solPieces[id].solutionCenterPosition = new Vector3(sx, sy);
					solPieces[id].rotation = rot;
					solPieces[id].rotationy = roty;
					solPieces[id].hardImage = hardImage;
					solPieces[id].edgesToCut = edgesToCut;
					solPieces[id].relPos = new PieceSolution.RelativePosition{pos1 = pos1, pos2 = pos2, adjacentPieces = adjacentPieces};
				}
			}
		}
		return solPieces;
	}

    public List<string> GetListOfPuzzles()
    {
        List<string> puzzles = new List<string>();

        for (int i = 0; i < puzzleData["Puzzles"].Count; i++)
        {
            puzzles.Add(puzzleData["Puzzles"][i]["name"].ToString());
        }
        return puzzles;
    }
}
