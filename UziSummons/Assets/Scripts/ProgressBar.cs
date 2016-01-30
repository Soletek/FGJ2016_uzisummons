using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {
    public float barRunTimeInSeconds;
    public float barHilightPosition; // 0.0 - 1.0
	public bool Showhhilight;
    public GameObject bar;
    public GameObject hilight;
    public GameObject background;
    Vector3 barScale;
    Vector3 barPosition;
	MeshRenderer meshrenderer;
    float progress;
	// Use this for initialization
	void Start () {
		MeshRenderer meshrenderer = gameObject.GetComponent<MeshRenderer> ();
		meshrenderer.enabled = false;
		DisableHilight ();
    }
	
	// Update is called once per frame
	void Update () {
	    if (barScale.x < barRunTimeInSeconds)
        {
            barScale.x = progress;
            barPosition.x = barScale.x / 2.0f - barRunTimeInSeconds / 2.0f;
            bar.transform.localScale = barScale;
            bar.transform.localPosition = barPosition;
        }
	}

	public void EnableBar ()
	{
        ResetProgressBar();
		bar.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
		hilight.transform.localPosition = new Vector3(-barRunTimeInSeconds / 2.0f + barRunTimeInSeconds * barHilightPosition, 0.0f, -.06f);
		barScale = bar.transform.localScale;
		barPosition = bar.transform.localPosition;
		background.transform.localScale = new Vector3(barRunTimeInSeconds, 1.0f, 1.0f);
	}

    public void DisableHilight()
    {
		hilight.SetActive(false);
    }

    public void EnableHilight()
    {
        hilight.SetActive(true);
    }

	public void DisableBar()
	{
		bar.SetActive (false);
        background.SetActive(false);
        hilight.SetActive(false);
	}

    public void SetTime(float time)
    {
        progress = time;
    }

    public void AddTime(float time)
    {
        progress += time;
    }

    public void ResetProgressBar()
    {
        progress = 0.0f;
        bar.SetActive(true);
        background.SetActive(true);
        hilight.SetActive(true);
    }
}
