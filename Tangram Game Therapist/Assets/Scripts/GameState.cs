using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using Assets.Scripts.Exp3;

public class GameState : MonoBehaviour
{
    private static GameState instance = null;
    DateTime now, vibratingTime, clueTime;
    public DateTime stopped = DateTime.Now, beginHardClue;
    public float durationHardClue;
    Color backColor;
    public int numberOfClicks = 0;
    public GameObject panel;
    public GameObject homeButton;
    public Button playButton;
    double diffSeconds = 0;
    public bool dragging = false, showingHardClue = false, gameStarted = false, quit = false,
        initialHelp = false, playButtonInteractable = false, showClue = false, showHardClue = false,
        haveToEnableAllPieces = false, haveToEnablePlayButton = false;
    bool pieceVibrating = false, clueShown = false;
    public PieceSolution showCluePiece;
    Piece vibratingPiece;

    //  Exp3 ExpAlgorithm = new Exp3();


    public struct PieceInfo
    {
        public string shape;
        public string size;
        public string color;
    }
    public Dictionary<string, PieceInfo> piecesInfo = new Dictionary<string, PieceInfo>();

    public Dictionary<int, Piece> placedPieces = new Dictionary<int, Piece>();
    public Dictionary<int, Piece> notPlacedPieces = new Dictionary<int, Piece>();
    List<Piece> allPieces = new List<Piece>();

