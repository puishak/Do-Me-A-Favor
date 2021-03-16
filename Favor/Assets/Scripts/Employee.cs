using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : Character
{
    public int sallaryPerMinute = 20;
    public Shop shop;
    public bool onService;

    new public void Update()
    {
        if (!newAim)
        {
            if (isCarrying())
            {
                walkTo(shop, false);
            }
            if (!onService)
            {
                if (shop.atService == null)
                {
                    if (location != shop.location)
                    {
                        walkTo(shop, true);
                    }
                    else
                    {
                        startService();
                    }
                }
                else if (shop.outstandingDeliveries.Count > 0)
                {
                    DoDelivery(shop.outstandingDeliveries[0]);   
                }
                else
                {
                    Loiter();
                }
            }
        }

        base.Update();
    }

    public void startService()
    {
        if (location == shop.location)
        {

            shop.StartService(this);
            onService = true;
        }
    }

    public void stopService()
    {
        walkOut();
    }

    void DoDelivery(DeliveryBox delivery)
    {
        pickUpDelivery(delivery);
    }
    
    public void getFired()
    {
        if (isCarrying())
        {
            dropDelivery();
        }
        Destroy(gameObject);
    }

}
