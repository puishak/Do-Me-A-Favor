using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBox : MonoBehaviour
{
    public Shop shop;
    public GameObject deliveryPoint;

    public GameObject objHighlight;
    public bool pressed;
    public bool highlight;

    private void Start()
    {
        deliveryPoint = GameObject.FindGameObjectWithTag("DeliveryPoint");
        if (deliveryPoint != null)
        {
            transform.position = deliveryPoint.transform.position;
            transform.rotation = deliveryPoint.transform.rotation;

            deliveryPoint.SetActive(false);
        }
        else
        {
            Debug.LogError("No more delivery points to create new delivery.");
            Destroy(gameObject);
        }
        
    }

    void Update()
    {
        if (pressed)
        {
            //Do some click animation maybe??
        }
        else if (highlight)
        {
            objHighlight.SetActive(true);
        }
        else
        {
            objHighlight.SetActive(false);
        }
        highlight = false;
        pressed = false;
    }

    public void getPickedUp(Transform t)
    {
        transform.SetParent(t);
        //change its position relative to the parent
        transform.localPosition = new Vector3(0, 1, 1);

    }

    public void drop()
    {
        shop.DropDelivery(this);
        transform.localPosition = new Vector3(0, 0, 1);
        transform.SetParent(shop.game.deliveryBoxParent.transform);
    }

    public void doorTriggered(Character.Location location)
    {
        if (shop.location == location)
        {
            shop.ReceiveDelivery(this);
            deliveryPoint.SetActive(true);
            Destroy(gameObject);
        }
    }

    
    
}
