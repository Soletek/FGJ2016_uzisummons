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

    public float shootingCooldown;
    public float moveCooldown;

    public override void SetData(LevelEnemyDataCollection data)
    {
        if (data.pattern == 1)
        {
            pattern = AIpattern.RANDOM;
            transform.position = new Vector3(Random.Range(-10, 10), 10, 0);
        }
        if (data.pattern == 2)
        {
            pattern = AIpattern.WAVE;
            transform.position = new Vector3(-11, 6, 0);
        }
    }

    protected override void UpdateAi()
    {
        if (state == AIstate.SUMMONED)
        {
            state = AIstate.SWAP_POS;
            if (pattern == AIpattern.RANDOM)
            {
                targetPosition = new Vector2(Random.Range(-9, 9), Random.Range(5, 8));
            }

            if (pattern == AIpattern.WAVE)
            {
                patternX = 10F * -patternDir;
                targetPosition = new Vector2(patternX, Mathf.Sin(patternX) *2F + 6F);
            }
        }

        if (state == AIstate.SWAP_POS)
        {
            shootingCooldown -= Time.deltaTime;
            if (shootingCooldown < 0) Shoot(player);

            if (((Vector2)transform.position - targetPosition).magnitude <= 1.5F)
            {
                //state = AIstate.SHOOTING;

                if (pattern == AIpattern.WAVE)
                {
                    patternX = patternX + 0.5F * patternDir;
                    targetPosition = new Vector2(patternX, Mathf.Sin(patternX) * 2F + 6F);
                }
            }
        }

    }

    void Shoot(GameObject target)
    {
        Vector2 shootvector = - (transform.position - target.transform.position) ;
        shootvector.Normalize();
        shootingCooldown = 2F;
        GameObject g;
        g = (GameObject)Instantiate(projectilePrefab, (Vector2)transform.position + shootvector * 0.2F, new Quaternion(0, 0, 0, 0));
        ProjectileScript projectilescript = g.GetComponent<ProjectileScript>();
        projectilescript.Speed = 8F;
        projectilescript.damage = 10F;
        projectilescript.GiveDirection(shootvector);  
    }

}