using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float hp;
    bool isAlive = true;
    public Vector2 targetPosition;
    public GameObject player;
    public float speedMod = 4.0F;

    // Use this for initialization
    void Start () {
        if (hp == 0) hp = 150F;
        player = GameObject.Find("Player"); // TODO from creation
	}

    public virtual void SetData(LevelEnemyDataCollection data)
    {}

    void FixedUpdate() {

        if (isAlive)
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();

            if (true) { // TODO (mode == flying) {
                Vector2 forcePosition = (targetPosition - (Vector2)transform.position);
                Vector2 forceDirection = forcePosition.normalized;
                float speed = forcePosition.magnitude * speedMod;
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
            Destroy(this.gameObject);
        }

        if (isAlive)
        {
            UpdateAi();
        }

		HardLimits ();
	}

    protected virtual void UpdateAi()
    {}

    public void GiveDamage(float f)
    {
        hp -= f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerProjectile")
        {
            GiveDamage(other.GetComponent<ProjectileScript>().damage);
            GetComponent<Rigidbody2D>().AddForce(other.GetComponent<Rigidbody2D>().velocity * 3F);
            other.GetComponent<ProjectileScript>().ObjectCollision(1);
        }
    }


	void HardLimits()
	{
		if (transform.position.x < -11) {
			transform.position = new Vector3 (11, transform.position.y, transform.position.z);
		}
		if (transform.position.x > 11) {
			transform.position = new Vector3 (-11, transform.position.y, transform.position.y);
		}
	}
}


