using UnityEngine;
using System.Collections;

public interface State {

	void BeginFirstGame ();
	
	void BeginNextGame ();

	void EndGame ();

	void HelpMotor ();

	void HelpAdjustingPiece ();

	void GivePositiveFeedback ();

	void GiveNegativeFeedback ();

	void StartedMoving(bool correctAngle);

	void FirstIdlePrompt();

	void FirstAnglePrompt();

	void FirstPlacePrompt();

	void SecondAnglePrompt();

	void SecondPrompt();

	void ThirdAnglePrompt();

	void ThirdPrompt();

	void Update();

    void RepeatPrompt();//acrescentei este

    void HardCluePrompt();//acrescentei este
}
