using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileScript : MonoBehaviour {

	public Vector2 Direction;
	public float Speed = 25.0f;
	public float lifetime = 6.0f;
    public float damage = 20.0f;
    public float gravity = 0.0F;

    AudioHandler audioHandler;

    [SerializeField]
    AudioClip soundName;

    public List<GameObject> destruction = new List<GameObject>();

    // Use this for initialization
    void Start () {
        audioHandler = GameObject.Find("AudioHandler").GetComponent<AudioHandler>();
        audioHandler.PlaySound(soundName);
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, gravity));
    }

    // Update is called once per frame
    void Update () {
		lifetime = lifetime - Time.deltaTime;
		if (lifetime < 0 ) {
			Destroy (gameObject);
		}
	
        if (transform.position.y <= 0)
        {
            audioHandler.PlaySound("Metalhit2");
            ObjectCollision();
        }
	}

	public void GiveDirection(Vector2 direction)
	{
		GetComponent<Rigidbody2D> ().velocity = direction * Speed;
	}

	public void GiveDirection(Vector3 direction)
	{
		GetComponent<Rigidbody2D> ().velocity = direction * Speed;
	}

    // called when projectile collides with another causing a particle effect
    public void ObjectCollision(int animationID = 0)
    {
        Instantiate(destruction[animationID], transform.position, Quaternion.LookRotation(new Vector3(0, 1, 0)));
        Destroy(this.gameObject);
    }



}
