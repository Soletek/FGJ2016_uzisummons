using UnityEngine;
using System.Collections;

public class FlyingDemon : Enemy
{
    
    public enum AIstate { SUMMONED, SHOOTING, SWAP_POS, FLYING };
    public AIstate state;

    public enum AIpattern { RANDOM, WAVE };
    public AIpattern pattern = AIpattern.WAVE;
    int patternDir = 1;
    float patternX;

    public GameObject projectilePrefab;

    public float shootingCooldown = 3.0F;
    public float moveCooldown;

    public override void SetData(LevelEnemyDataCollection data)
    {
        if (data.pattern == 1)
        {
            pattern = AIpattern.RANDOM;
            transform.position = new Vector3(Random.Range(-10F, 10F), 10, 0);
        }
        if (data.pattern == 2 || data.pattern == 3)
        {
            pattern = AIpattern.WAVE;
            patternDir = (data.pattern == 2) ? 1 : -1;
            transform.position = new Vector3(-11 * patternDir, 6, 0);
        }

        // Level music (temp maybe)
        audioHandler.SwapMusicTrack(0);
    }

    void FixedUpdate()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();

        if (isAlive)
        {
            if (true)
            { // TODO (mode == flying) {
                Vector2 forcePosition = (targetPosition - (Vector2)transform.position);
                Vector2 forceDirection = forcePosition.normalized;
                float speed = forcePosition.magnitude * speedMod;
                Vector2 force = forceDirection.normalized * speed;

                rbody.AddForce(force);
            }
        }
        else {
            // simulate gravity
            rbody.AddForce(new Vector3(0, -15F, 0));
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Player")
        {
            if (!isAlive)
            {
                Instantiate(destruction, transform.position, Quaternion.LookRotation(new Vector3(0, 1, 0)));
                Destroy(this.gameObject);
            }
        }
    }

    protected override void ResetAI()
    {
        if (pattern == AIpattern.WAVE)
        {
            patternX = 10F * -patternDir;
            targetPosition = new Vector2(patternX, Mathf.Sin(patternX) * 2F + 6F);
        }
    }

    protected override void UpdateAI()
    {
        if (transform.position.y < 10F)
        {
            shootingCooldown -= Time.deltaTime;
            moveCooldown -= Time.deltaTime;
        }

        if (state == AIstate.SUMMONED)
        {
            state = AIstate.SWAP_POS;
            if (pattern == AIpattern.RANDOM)
            {
                targetPosition = new Vector2(Random.Range(-9F, 9F), Random.Range(5F, 8F));
            }

            if (pattern == AIpattern.WAVE)
            {
                patternX = 10F * -patternDir;
                targetPosition = new Vector2(patternX, Mathf.Sin(patternX) * 2F + 6F);
            }
        }

        if (state == AIstate.SWAP_POS)
        {
            if (shootingCooldown < 0) Shoot(player);

            if (((Vector2)transform.position - targetPosition).magnitude <= 1.5F)
            {

                if (pattern == AIpattern.RANDOM)
                {
                    moveCooldown = Random.Range(5F, 8F);
                    state = AIstate.SHOOTING;
                }

                if (pattern == AIpattern.WAVE)
                {
                    patternX = patternX + 0.5F * patternDir;
                    targetPosition = new Vector2(patternX, Mathf.Sin(patternX) * 2F + 6F);
                }
            }
        }

        if (state == AIstate.SHOOTING)
        {
            if (shootingCooldown < 0) Shoot(player);
            if (moveCooldown < 0)
            {
                if (pattern == AIpattern.RANDOM)
                {
                    state = AIstate.SWAP_POS;
                    targetPosition = new Vector2(Random.Range(-9F, 9F), Random.Range(5F, 8F));
                }
            }
        }
    }

    void Shoot(GameObject target)
    {
        //Vector2 shootvector = - (transform.position - target.transform.position) ; // target player
        Vector2 shootvector = new Vector2(Random.Range(-1F, 1F), -2F);
        shootvector.Normalize();
        shootingCooldown = Random.Range(1.9F, 2.1F);

        GameObject bullet = (GameObject)Instantiate(projectilePrefab, (Vector2)transform.position + shootvector * 0.2F, new Quaternion(0, 0, 0, 0));
        ProjectileScript projectileScript = bullet.GetComponent<ProjectileScript>();
        projectileScript.Speed = 8F;
        projectileScript.damage = 10F;
        projectileScript.gravity = -4F;
        projectileScript.GiveDirection(shootvector);
    }

}