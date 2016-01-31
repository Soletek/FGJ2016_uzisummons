using UnityEngine;
using System.Collections;

public class WalkingDemon : Enemy {

    public enum AIstate { SUMMONED, WALKING, STOPPED, ATTACKING_1, ATTACKING_2 };
    public AIstate state;
    int patternDir = 1;
    int rotation = -1;
    public int substate = 0;

    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;

    public float shootingCooldown = 3.0F;
    public float stateTimer;

    public override void SetData(LevelEnemyDataCollection data)
    {
        if (data.pattern == 1)
        {
            patternDir = 1;
            transform.position = new Vector3(-7, 11, 0);
        }
        if (data.pattern == 2 || data.pattern == 3)
        {
            patternDir = -1;
            transform.position = new Vector3(7, 11, 0);
        }

        // Boss music (temp maybe)
        if (audioHandler == null) audioHandler = GameObject.Find("AudioHandler").GetComponent<AudioHandler>();
        audioHandler.SwapMusicTrack(-1);
    }

    protected override void UpdateAI()
    {
        if (transform.position.y < 10F)
        {
            shootingCooldown -= Time.deltaTime;
            stateTimer -= Time.deltaTime;
        }

        if (state == AIstate.STOPPED)
        {
            if (stateTimer < 0)
            {
                rotation = (rotation + 1) % 3;

                if (rotation == 0)
                {
                    state = AIstate.ATTACKING_2;
                    stateTimer = 0.5F;
                    substate = 0;
                }

                if (rotation == 1)
                {
                    state = AIstate.ATTACKING_1;
                    stateTimer = 4F;
                }

                if (rotation == 2)
                {
                    state = AIstate.WALKING;
                    stateTimer = 2.8F;
                }

            }
        }

        if (state == AIstate.WALKING)
        {
            if (stateTimer < 0)
            {
                transform.position = new Vector3(-patternDir * -7, transform.position.y, 0);
                patternDir = patternDir * -1;
                state = AIstate.STOPPED;
                stateTimer = 1.5F;
            } else
            {
                transform.position = new Vector3(-patternDir * (stateTimer * 5F - 7), transform.position.y, 0);
            }
        }

        if (state == AIstate.ATTACKING_1)
        {
            if (stateTimer < 0)
            {
                state = AIstate.STOPPED;
                stateTimer = 1.5F;
            }

            if (shootingCooldown < 0) Shoot();
        }

        if (state == AIstate.ATTACKING_2)
        {
            if (substate == 0 && stateTimer < 0)
            {
                Shoot2();
                stateTimer = 0.2F;
                substate++;
            }
            else if (substate == 1 && stateTimer < 0)
            {
                stateTimer = 0.5F;
                GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 5000F, 0));
                substate++;
            }
            else if (substate == 2 && stateTimer < 0)
            {
                Shoot2();
                stateTimer = 0.7F;
                substate++;
            }
            else if (substate == 3 && stateTimer < 0)
            {
                Shoot2();
                stateTimer = 0.1F;
                substate++;
            }
            else if (substate == 4 && stateTimer < 0)
            {
                state = AIstate.STOPPED;
                stateTimer = 1.5F;
            }
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();

        // simulate gravity
        rbody.AddForce(new Vector3(0, -150F, 0));

        if (!isAlive)
        {
            Instantiate(destruction, transform.position, Quaternion.LookRotation(new Vector3(0, 1, 0)));
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Floor")
        {
            if (state == AIstate.SUMMONED)
            {
                audioHandler.PlaySound("ImpactMetal_big");
                audioHandler.SwapMusicTrack(1);
                GameObject.Find("Main Camera").GetComponent<CameraShake>().InvokeShake(1.0F);
                state = AIstate.STOPPED;
                stateTimer = 2F;
            } else
            {
                GameObject.Find("Main Camera").GetComponent<CameraShake>().InvokeShake(.4F);
            }
        }
    }


    void Shoot()
    {
        Vector2 shootvector = new Vector2(Random.Range(-1F, 1F), .7F);
        shootvector.Normalize();
        shootingCooldown = .06F;
        GameObject bullet = (GameObject)Instantiate(projectilePrefab, (Vector2)transform.position + shootvector * 0.4F, new Quaternion(0, 0, 0, 0));
        ProjectileScript projectileScript = bullet.GetComponent<ProjectileScript>();
        projectileScript.Speed = 18F;
        projectileScript.damage = 30F;
        projectileScript.gravity = -20F;
        projectileScript.GiveDirection(shootvector);
    }

    void Shoot2()
    {
        for (int i = -1; i < 3; i += 2)
        {
            Vector2 shootvector = new Vector2(i, 0);
            shootvector.Normalize();
            GameObject bullet = (GameObject)Instantiate(projectilePrefab2, (Vector2)transform.position + shootvector * 0.4F, new Quaternion(0, 0, 0, 0));
            ProjectileScript projectileScript = bullet.GetComponent<ProjectileScript>();
            projectileScript.Speed = 24F;
            projectileScript.damage = 40F;
            projectileScript.GiveDirection(shootvector);
        }
    }
}
