using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	bool Sweetspotused = true;
	bool Isreloading = false;
	bool Isdashing = false;
	public float speed = 5.0f;
	public float Reloadsweetspotlocation = 1.0f;
	public float Reloadsweetspotleeway = 0.7f;
	public float Dashingscale = 0.5f;
	public float Dashingtime = 1.0f;
	public float Dashingspeed = 10.0f;
	public float hp = 100.0f;
	public bool Canjump = true;
	public float Firerate = 0.25f;
	public float Reloadrate = 2.0f;
	float Dashingcooldown = 0.0f;
	float Reloadcooldown = 0.0f;
	float Shootingcooldown = 0.0f;
	float timer = 0.0f;
	float Spread = 0.1f;
	public int Clipsize = 60;

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
		//	Debug.Log (Reloadcooldown);
		}
	
	}

	public void Movementcheck()
	{
		if (((Input.GetKey (KeyCode.JoystickButton6) || Input.GetKey (KeyCode.JoystickButton7)) || (Input.GetKeyDown(KeyCode.Q) 
			|| Input.GetKey(KeyCode.E)) )&& Canjump && !Isdashing && !((Input.GetKeyDown (KeyCode.JoystickButton6)
				&& Input.GetKey (KeyCode.JoystickButton7)) || (Input.GetKey(KeyCode.Q) && (Input.GetKey(KeyCode.E)) ))) {
			// Debug.Log ("dash!");
			if (Input.GetKey (KeyCode.JoystickButton6) || Input.GetKey(KeyCode.Q)) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (-Dashingspeed, 0.0f);
				Dashingcooldown = Dashingtime;
				Isdashing = true;
			} else if (Input.GetKey (KeyCode.JoystickButton7) || Input.GetKey(KeyCode.E)) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Dashingspeed, 0.0f);
				Dashingcooldown = Dashingtime;
				Isdashing = true;
			}
		}
	
		if (!Isdashing) {
			if (Input.GetKeyDown (KeyCode.JoystickButton5) && Canjump) {
				Canjump = false;
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 7.0f);
			}

			if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.D)) && !(Input.GetKeyDown (KeyCode.A) && (Input.GetKeyDown (KeyCode.D)))) {
				if (Input.GetKeyDown (KeyCode.A)) {
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (1.0f * speed, GetComponent<Rigidbody2D> ().velocity.y);
				} else {
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (-1.0f * speed, GetComponent<Rigidbody2D> ().velocity.y);
				}
			} else {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, GetComponent<Rigidbody2D> ().velocity.y);
			}
		}


	}

	public void Shooting()
	{

		if (!Isreloading) {
			if (Input.GetKeyDown (KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.R)) {
				Isreloading = true;
				Reloadcooldown = Reloadrate;
				Sweetspotused = false;
			} else if (Input.GetMouseButton(0) || (Input.GetAxis ("RightstickHori") != 0 || Input.GetAxis ("RightstickVert") != 0) && Shootingcooldown <= 0 && !Isdashing) {
				if (Clipsize > 0) {
					Clipsize--;
					if (!Input.GetMouseButton (0)) {
						Vector2 shootvector = new Vector2 (Input.GetAxis ("RightstickHori"), Input.GetAxis ("RightstickVert"));
						shootvector = shootvector + new Vector2 (shootvector.magnitude * Random.Range (-Spread, Spread), shootvector.magnitude * Random.Range (-Spread, Spread));
						shootvector.Normalize ();
						Shootingcooldown = Firerate;
						GameObject g;
						g = (GameObject)Instantiate (projectile, (Vector2)transform.position + shootvector * 0.5F, new Quaternion (0, 0, 0, 0));
						ProjectileScript projectilescript = g.GetComponent<ProjectileScript> ();
						projectilescript.GiveDirection (shootvector);
					}
					else
					{
						Vector3 Cursorpos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						Vector3 shootvector3 = -transform.position + Cursorpos;
						Vector2 shootvector = (Vector2)shootvector3;
						shootvector.Normalize ();
						Shootingcooldown = Firerate;
						GameObject g;
						g = (GameObject)Instantiate (projectile, (Vector2)transform.position + shootvector * 0.5f, new Quaternion (0, 0, 0, 0));
						ProjectileScript projectilescript = g.GetComponent<ProjectileScript> ();
						projectilescript.GiveDirection (shootvector);
					}
				}
			}

		} else if ((Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.R)) && Sweetspotused == false)
		{
			if ((Reloadsweetspotlocation - Reloadsweetspotleeway) <= Reloadcooldown && Reloadcooldown <= (Reloadsweetspotlocation + Reloadsweetspotleeway)) {
				Isreloading = false;
				Clipsize = 60;
				Reloadcooldown = 0;
				Sweetspotused = true;
			}
			else 
			{
				Sweetspotused = true;
				Reloadcooldown = Reloadcooldown + 1.0f;
				if (Reloadcooldown > Reloadrate)
				{
					Reloadcooldown = Reloadrate;
				}
			}
		}

	}

	public void Cooldowns()
	{
		timer = Time.deltaTime;
		if (Shootingcooldown > 0) {
			Shootingcooldown = Shootingcooldown - timer;
			
		}
		if (Reloadcooldown > 0) {
			Reloadcooldown = Reloadcooldown - timer;
			if (Reloadcooldown <= 0) {
				Isreloading = false;
				Clipsize = 60;
			}
		}
		if (Dashingcooldown > 0) {
			Dashingcooldown = Dashingcooldown - timer;
			if (Dashingcooldown <= 0) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				Isdashing = false;
			}

	}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Floor") {
			Canjump = true;
		}
	}

	void OnGUI() {
		GUILayout.Box (Clipsize.ToString());

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
            GiveDamage(other.GetComponent<ProjectileScript>().damage);
            other.GetComponent<ProjectileScript>().ObjectCollision();
        }
    }
}
