using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Character
{
    float willingness;
    public bool purpose;
    public bool referred;
    public Shop referredShop;
    public bool referredByPlayer;

    bool transacting = false;

    new public void Start()
    {
        base.Start();

        willingness = game.maxCustomerWillingness;
    }

    void findAim()
    {
        if (UnityEngine.Random.value <= willingness)
        {
            purpose = true;
            if (referred && referredShop != null)
            {
                walkTo(referredShop, true);
            }
            else
            {
                int whichShop = UnityEngine.Random.Range(0, game.shops.Length);
                walkTo(game.shops[whichShop], true);
            }
        }
        else
        {
            Loiter();

        }
        willingness -= game.decreaseWillingnessOnNewAim;

    }

    

    IEnumerator transact()
    {
        if(location != Location.OUTSIDE)
        {
            yield return new WaitForSeconds(2);
            int unitBuy = UnityEngine.Random.Range(1, game.maxUnitBuy);
            game.findShop[location].ReceiveCustomer(unitBuy, this, referredByPlayer);

            yield return new WaitForSeconds(2);
            walkOut();
            purpose = false;
            transacting = false;
            findAim();
        }
        StopCoroutine(transact());
    }

    new public void Update()
    {
        if (!transacting)
        {
            if (location != Location.OUTSIDE && purpose)
            {
                StartCoroutine(transact());
                transacting = true;
            }
            else if (!newAim)
            {
                findAim();
            }
            
            if (willingness <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                base.Update();
            }

        }
    }

    public void Refer(Shop refShop, bool byPLayer)
    {
        willingness -= game.decreaseWillingnessOnReference;

        referredShop = refShop;
        referredByPlayer = byPLayer;
    }
}
