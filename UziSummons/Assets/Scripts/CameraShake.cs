using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    public float shake = 0F;
    
	void Update () {
	    if (shake > 0)
        {
            Vector3 angle = new Vector3(Random.Range(-shake, shake), Random.Range(-shake, shake), Random.Range(-shake, shake)) * 2F;
            transform.rotation = Quaternion.Euler(angle);
            shake -= Time.deltaTime;
        } else
        {
            Vector3 angle = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(angle);
        }
	}

    public void InvokeShake (float intensity = .5F)
    {
        shake = intensity;
    }
}
