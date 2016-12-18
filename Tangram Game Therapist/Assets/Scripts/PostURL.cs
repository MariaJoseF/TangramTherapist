using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PostURL : MonoBehaviour {
	/*private static PostURL instance = null;
	string urlMac = "http://192.168.1.65:5000/players_activity/";
	string url = "http://172.20.10.2:5000/players_activity/";
	WWWForm form;
	
	public static PostURL Instance {
		get { 
			return instance; 
		}
	}
	
	void Awake () {
		DontDestroyOnLoad(gameObject);
		
		//Check if instance already exists
		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject);
	}


	void Start () {
		//urlMac = "http://169.254.57.101:5000/players_activity/";
		//WWWForm form = new WWWForm();
		//form.AddField("var1", "value1");
		//form.AddField("var2", "value2");
		//WWW www = new WWW(url, form);
		//StartCoroutine(WaitForRequest(www));
	}

	public void WritePlayersInitialActivity (string player, string puzzle, string difficulty, string mode, string sound, string distanceThreshold) { 
		//url = "http://172.20.10.2:5000/players_activity/";
		WWWForm form = new WWWForm();
		form.AddField("player", player);
		form.AddField("puzzle", puzzle);
		form.AddField("difficulty", difficulty);
		form.AddField("rotation_mode", mode);
		form.AddField ("sound", sound);
		form.AddField ("distance_threshold", distanceThreshold);
		form.AddField("action", "");
		form.AddField("timestamp", "");
		form.AddField("piece", "");
		form.AddField("duration", "");
		form.AddField("direction", "");
		form.AddField("angle", "");

		WWW www = new WWW (urlMac, form);
		//WWW www = new WWW (url, form);
		StartCoroutine (WaitForRequest (www));
	}

	public void WritePlayersFinalActivity (string player, string type, string timestamp) { 
		//url = "http://172.20.10.2:5000/players_activity/";
		WWWForm form = new WWWForm();
		form.AddField("player", player);
		form.AddField("action", type);
		form.AddField("timestamp", timestamp);
		form.AddField("puzzle", "");
		form.AddField("difficulty", "");
		form.AddField("rotation_mode", "");
		form.AddField ("sound", "");
		form.AddField ("distance_threshold", "");
		form.AddField("piece", "");
		form.AddField("duration", "");
		form.AddField("direction", "");
		form.AddField("angle", "");

		WWW www = new WWW (urlMac, form);
		//WWW www = new WWW (url, form);
		StartCoroutine (WaitForRequest (www));
	}
	
	public void WritePlayersMovingActivity (string player, string type, string timestamp, string piece) { 
		//url = "http://172.20.10.2:5000/players_activity/";
		WWWForm form = new WWWForm();
		form.AddField("player", player);
		form.AddField("action", type);
		form.AddField("timestamp", timestamp);
		form.AddField("piece", piece);
		form.AddField("puzzle", "");
		form.AddField("difficulty", "");
		form.AddField("rotation_mode", "");
		form.AddField ("sound", "");
		form.AddField ("distance_threshold", "");
		form.AddField("duration", "");
		form.AddField("direction", "");
		form.AddField("angle", "");

		WWW www = new WWW (urlMac, form);
		//WWW www = new WWW (url, form);
		StartCoroutine (WaitForRequest (www));
	}

	public void WritePlayersDroppingActivity (string player, string type, string timestamp, string piece, string duration) { 
		//url = "http://172.20.10.2:5000/players_activity/";
		WWWForm form = new WWWForm();
		form.AddField("player", player);
		form.AddField("action", type);
		form.AddField("timestamp", timestamp);
		form.AddField("piece", piece);
		form.AddField("duration", duration);
		form.AddField("puzzle", "");
		form.AddField("difficulty", "");
		form.AddField("rotation_mode", "");
		form.AddField ("sound", "");
		form.AddField ("distance_threshold", "");
		form.AddField("direction", "");
		form.AddField("angle", "");

		WWW www = new WWW (urlMac, form);
		//WWW www = new WWW (url, form);
		StartCoroutine (WaitForRequest (www));
	}

	public void WritePlayersRotatingActivity (string player, string type, string timestamp, string piece, string direction, string angle) { 
		//url = "http://172.20.10.2:5000/players_activity/";
		WWWForm form = new WWWForm();
		form.AddField("player", player);
		form.AddField("action", type);
		form.AddField("timestamp", timestamp);
		form.AddField("piece", piece);
		form.AddField("direction", direction);
		form.AddField("angle", angle);
		form.AddField("puzzle", "");
		form.AddField("difficulty", "");
		form.AddField("rotation_mode", "");
		form.AddField ("sound", "");
		form.AddField ("distance_threshold", "");
		form.AddField("duration", "");

		WWW www = new WWW (urlMac, form);
		//WWW www = new WWW (url, form);
		StartCoroutine (WaitForRequest (www));
	}

	public void WritePlayersChangingDirActivity (string player, string type, string timestamp, string piece, string direction) { 
		//url = "http://172.20.10.2:5000/players_activity/";
		WWWForm form = new WWWForm();
		form.AddField("player", player);
		form.AddField("action", type);
		form.AddField("timestamp", timestamp);
		form.AddField("piece", piece);
		form.AddField("direction", direction);
		form.AddField("puzzle", "");
		form.AddField("difficulty", "");
		form.AddField("rotation_mode", "");
		form.AddField ("sound", "");
		form.AddField ("distance_threshold", "");
		form.AddField("duration", "");
		form.AddField("angle", "");

		WWW www = new WWW (urlMac, form);
		//WWW www = new WWW (url, form);
		StartCoroutine (WaitForRequest (www));
	}
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
			// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			//Debug.Log("WWW Error: "+ www.error);
		}    
	}   */ 
	
}
