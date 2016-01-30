﻿using UnityEngine;
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
        if (data.pattern == 2)
        {
            pattern = AIpattern.WAVE;
            transform.position = new Vector3(-11, 6, 0);
        }
    }

    protected override void UpdateAi()
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
        Vector2 shootvector = - (transform.position - target.transform.position) ;
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