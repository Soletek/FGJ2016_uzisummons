using UnityEngine;
using System.Collections;

public class LifeTimer : MonoBehaviour {

    public float lifetime = 3.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifetime = lifetime - Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
    }
}
