using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public Vector2 Direction;
	public float Speed = 25.0f;
	public float lifetime = 2.0f;
    public float damage = 20.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifetime = lifetime - Time.deltaTime;
		if (lifetime < 0) {
			Destroy (gameObject);
		}
	
	}

	public void GiveDirection(Vector2 direction)
	{
		GetComponent<Rigidbody2D> ().velocity = direction * Speed;
	}

}
