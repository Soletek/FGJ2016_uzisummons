using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(string name)
    {
        GameObject sound = (GameObject)Instantiate(Resources.Load("Audio/" + name));
        sound.transform.SetParent(this.transform);
    }
}
