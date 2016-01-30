using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

    public GameObject audioPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(string name, float pitch = -1F)
    {
        AudioSource sound = Instantiate(audioPrefab).GetComponent<AudioSource>();
        
        if (pitch < 0) {
            sound.pitch = Random.Range(.7F, 1.2F);
        } else {
            sound.pitch = pitch;
        }

        Resources.Load("Audio/" + name);
        sound.transform.SetParent(this.transform);
    }
}
