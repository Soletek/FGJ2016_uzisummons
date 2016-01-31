using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	enum Controllmethod {Controller, Mouse};
	Controllmethod controllmethod = Controllmethod.Controller;
	public GameObject Reloadbar;
	bool Sweetspotused = true;
	bool Isreloading = false;
	bool Isdashing = false;
	bool Islookingright = true;
	public GameObject Character;
	public GameObject progressbar;
	public float ShootSpawndistance = 1.0f;
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
	public GameObject Righthand;
	float Dashingcooldown = 0.0f;
	float Reloadcooldown = 0.0f;
	float Shootingcooldown = 0.0f;
	float timer = 0.0f;
	float Spread = 0.1f;
	public int Clipsize = 60;
	Vector3 Oldmouseloc;
	Vector2 lastshot;
	Vector2 truelastshot;
	public GameObject projectile;
    public AudioHandler audioHandler;
	public Texture2D Crosshair;
	bool Isflying = false;
	float Flyingcooldown = 0.0f;
	float Flyingtime = 0.5f;

	// Use this for initialization
	void Start () {
        if (audioHandler == null) audioHandler = GameObject.Find("AudioHandler").GetComponent<AudioHandler>();
        Oldmouseloc = Input.mousePosition;
		controllmethod = Controllmethod.Controller;
		ProgressBar progresbar = progressbar.GetComponent<ProgressBar> ();
		progresbar.DisableBar ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hp < 0) {
			SceneManager.LoadScene (1);
		}
		FigureTruelastshot ();
		Modelhandling ();
		ControllCheck ();
		if (AliveCheck()) {
			Cooldowns ();
			if (!Isflying) {
				Movementcheck ();
			}
			Shooting ();
		//	Debug.Log (Reloadcooldown);
		}
		HardLimits ();
	
	}

	public void Movementcheck()
	{
		if (((Input.GetKey (KeyCode.JoystickButton6) || Input.GetKey (KeyCode.JoystickButton7)) || (Input.GetKeyDown(KeyCode.Q) 
			|| Input.GetKey(KeyCode.E)) )&& Canjump && !Isdashing && !((Input.GetKeyDown (KeyCode.JoystickButton6)
				&& Input.GetKey (KeyCode.JoystickButton7)) || (Input.GetKey(KeyCode.Q) && (Input.GetKey(KeyCode.E)) ))) {
			// Debug.Log ("dash!");

			//DASH SOUND EFFECT

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
			if ((Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.Space) ) && Canjump) {
				Canjump = false;
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 10.0f);
			}

			if ((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) && !(Input.GetKey (KeyCode.A) && (Input.GetKey (KeyCode.D)))) {
				if (Input.GetKey (KeyCode.A)) {
					Islookingright = false;
					//Play running animation
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (-1.0f * speed, GetComponent<Rigidbody2D> ().velocity.y);
				} else {
					Islookingright = true;
					//Play running animation
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (1.0f * speed, GetComponent<Rigidbody2D> ().velocity.y);
				}
			} else {
				
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, GetComponent<Rigidbody2D> ().velocity.y);
				if (Input.GetAxis ("RightstickHori") > 0.0f) {
					Islookingright = true;
					//Play running animation
				} else if (Input.GetAxis ("RightstickHori") < 0.0f) {
					Islookingright = false;
					//Play running animation
				} else {

					//Stop runnin animation
				}
				}
		}


	}

	public void Shooting()
	{

		if (!Isreloading) {
			if (Input.GetKeyDown (KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) {
				Isreloading = true;
				ProgressBar progresbar = progressbar.GetComponent<ProgressBar> ();
				progresbar.EnableBar ();
				progresbar.ResetProgressBar ();
				progresbar.EnableHilight ();
                audioHandler.PlaySound("step1");

                Reloadcooldown = Reloadrate;
				Sweetspotused = false;
			} else if ((Input.GetMouseButton(0) || Input.GetAxis ("RightstickHori") != 0 || Input.GetAxis ("RightstickVert") != 0) && Shootingcooldown <= 0 && !Isdashing) {
				if (Clipsize > 0) {
					Clipsize--;
					if (!Input.GetMouseButton (0)) {
						Vector2 shootvector = new Vector2 (Input.GetAxis ("RightstickHori"), Input.GetAxis ("RightstickVert"));
						shootvector = shootvector + new Vector2 (shootvector.magnitude * Random.Range (-Spread, Spread), shootvector.magnitude * Random.Range (-Spread, Spread));
						shootvector.Normalize ();
						lastshot = shootvector;
						Shootingcooldown = Firerate;
						GameObject g;
						g = (GameObject)Instantiate (projectile, (Vector2)Righthand.transform.position + shootvector * ShootSpawndistance, new Quaternion (0, 0, 0, 0));
						ProjectileScript projectilescript = g.GetComponent<ProjectileScript> ();
						projectilescript.GiveDirection (shootvector);
					} else {

						Vector3 mouse_pos;
						Vector3 obj_pos;
						float angle;
						mouse_pos = Input.mousePosition;
						mouse_pos.z = 0f;
						obj_pos = Camera.main.WorldToScreenPoint (transform.position);
						mouse_pos.x = mouse_pos.x - obj_pos.x;
						mouse_pos.y = mouse_pos.y - obj_pos.y;
						angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
						Vector3 newFwd = Quaternion.Euler (new Vector3 (0, 0, angle)) * Vector3.right;
						Vector3 shootvector = newFwd;
						shootvector = shootvector + new Vector3 (shootvector.magnitude * Random.Range (-Spread, Spread), shootvector.magnitude * Random.Range (-Spread, Spread));
						shootvector.Normalize ();
						Shootingcooldown = Firerate;
						GameObject g;
						g = (GameObject)Instantiate (projectile, Righthand.transform.position + shootvector * ShootSpawndistance, new Quaternion (0, 0, 0, 0));
						ProjectileScript projectilescript = g.GetComponent<ProjectileScript> ();
						projectilescript.GiveDirection (shootvector);
					}
				}
					else{
					if (Shootingcooldown > 0) {
						Shootingcooldown = Firerate;
						audioHandler.PlaySound ("Dry Fire Gun");
					}
                }

			}

		} else if ((Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) && Sweetspotused == false)
		{
			if ((Reloadsweetspotlocation - Reloadsweetspotleeway) <= Reloadcooldown && Reloadcooldown <= (Reloadsweetspotlocation + Reloadsweetspotleeway)) {

                //Succesful spee reload YAY reloadgood effect
                audioHandler.PlaySound("step2.good");
				ProgressBar progresbar = progressbar.GetComponent<ProgressBar> ();
				progresbar.DisableBar ();
                Isreloading = false;
				Clipsize = 60;
				Reloadcooldown = 0;
				Sweetspotused = true;

			}
			else 
			{
                //Shitty reload, ugly sound
                audioHandler.PlaySound("step2.bad");
				ProgressBar progresbar = progressbar.GetComponent<ProgressBar> ();
				progresbar.DisableHilight ();
				float oldcooldown = Reloadcooldown;
                Sweetspotused = true;
				Reloadcooldown = Reloadcooldown + 1.0f;
				if (Reloadcooldown > Reloadrate)
				{
					Reloadcooldown = Reloadrate;
				}
				oldcooldown = Reloadcooldown - oldcooldown;
				progresbar.RemoveTime (oldcooldown);
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

			ProgressBar progresbar = progressbar.GetComponent<ProgressBar> ();

			
			progresbar.CalledUpdate (Time.deltaTime);
			Reloadcooldown = Reloadcooldown - timer;
			if (Reloadcooldown <= 0) {

                //Reload complete, play a sound
                audioHandler.PlaySound("step3");
				progresbar.DisableBar ();
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

	
	if (Flyingcooldown > 0) {
		Flyingcooldown = Flyingcooldown - timer;
		if (Flyingcooldown <= 0)
		{
			Isflying = false;
		}
	}
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Floor") {
			Canjump = true;
		}

		if (coll.gameObject.tag == "Enemy") {
			if (coll.gameObject.transform.position.x > transform.position.x) {
				if (!Isflying) {
					Canjump = false;
					hp = hp - 10.0f;
					Isflying = true;
					Flyingcooldown = Flyingtime;
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (-10.0f, 10.0f);
				}
				
			} else {
				if (!Isflying) {
					Canjump = false;
					hp = hp - 10.0f;
					Isflying = true;
					Flyingcooldown = Flyingtime;
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (10.0f, 10.0f);
				}

			}
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
            //YOu take hit, ouch sound
            audioHandler.PlaySound("BodyHit2");

            GiveDamage(other.GetComponent<ProjectileScript>().damage);
            other.GetComponent<ProjectileScript>().ObjectCollision(1);
        }
    }

	void ControllCheck()
	{
		switch (controllmethod) {
		case Controllmethod.Controller:
			if (MouseCheck ()) {
				controllmethod = Controllmethod.Mouse;
				Debug.Log ("siirrtyaan hiireen");
			}
			break;
		case Controllmethod.Mouse:
			if (ControllerCheck ()) {
				Debug.Log ("siirrytaan ohjaimeen");
				controllmethod = Controllmethod.Controller;
			}
			break;

		}
		
	}

	bool MouseCheck()
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
			{return true;
			}
			else if (Input.mousePosition != Oldmouseloc)
			{
			return true;
			}
		Oldmouseloc = Input.mousePosition;
		return false;
			}

	bool ControllerCheck()
	{
		if (Input.GetKey (KeyCode.Joystick1Button4) || Input.GetKey (KeyCode.Joystick1Button5) || Input.GetKey (KeyCode.Joystick1Button6) || Input.GetKey (KeyCode.Joystick1Button7)) {
			Oldmouseloc = Input.mousePosition;
			return true;
		} else if ((Input.GetAxis ("Horizontal") != 0.0f) || (Input.GetAxis ("Vertical") != 0.0f) || (Input.GetAxis ("RightstickHori") != 0.0f) || (Input.GetAxis ("RightstickVert") != 0.0f)) {
			Oldmouseloc = Input.mousePosition;
			return true;
		}
		Oldmouseloc = Input.mousePosition;
		return false;
	}

	void Modelhandling()
	{
		if (Islookingright) {
			Character.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

        } else {
			Character.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        }

		if (controllmethod == Controllmethod.Mouse) {
			Vector3 mouse_pos;
			Vector3 obj_pos;
			float angle;
			mouse_pos = Input.mousePosition;
			mouse_pos.z = 10;
			obj_pos = Camera.main.WorldToScreenPoint (Righthand.transform.position);
			mouse_pos.x = mouse_pos.x - obj_pos.x;
			mouse_pos.y = mouse_pos.y - obj_pos.y;
			angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            if (Islookingright)
                Righthand.transform.localRotation = Quaternion.Euler(new Vector3(-angle - 90, 0, 0));
            else
                Righthand.transform.localRotation = Quaternion.Euler(new Vector3(angle + 90, 0, 0));
        } else {
			float angle;
			Vector3 location;
			location =  truelastshot;

			if (Islookingright) {
				angle = Mathf.Atan2 (location.x, -location.y) * Mathf.Rad2Deg;
				Righthand.transform.localRotation = Quaternion.Euler (new Vector3 (-angle, 0, 0));
			} else {
				angle = Mathf.Atan2 (-location.x,  location.y) * Mathf.Rad2Deg;
				Righthand.transform.localRotation = Quaternion.Euler (new Vector3 (angle + 180, 0, 0));
			}
		}	
	}

    void HardLimits()
    {
        if (transform.position.x < -11.2)
        {
            transform.position = new Vector3(-11.2F, transform.position.y, 0);
        }
        if (transform.position.x > 11.2)
        {
            transform.position = new Vector3(11.2F, transform.position.y, 0);
        }
    }

	void OnGUI()
	{
		if (controllmethod == Controllmethod.Mouse) {
			Rect position = new Rect ( Input.mousePosition.x - (Crosshair.width / 2), (Screen.height - Input.mousePosition.y) - (Crosshair.height / 2),Crosshair.width ,Crosshair.height );
			GUI.DrawTexture (position, Crosshair);
		}
		GUILayout.Box (Clipsize.ToString());
		GUILayout.Box ( hp.ToString());
	}

	void FigureTruelastshot()
	{
		if(new Vector2 (Input.GetAxis ("RightstickHori"), Input.GetAxis ("RightstickVert")) != Vector2.zero)
		{
		Vector2 shootvector = new Vector2 (Input.GetAxis ("RightstickHori"), Input.GetAxis ("RightstickVert"));
		truelastshot = shootvector;
	}
	}

}
