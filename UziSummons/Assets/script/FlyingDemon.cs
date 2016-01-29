using UnityEngine;
using System.Collections;

public class FlyingDemon : Enemy
{
    
    public enum AIstate { AI_SUMMONED, AI_SHOOTING, AI_SWAP_POS, AI_FLYING };
    public AIstate state;

    public GameObject projectilePrefab;
    public GameObject player;

    public float shootingCooldown;

    protected override void UpdateAi()
    {
        shootingCooldown -= Time.deltaTime;
        if (shootingCooldown < 0) Shoot(player);

        if (state == AIstate.AI_SUMMONED)
        {
            state = AIstate.AI_SWAP_POS;
            targetPosition = new Vector2(Random.Range(-9, 9), Random.Range(5, 8)); 
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