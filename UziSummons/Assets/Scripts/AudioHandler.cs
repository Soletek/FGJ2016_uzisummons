using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

    public GameObject audioPrefab;

    public AudioClip[] music;
    int currentMusic = -1;
    float musicFadeVolume = 1;
    bool fade = false;

	// Use this for initialization
	void Start () {
        SwapMusicTrack(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (fade)
        {
            musicFadeVolume -= Time.deltaTime * 1.5F;
            GetComponent<AudioSource>().volume = musicFadeVolume;
            if (musicFadeVolume <= 0) fade = false;
        }
    }

    // ids
    //  -1 = silence;
    //  0 = main;
    //  1 = boss;
    public void SwapMusicTrack(int id)
    {
        if (id == currentMusic)
        {
            return;
        }

        currentMusic = id;

        if (id == -1)
        {
            musicFadeVolume = 1.0F;
            fade = true;
        }
        else
        {
            GetComponent<AudioSource>().volume = 1.0F;
            GetComponent<AudioSource>().clip = music[id];
            GetComponent<AudioSource>().Play();
        }
    }

    public void PlaySound(string clipName, float pitch = -1F)
    {
        PlaySound((AudioClip)Resources.Load("Audio/" + clipName), pitch);
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
