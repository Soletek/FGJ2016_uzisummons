using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float hp;
    protected bool isAlive = true;
    public Vector2 targetPosition;
    public GameObject player;
    public float speedMod = 4.0F;
    public AudioHandler audioHandler;

    public GameObject destruction;


    // Use this for initialization
    void Start () {
        if (audioHandler == null) audioHandler = GameObject.Find("AudioHandler").GetComponent<AudioHandler>();
        if (hp == 0) hp = 150F;
        player = GameObject.Find("Player"); // TODO from creation
	}

    public virtual void SetData(LevelEnemyDataCollection data)
    {}

    
	
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

    protected virtual void AI_OnFloor()
    { }

    public void GiveDamage(float f)
    {
        hp -= f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Floor")
        {
            if (!isAlive)
            {
                Instantiate(destruction, transform.position, Quaternion.LookRotation(new Vector3(0, 1, 0)));
                Destroy(this.gameObject);
            } else
            {
                AI_OnFloor();
            }
        } 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerProjectile")
        {
            GiveDamage(other.GetComponent<ProjectileScript>().damage);
            GetComponent<Rigidbody2D>().AddForce(other.GetComponent<Rigidbody2D>().velocity * 3F);
            other.GetComponent<ProjectileScript>().ObjectCollision(1);

            audioHandler.PlaySound("BodyHit1");
        }
    }
}


