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
	// Use this for initialization
	void Start () {
		MeshRenderer meshrenderer = gameObject.GetComponent<MeshRenderer> ();
		meshrenderer.enabled = false;
		DisableHilight ();
    }
	
	// Update is called once per frame
	public void CalledUpdate (float progress) {
	    if (barScale.x < barRunTimeInSeconds)
        {
            barScale.x = bar.transform.localScale.x + Time.deltaTime;
            barPosition.x = barScale.x / 2.0f - barRunTimeInSeconds / 2.0f;
            bar.transform.localScale = barScale;
            bar.transform.localPosition = barPosition;
        }
	}

	public void EnableBar ()
	{
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

	public void DisableBar()
	{
		bar.SetActive (false);
	}
}
