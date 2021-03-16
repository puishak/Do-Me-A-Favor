using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owner : Character
{
    public int money;
    public Shop shop;
    public int relationship;

    new public void Start()
    {
        base.Start();
        
        relationship = UnityEngine.Random.Range(0, game.initMaxRelationship);

    }

    

    public void ChangeMoney(int amount)
    {
        money += amount;
    }

}
