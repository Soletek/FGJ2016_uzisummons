using UnityEngine;
using System.Collections;

public class FlyingDemon : Enemy
{
    
    public enum AIstate { AI_SUMMONED, AI_SHOOTING, AI_SWAP_POS, AI_FLYING };
    public AIstate state;

    protected override void UpdateAi()
    {
        //base.UpdateAi();

        // Spawn position (-10, 10) - (10, 10) 

        if (state == AIstate.AI_SUMMONED)
        {
            state = AIstate.AI_SWAP_POS;
            targetPosition = new Vector2(Random.Range(-9, 9), Random.Range(5, 8)); 
        }



    }

}