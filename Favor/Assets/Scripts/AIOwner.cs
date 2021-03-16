using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOwner : Owner
{
    List<GameObject> loiterPoints = new List<GameObject>();

    public enum FavorType {DELIVERY, FIX }


    bool doingFavor;
    FavorType favorType;

    int minimumInventory = 20;

    new private void Start()
    {
        base.Start();

        for (int i = 0; i < 5; i++)
        {
            GameObject chosenPoint = null;
            foreach (GameObject point in game.wayPoints)
            {
                if (!loiterPoints.Contains(point))
                {
                    if (chosenPoint == null || DistanceFromShop(point) < DistanceFromShop(chosenPoint))
                    {
                        chosenPoint = point;
                    }
                }
            }
            loiterPoints.Add(chosenPoint);
        }

    }

    float DistanceFromShop(GameObject obj)
    {
        return Vector3.Distance(obj.transform.position, shop.door.transform.position);
    }

    new public void Update()
    {
        if (money > shop.employees.Count * game.AImoneyPerEmployee)
        {
            shop.HireEmployee();
        }else if(money < (shop.employees.Count - 1) * game.AImoneyPerEmployee)
        {
            shop.FireEmployee();
        }
        if (shop.inventory + (shop.outstandingDeliveries.Count + shop.activeDeliveries.Count) * game.deliverySize < minimumInventory)
        {
            shop.GenerateDelivery(1);
        }
        if (shop.inventory + (shop.outstandingDeliveries.Count + shop.activeDeliveries.Count) * game.deliverySize < money / 10)
        {
            shop.GenerateDelivery(1);
        }

        if (!newAim)
        {
            if (isCarrying())
            {
                if (gameObject.GetComponentInChildren<DeliveryBox>().shop == shop)
                {
                    walkTo(shop, false);
                    
                }
                else if (gameObject.GetComponentInChildren<DeliveryBox>().shop == game.player.shop)
                {
                    walkTo(game.player.shop, false);
                }
            }
            else if (doingFavor)
            {
                if (favorType == FavorType.DELIVERY)
                {
                    if (game.player.shop.activeDeliveries.Count > 0)
                    {
                        pickUpDelivery(game.player.shop.activeDeliveries[0]);
                    }
                    doingFavor = false;
                }
                else if (favorType == FavorType.FIX)
                {
                    if (location == game.player.shop.location)
                    {
                        game.player.shop.fixIssue(0, this);
                        doingFavor = false;
                    }
                    else
                    {
                        walkTo(game.player.shop, true);
                    }
                }
            }
            else
            {
                findAim();
            }
        }

        base.Update();
    }

    public bool AskFavor(FavorType type)
    {
        if (UnityEngine.Random.Range(0, 100) > relationship)
        {
            doingFavor = true;
            favorType = type;
            return true;
        }
        else
        {
            return false;
        }
    }

    void findAim()
    {
        if (shop.employees.Count <= 1)
        {
            if (shop.issues.Count > 0)
            {
                if (shop.issues.Count > 6 || shop.issues.Count >= shop.activeDeliveries.Count)
                {
                    FixIssue();
                }
                else
                {
                    DoDelivery();
                }
            }
            else 
            {
                DoDelivery();
            }
        }
        else if (shop.issues.Count > 0) 
        {
            FixIssue();
        }
        else
        {
            Loiter();
        }

    }

    void FixIssue()
    {

        if (location == shop.location)
        {
            shop.fixIssue(0, this);
        }else
        {
            walkTo(shop, true);
        }
    }


    void DoDelivery()
    {
        if (shop.activeDeliveries.Count > 0)
        {
            pickUpDelivery(shop.activeDeliveries[0]);
            shop.StartDelivery(shop.activeDeliveries[0]);
        }
        else
        {
            Loiter();
        }
    }

    new private void Loiter()
    {
        int whichPoint = UnityEngine.Random.Range(0, loiterPoints.Count);
        walkTo(loiterPoints[whichPoint].transform.position);
    }
}
