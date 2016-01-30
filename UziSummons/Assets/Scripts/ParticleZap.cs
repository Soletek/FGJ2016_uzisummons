using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleZap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartEffect();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void StartEffect()
    {
        int count = 200;
        List<ParticleSystem.Particle> pars = new List<ParticleSystem.Particle>();

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle p = new ParticleSystem.Particle();
            p.position = transform.position + new Vector3(0, 0, (float)i * 0.01F);
            pars.Add(p);
        }

        GetComponent<ParticleSystem>().SetParticles(pars.ToArray(), count);
    }
}
