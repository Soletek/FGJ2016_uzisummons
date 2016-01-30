using UnityEngine;
using System.Collections;

public class WalkingDemon : Enemy {

    public enum AIstate { SUMMONED, WALKING, STOPPED, ATTACKING_1, ATTACKING_2 };
    public AIstate state;
    int patternDir = 1;

    public GameObject projectilePrefab;

    public float shootingCooldown = 3.0F;
    public float stateTimer;

    public override void SetData(LevelEnemyDataCollection data)
    {
        if (data.pattern == 1)
        {
            patternDir = 1;
            transform.position = new Vector3(-9, 11, 0);
        }
        if (data.pattern == 2 || data.pattern == 3)
        {
            patternDir = -1;
            transform.position = new Vector3(9, 11, 0);
        }
    }

    protected override void UpdateAi()
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
                int rand = Random.Range(0, 4) + 1;

                if (rand <= 2) {
                    state = AIstate.WALKING;
                    stateTimer = Random.Range(4F, 6F);
                }

                if (rand > 2)
                {
                    state = AIstate.WALKING;
                    stateTimer = Random.Range(4F, 6F);
                }
            }
        }

        if (state == AIstate.WALKING)
        {
            if (stateTimer < 0) state = AIstate.STOPPED;
        }

        if (state == AIstate.ATTACKING_1)
        {
            if (stateTimer < 0)
            {
                state = AIstate.STOPPED;
            }
            // TODO
        }

        if (state == AIstate.ATTACKING_2)
        {
            if (stateTimer < 0) state = AIstate.WALKING;

            // TODO
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();

        // simulate gravity
        rbody.AddForce(new Vector3(0, -150F, 0));

        if (state == AIstate.WALKING)
        {
            // move
            rbody.AddForce(new Vector3(95F * patternDir, 0, 0));
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Floor")
        {
            if (state == AIstate.SUMMONED)
            {
                audioHandler.PlaySound("ImpactMetal_big");
                GameObject.Find("Main Camera").GetComponent<CameraShake>().InvokeShake(1.0F);
                state = AIstate.STOPPED;
                stateTimer = 1.2F;
            }
        }
    }


    void Shoot(Vector3 angle)
    {
        // TODO WORK
        Vector2 shootvector = angle;
        shootvector.Normalize();
        shootingCooldown = Random.Range(1.9F, 2.1F);
        GameObject g;
        g = (GameObject)Instantiate(projectilePrefab, (Vector2)transform.position + shootvector * 0.2F, new Quaternion(0, 0, 0, 0));
        ProjectileScript projectileScript = g.GetComponent<ProjectileScript>();
        projectileScript.Speed = 8F;
        projectileScript.damage = 10F;
        projectileScript.GiveDirection(shootvector);
    }
}
