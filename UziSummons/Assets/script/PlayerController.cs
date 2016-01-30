using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float hp = 100.0f;
	public bool Canjump = true;
	public float Firerate = 0.25f;
	float Shootingcooldown = 0.0f;
	float timer = 0.0f;
	public GameObject projectile;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (AliveCheck()) {
			Cooldowns ();
			Movementcheck ();
			Shooting ();
		}
	
	}

	public void Movementcheck()
	{
		
		if (Input.GetKeyDown (KeyCode.JoystickButton5) && Canjump) {
			Canjump = false;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 7.0f);
		}
			
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis("Horizontal")*5.0f, GetComponent<Rigidbody2D> ().velocity.y);



	}

	public void Shooting()
	{
		if ((Input.GetAxis ("RightstickHori") != 0 || Input.GetAxis ("RightstickVert") != 0) && Shootingcooldown <= 0) {
			Vector2 shootvector = new Vector2 (Input.GetAxis ("RightstickHori"), Input.GetAxis ("RightstickVert"));
			shootvector.Normalize ();
			Shootingcooldown = Firerate;
			GameObject g;
			g = (GameObject)Instantiate (projectile, (Vector2)transform.position + shootvector * 0.5F, new Quaternion (0, 0, 0, 0));
			ProjectileScript projectilescript = g.GetComponent<ProjectileScript> ();
			projectilescript.GiveDirection (shootvector);
		}

	}

	public void Cooldowns()
	{
		timer = Time.deltaTime;
		if (Shootingcooldown > 0) {
			Shootingcooldown = Shootingcooldown - timer;
			
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Floor") {
			Canjump = true;
		}
	}

	public void GiveDamage(float f)
	{
		hp -= f;
	}

	bool AliveCheck()
	{
		if (hp < 0.0f) {
			return false;
		} else {
			return true;
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyProjectile")
        {
            hp = hp - other.GetComponent<ProjectileScript>().damage;
            Destroy(other);
        }
    }
}
