using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LOSESCREEN : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Joystick1Button2))
			{
			Application.Quit();
			}
		if (Input.GetKey (KeyCode.R) || Input.GetKey (KeyCode.Joystick1Button1)) {
			SceneManager.LoadScene (0);
		}
	
	}
}
