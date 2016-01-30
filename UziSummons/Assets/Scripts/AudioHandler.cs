﻿using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

    public GameObject audioPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(AudioClip clip, float pitch = -1F)
    {
        AudioSource sound = Instantiate(audioPrefab).GetComponent<AudioSource>();
        
        if (pitch < 0) {
            sound.pitch = Random.Range(.9F, 1.1F);
            sound.volume = Random.Range(.7F, .8F);
        } else {
            sound.pitch = pitch;
        }

        sound.clip = clip;
        sound.transform.SetParent(this.transform);

        sound.Play();
    }
}
