using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float hp;
    bool isAlive = true;
    public Vector2 targetPosition;
    

    // Use this for initialization
    void Start () {
        if (hp == 0) hp = 50F;

	}

    void FixedUpdate() {

        if (isAlive)
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();

            if (true) { // TODO (mode == flying) {
                Vector2 forcePosition = (targetPosition - (Vector2)transform.position);
                Vector2 forceDirection = forcePosition.normalized;
                float speed = forcePosition.magnitude;
                Vector2 force = forceDirection.normalized * speed;

                rbody.AddForce(force);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {


	    if (hp <= 0)
        {
            isAlive = false;
        }

        if (isAlive)
        {
            UpdateAi();
        }
	}

    protected virtual void UpdateAi()
    {}

    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerProjectile") {
            hp = hp - other.GetComponent<ProjectileScript>().damage;
            Destroy(other);
        }
    }
}