    public static GameState Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        InitializePiecesInfo();
    }

    void Update()
    {
        if (gameStarted)
        {
            DateTime now = DateTime.Now;
            if (initialHelp && (now - GameManager.Instance.beginGameTime).TotalSeconds > 5)
            {
                initialHelp = false;
                stopped = DateTime.Now;
                GameManager.Instance.beginGameTime = DateTime.Now;
            }
            if (showHardClue)
            {
                showHardClue = false;
                SolutionManager.Instance.ShowHardClue(UtterancesManager.Instance.hardClueSeconds);
            }
            if (showingHardClue)
            {
                if ((now - beginHardClue).TotalSeconds >= durationHardClue)
                {
                    SolutionManager.Instance.GetComponent<SpriteRenderer>().enabled = false;
                    showingHardClue = false;
                }
            }
            if (pieceVibrating)
            {
                if ((now - vibratingTime).TotalSeconds >= 1)
                    StopVibrating();
            }
            if (showClue)
            {
                showClue = false;
                ShowClue();
            }
            if (clueShown)
            {
                if ((now - clueTime).TotalSeconds >= 3)
                    HideClue();
            }
            if (haveToEnableAllPieces)
            {
                EnableAllPieces();
                haveToEnableAllPieces = false;
            }
        }
        if (haveToEnablePlayButton)
        {
            EnablePlayButton();
            haveToEnablePlayButton = false;
        }
    }

    public void BeginGame(SolutionManager.Difficulty difficulty, string puzzle, SceneProperties.RotationMode rotationMode, float distanceThreshold)
    {
        gameStarted = true;
        numberOfClicks = 0;
        diffSeconds = 0;
        dragging = false;
        showingHardClue = false;
        pieceVibrating = false;
        clueShown = false;
        placedPieces = new Dictionary<int, Piece>();
        notPlacedPieces = new Dictionary<int, Piece>();
        Therapist.Instance.BeginGame(difficulty, puzzle, rotationMode, distanceThreshold);
        stopped = DateTime.Now;
        allPieces = new List<Piece>(SolutionManager.Instance.allPieces);
        DisableAllPieces();
    }

    public void EndGame()
    {
        gameStarted = false;
        playButtonInteractable = false;
        Therapist.Instance.EndGame();
    }

    public void StartedMoving(Piece piece)
    {
        dragging = true;
        if (Therapist.Instance.currentPiece != piece)
        {
            if (Therapist.Instance.currentPiece != null)
                Therapist.Instance.currentPiece.StopCountingTime();
            piece.StartCountingTime();
        }
        Therapist.Instance.currentPiece = piece;
        if (pieceVibrating)
            StopVibrating();
        Therapist.Instance.StartedMoving(false);
    }

    public void FoundTheRightSpot(Piece piece)
    {
        Therapist.Instance.currentPiece = null;
        Therapist.Instance.currentPlace = null;
        Therapist.Instance.GivePositiveFeedback();
        stopped = DateTime.Now;
        dragging = false;
    }

    public void RunExp(Piece piece_, int type_feedback, PieceSolution notFoundPlace = null, double notFoundDistance = 0)
    {
        switch (type_feedback)
        {
            case 1://FoundTheRightSpot -> positive feedback
                FoundTheRightSpot(piece_);
                break;
            case 2://NotFoundTheRightSpot -> negative feedback, motor_help and_close_help
                NotFoundTheRightSpot(piece_, notFoundPlace, notFoundDistance);
                break;
            case 3://IncorrectAngle -> negative feedback
                IncorrectAngle(piece_, notFoundPlace);
                break;
            default:
                break;
        }

        //float[] rewards = { 0.2f, 0.2f,
        //0.5f, 0.0f,
        //0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f,
        //0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f};

        //        try
        //        {
        //            action = -1;//No action selected yet


        //            action = ExpAlgorithm.RunExp3();
        //            print("------------------ Action selected = " + action);


        //            switch (action)
        //            {
        //                case 0:// -> motor_help
        //                    Therapist.Instance.MotorHelpState.HelpMotor();
        //                    //UtterancesManager.Instance.MotorHelp();
        //                    break;
        //                case 1:// -> close_help
        //                    //Therapist.Instance.FitHelpState.HelpAdjustingPiece();
        //                    UtterancesManager.Instance.CloseHelp();
        //                    break;
        //                case 2:// -> positive feedback
        //                    //Therapist.Instance.PositiveFeedState.GivePositiveFeedback();
        //                    UtterancesManager.Instance.PositiveFeedback(StringNumberOfPieces(GameState.Instance.notPlacedPieces.Count));
        //                    break;
        //                case 3:// -> negative feedback
        //                       //Therapist.Instance.NegativeFeedState.GiveNegativeFeedback();
        //                    UtterancesManager.Instance.NegativeFeedback();
        //                    break;
        //                case 4:// -> first angle prompt
        //                    //Therapist.Instance.FirstAnglePromptState.FirstAnglePrompt();
        //                    UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(piece_.name)); 
        //                    //UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));

        //                    break;
        //                case 5:// -> first finger angle prompt
        //                    //Therapist.Instance.FirstAnglePromptState.RepeatPrompt();
                            UtterancesManager.Instance.FirstAnglePromptFinger(GameState.Instance.PieceInformation(piece_.name));
        //                  //  GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)
        //                    break;
        //                case 6:// -> first button angle prompt
        //                       // Therapist.Instance.FirstAnglePromptState.RepeatPrompt();
        //                    UtterancesManager.Instance.FirstAnglePromptButton(GameState.Instance.PieceInformation(piece_.name));
        //                    //                    UtterancesManager.Instance.FirstAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));

        //                    break;
        //                case 7:// -> second angle prompt
        //                       //Therapist.Instance.SecondAnglePromptState.SecondAnglePrompt();
        //                    UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(piece_.name));
        //                    //                    UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));

        //                    break;
        //                case 8:// -> second finger angle prompt
        //                       Therapist.Instance.SecondAnglePromptState.SecondAnglePrompt();

        //                    currentPiece = piece_;
        //                   // currentPiece = Therapist.Instance.currentPiece;
        //                    currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);

        //                    rotationDirection = CalculateDirectionOfRotation(currentPiece, currentPlace);

        //                    UtterancesManager.Instance.SecondAnglePromptFinger(GameState.Instance.PieceInformation(piece_.name), rotationDirection);
        //                    break;
        //                case 9:// -> second button angle prompt
        //                       //Therapist.Instance.SecondAnglePromptState.SecondAnglePrompt();
        //                    UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(piece_.name), StringNumberOfClicks(GameState.Instance.numberOfClicks));
        //                    //                    UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), StringNumberOfClicks(GameState.Instance.numberOfClicks));
        //                    break;
        //                case 10:// -> third angle prompt
        //                        //Therapist.Instance.ThirdAnglePromptState.ThirdAnglePrompt();
        //                    UtterancesManager.Instance.ThirdAnglePrompt(GameState.Instance.PieceInformation(piece_.name));
        //                   // UtterancesManager.Instance.ThirdAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
        //                    break;
        //                case 11:// -> stop angle prompt
        //                    //Therapist.Instance.ThirdAnglePromptState.StartedMoving(true);
        //                   // UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
        //                    UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(piece_.name));
        //                    break;
        //                case 12:// -> idle prompt
        //                        //Therapist.Instance.FirstIdlePromptState.FirstIdlePrompt();
        //                    UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(piece_.name));
        //                    //UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
        //                    break;
        //                case 13:// -> place prompt
        //                        //Therapist.Instance.FirstPlacePromptState.FirstPlacePrompt();
        //                    UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(piece_.name));
        ////                    UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
        //                    break;
        //                case 14:// -> sec_1position prompt
        //                        //Therapist.Instance.SecondPromptState.SecondPrompt();
        //                    currentPiece = piece_;
        //                   // currentPiece = Therapist.Instance.currentPiece;
        //                    currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);


        //                    string a = currentPlace.relPos.pos2;



        //                    UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(piece_.name), currentPlace.relPos.pos2);
        //                    break;
        //                case 15:// -> sec_2position prompt
        //                        //Therapist.Instance.SecondPromptState.SecondPrompt();
        //                    currentPiece = piece_;
        //                    // currentPiece = Therapist.Instance.currentPiece;
        //                    currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);
        //                    UtterancesManager.Instance.SecondPrompt2Position(GameState.Instance.PieceInformation(piece_.name), currentPlace.relPos.pos1, currentPlace.relPos.pos2);
        //                    break;
        //                case 16:// -> sec_place prompt
        //                        //Therapist.Instance.SecondPromptState.SecondPrompt();
        //                    currentPiece = piece_;
        //                    // currentPiece = Therapist.Instance.currentPiece;
        //                    currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);
        //                    adjacentPieces = new Dictionary<int, String>(currentPlace.relPos.adjacentPieces);
        //                    availableAdjacentPieces = new Dictionary<int, string>(IntersectDictionaries(adjacentPieces, GameState.Instance.placedPieces));

        //                    int pieceId = GameState.Instance.RandomKeys(availableAdjacentPieces).First();
        //                    string piece = SolutionManager.Instance.FindMatchIdName(pieceId);
        //                    string relativePosition = availableAdjacentPieces[pieceId];

        //                    UtterancesManager.Instance.SecondPromptPlace(GameState.Instance.PieceInformation(piece_.name), relativePosition, GameState.Instance.PieceInformation(piece));
        //                    break;
        //                case 17:// -> third prompt
        //                    //Therapist.Instance.ThirdPromptState.ThirdPrompt();
        //                    UtterancesManager.Instance.ThirdPrompt(GameState.Instance.PieceInformation(piece_.name));
        //                    break;
        //                case 18:// -> hard_clue prompt
        //                    UtterancesManager.Instance.HardClue(3f);
        //                    break;

        //            }
        //        }
        //        catch (Exception e)
        //        {

        //            Debug.Log("Error : " + e.Message);
        //            UtterancesManager.Instance.WriteJSON("Error : " + e.Message + " action = " + action);
        //        }
        //        finally
        //        {
        //            Debug.Log("Finaly ExpAlgorithm.RunExp3(17, rewards, 0.07f);");
        //        }

    }

    /// 
    /// 
    /// 

    Dictionary<int, string> IntersectDictionaries(Dictionary<int, string> dic1, Dictionary<int, Piece> dic2)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        IEnumerable<int> commonKeys = dic1.Keys.Intersect(dic2.Keys);
        foreach (int i in commonKeys)
        {
            result.Add(i, dic1[i]);
        }
        return result;
    }
    Dictionary<int, string> adjacentPieces = new Dictionary<int, string>();
    Dictionary<int, string> availableAdjacentPieces = new Dictionary<int, string>();

    string StringNumberOfPieces(int nPieces)
    {
        string number;
        if (nPieces == 1)
            number = "uma peça";
        else if (nPieces == 2)
            number = "duas peças";
        else
            number = nPieces + " peças";
        return number;
    }

    PieceSolution currentPlace;
    Piece currentPiece;
    string rotationDirection;

    string StringNumberOfClicks(int clicks)
    {
        string number;
        if (clicks == 1)
            number = "uma vez";
        else if (clicks == 2)
            number = "duas vezes";
        else
            number = clicks + " vezes";
        return number;
    }

    string CalculateDirectionOfRotation(Piece currentPiece, PieceSolution currentPlace)
    {
        //Isto não funciona mt bem, mas está quase feito. será que faz sentido usar isto?
        /*string currentDirection;
		int variation;
		if (currentPiece.rotation > 180) 
			variation = ((int)currentPiece.rotation - 260) - (int)currentPlace.rotation;
		else 
			variation = (int)currentPiece.rotation - (int)currentPlace.rotation;

		if (variation < 0)
			currentDirection = "esquerda";
		else
			currentDirection = "direita";
		
		return currentDirection;*/
        int rand = UnityEngine.Random.Range(0, 2);
        return (rand == 0 ? "isquerda" : "direita");
    }

    /// 
    /// 
    ///

    public void NotFoundTheRightSpot(Piece piece, PieceSolution place, double distance)
    {
        if (Therapist.Instance.currentPiece != piece && Therapist.Instance.currentPiece != null)
        {
            Therapist.Instance.currentPiece.StopCountingTime();
        }
        Therapist.Instance.currentPiece = piece;

        //Piece close with the right spot
        if (distance < 1.7)
        {
            UtterancesManager.Instance.WriteJSON("CLOSE TRY " + piece.name);

            if (Therapist.Instance.nFailedTries >= 2)
            {
                GameManager.Instance.closeTries++;
                Therapist.Instance.HelpAdjustingPiece();
            }
        }
        //Piece in the wrong spot
        else if (SolutionManager.Instance.DistanceBetweenPositions(piece.originalPosition, piece.Position) > 1)
        {
            Therapist.Instance.currentPiece = piece;
            Therapist.Instance.nFailedTries++;
            print(Therapist.Instance.nFailedTries + " feed negativo");
            UtterancesManager.Instance.WriteJSON("WRONG TRY " + Therapist.Instance.nFailedTries + " " + piece.name);

            Therapist.Instance.GiveNegativeFeedback();
        }
        //Problems moving the piece
        else if (SolutionManager.Instance.DistanceBetweenPositions(piece.originalPosition, piece.Position) <= 1)
        {
            Therapist.Instance.HelpMotor();
            UtterancesManager.Instance.WriteJSON("HELP MOTOR");
        }
        stopped = DateTime.Now;
        dragging = false;
    }

    public void IncorrectAngle(Piece piece, PieceSolution place)
    {
        Therapist.Instance.currentPiece = piece;
        Therapist.Instance.currentPlace = place;
        Therapist.Instance.GiveNegativeFeedback();
        Therapist.Instance.nWrongAngleTries++;
        stopped = DateTime.Now;
        dragging = false;
    }

    public void RotatedThePiece(Piece piece)
    {
        if (Therapist.Instance.currentPiece != piece)
        {
            if (Therapist.Instance.currentPiece != null)
                Therapist.Instance.currentPiece.StopCountingTime();
            piece.StartCountingTime();
        }
        Therapist.Instance.currentPiece = piece;
        int angle = (int)piece.rotation;

        Therapist.Instance.currentPlace = FindTheClosestPlace(piece);

        if (Therapist.Instance.currentPlace != null && (int)Therapist.Instance.currentPlace.rotation > angle - 1 && (int)Therapist.Instance.currentPlace.rotation < angle + 1)
        {
            Therapist.Instance.StartedMoving(true);
        }
        else
        {
            Therapist.Instance.StartedMoving(false);
        }
        stopped = DateTime.Now;
        dragging = false;
    }

    public PieceSolution FindTheClosestPlace(Piece piece)
    {
        PieceSolution place = null;
        int clicks = 100;
        foreach (KeyValuePair<int, PieceSolution> kvp in SolutionManager.Instance.pieceSolutions)
        {
            if (kvp.Value.pieceType == piece.type)
            {
                int currentClicks;
                if (SceneProperties.Instance.rotationMode == SceneProperties.RotationMode.button)
                    currentClicks = CalculateNumberOfClicks(piece, kvp.Value);
                else
                    currentClicks = CalculateNumberOfRotations(piece, kvp.Value);

                if (currentClicks < clicks)
                {
                    clicks = currentClicks;
                    place = kvp.Value;
                }
            }
        }
        numberOfClicks = clicks;
        return place;
    }

    public PieceSolution FindTheCorrectPlace(Piece piece)
    {
        foreach (KeyValuePair<int, PieceSolution> kvp in SolutionManager.Instance.pieceSolutions)
        {
            if (kvp.Value.pieceType == piece.type)
            {
                if (kvp.Value.rotation > piece.rotation - 1 && kvp.Value.rotation < piece.rotation + 1)
                    return kvp.Value;
            }
        }
        return null;
    }

    public int CalculateNumberOfClicks(Piece currentPiece, PieceSolution currentPlace)
    {
        int pieceRot, placeRot, variation;

        if (currentPiece.PieceType == Piece.Type.trapezoid)
        {
            pieceRot = (int)currentPiece.rotation;
            if (pieceRot >= 180)
                pieceRot -= 180;
            placeRot = (int)currentPlace.rotation;
            if (placeRot >= 180)
                placeRot -= 180;
            if ((int)currentPiece.rotationy == 180)
                variation = placeRot - pieceRot;
            else variation = pieceRot - placeRot;

            if (variation < 0)
                variation += 180;

        }
        else
        {
            pieceRot = (int)currentPiece.rotation;
            placeRot = (int)currentPlace.rotation;
            variation = pieceRot - placeRot;
            if (variation != 0 && currentPiece.PieceType == Piece.Type.square)
                return 1;
            else if (variation < 0)
                variation += 360;
        }
        return variation / 45;
    }

    public int CalculateNumberOfRotations(Piece currentPiece, PieceSolution currentPlace)
    {
        int pieceRot, placeRot, variation, variation1, variation2;

        pieceRot = (int)currentPiece.rotation;
        placeRot = (int)currentPlace.rotation;
        variation1 = pieceRot - placeRot;
        variation2 = placeRot - pieceRot;

        if (variation1 < 0)
            variation1 += 360;
        if (variation2 < 0)
            variation2 += 360;

        if (variation1 < variation2)
            variation = variation1;
        else
            variation = variation2;

        return variation / 45;
    }

    public IEnumerable<TKey> RandomKeys<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        System.Random rand = new System.Random();
        List<TKey> keys = Enumerable.ToList(dict.Keys);
        int size = dict.Count - 1;
        while (true)
        {
            yield return keys[rand.Next(size)];
        }
    }

    public Piece FindNewPiece()
    {
        Dictionary<int, Piece> possiblePieces = new Dictionary<int, Piece>();

        foreach (KeyValuePair<int, Piece> kvp in notPlacedPieces)
        {
            if (FindTheCorrectPlace(kvp.Value) != null)
            {
                possiblePieces[kvp.Key] = kvp.Value;
            }
        }
        if (possiblePieces.Count != 0)
            return possiblePieces[RandomKeys(possiblePieces).First()];
        else return notPlacedPieces[RandomKeys(notPlacedPieces).First()];
    }

    public void VibratePiece(Piece piece)
    {
        piece.animator.enabled = true;
        piece.animator.Play(piece.animatorName);
        vibratingPiece = piece;
        pieceVibrating = true;
        vibratingTime = DateTime.Now;
    }

    void StopVibrating()
    {
        vibratingPiece.animator.enabled = false;
        pieceVibrating = false;
        vibratingPiece.transform.position = vibratingPiece.originalPosition;
        vibratingPiece = null;
    }

    public void ShowClue()
    {
        if (showCluePiece != null)
        {
            showCluePiece.whiteBorder.GetComponent<Renderer>().enabled = true;
            showCluePiece.whiteBorder.GetComponent<Animator>().enabled = true;
            showCluePiece.whiteBorder.GetComponent<Animator>().Play("glow");
            if (Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.easy)
                backColor = showCluePiece.back.GetComponent<SpriteRenderer>().color;
            else
                showCluePiece.back.GetComponent<Renderer>().enabled = true;
            showCluePiece.back.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.25f);
            UtterancesManager.Instance.WriteJSON("SHOW GLOW CLUE " + showCluePiece.name);
        }
        clueShown = true;
        clueTime = DateTime.Now;
    }

    public void HideClue()
    {
        if (showCluePiece != null)
        {
            showCluePiece.whiteBorder.GetComponent<Animator>().enabled = false;
            showCluePiece.whiteBorder.GetComponent<Renderer>().enabled = false;
            if (Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.easy)
                showCluePiece.back.GetComponent<SpriteRenderer>().color = backColor;
            else
                showCluePiece.back.GetComponent<Renderer>().enabled = false;
        }
        clueShown = false;
        showCluePiece = null;
    }

    public string PieceInformation(string id)
    {
        string piece;
        int random = UnityEngine.Random.Range(0, 2);

        if (piecesInfo[id].size == null || random == 0)
            piece = piecesInfo[id].shape + " " + piecesInfo[id].color;
        else piece = piecesInfo[id].shape + " " + piecesInfo[id].size + " " + piecesInfo[id].color;
        return piece;
    }

    void InitializePiecesInfo()
    {
        piecesInfo["bigtriangle1"] = new PieceInfo { shape = "triângulo", size = "grande", color = "laranja" };
        piecesInfo["bigtriangle2"] = new PieceInfo { shape = "triângulo", size = "grande", color = "azul" };
        piecesInfo["mediumtriangle"] = new PieceInfo { shape = "triângulo", color = "cor-de-rosa" };
        piecesInfo["littletriangle1"] = new PieceInfo { shape = "triângulo", size = "pequeno", color = "vermelho" };
        piecesInfo["littletriangle2"] = new PieceInfo { shape = "triângulo", size = "pequeno", color = "roxo" };
        piecesInfo["trapezoid"] = new PieceInfo { shape = "losango", color = "amarelo" };
        piecesInfo["square"] = new PieceInfo { shape = "quadrado", color = "verde" };
    }

    public void InitializeNotPlacedPieces(Piece piece)
    {
        notPlacedPieces[SolutionManager.Instance.FindMatchId(piece.name)] = piece;
    }

    void DisableAllPieces()
    {
        foreach (Piece p in allPieces)
        {
            p.isLocked = true;
        }
    }

    public void EnableAllPieces()
    {
        foreach (Piece p in allPieces)
        {
            p.isLocked = false;
        }
    }

    public void EnableOnePiece(Piece piece)
    {
        piece.GetComponent<Piece>().isLocked = false;

        if (SceneProperties.Instance.rotationMode == SceneProperties.RotationMode.finger)
        {
            piece.GetComponent<TouchPiece>().isLocked = false;
            piece.GetComponentInChildren<Dragable>().isLocked = true;
        }
        else
        {
            piece.GetComponent<ButtonPiece>().ShowRotatable();
        }
    }

    public void DisableOnePiece(Piece piece)
    {
        piece.GetComponent<Piece>().isLocked = true;

        if (SceneProperties.Instance.rotationMode == SceneProperties.RotationMode.finger)
        {
            piece.GetComponent<TouchPiece>().isLocked = true;
            piece.GetComponentInChildren<Dragable>().isLocked = false;
        }
        else
        {
            piece.GetComponent<ButtonPiece>().HideRotatable();
        }
    }

    public void EnablePlayButton()
    {
        playButtonInteractable = true;
    }

    public double DiffSeconds
    {
        get
        {
            return diffSeconds;
        }
    }

}
