using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    Owner player;
    [SerializeField]
    UI mainUI;


    Shop s;
    Character c;
    DeliveryBox d;

    enum Hoverable 
    {
        NONE,
        POINT,
        SHOP,
        DELIVERY_BOX,
        CHARACTER
    }

    Hoverable hoveringAt;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponent<Owner>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && !IsOverUI())
        {
            s = hit.transform.GetComponentInParent<Shop>();
            c = hit.transform.GetComponent<Character>();
            d = hit.transform.GetComponent<DeliveryBox>();

            if (Physics.Raycast(ray, out hit))
            {
                    
                if (c != null && c != transform.GetComponent<Character>())
                {
                    hoveringAt = Hoverable.CHARACTER;
                }
                else if (s != null)
                {
                    //Clicked on a shop
                    hoveringAt = Hoverable.SHOP;
                }
                else if(d != null)
                {
                    hoveringAt = Hoverable.DELIVERY_BOX;
                }
                else
                {
                    hoveringAt = Hoverable.POINT;
                }
            }
        }
        else
        {
            hoveringAt = Hoverable.NONE;
        }
            
        if (Input.GetMouseButtonDown(0) && !IsOverUI())
        {
            //user clicked
            if (player.location != Character.Location.OUTSIDE)
            {
                //player is inside a shop
                player.walkOut();
                    
            }
            mainUI.gameUI.showHoverNothing();

            switch (hoveringAt)
            {
                case Hoverable.NONE:
                    break;
                    
                case Hoverable.POINT:
                    //Debug.Log("Destination is a point: " + hit.point);
                    player.walkIn = false;
                    player.walkTo(hit.point);
                    break;
                case Hoverable.SHOP:
                    if (s != null && s.location != player.location)
                    {
                        //not the shop player is currently in
                        //Walk into the shop
                        //Debug.Log("Destination is a shop: " + s.location);
                        player.walkTo(s, true);
                        s.pressed = true;
                    }
                    break;
                case Hoverable.CHARACTER:
                    if (c != null)
                    {
                        //player.walkTo(c);
                        //Debug.Log("Destination is a character");
                        c.pressed = true;
                    }
                    break;
                case Hoverable.DELIVERY_BOX:
                    if (d != null)
                    {
                        player.pickUpDelivery(d);
                        //Debug.Log("Destination is a DeliveryBox");
                        d.pressed = true;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            //user is only hovering
            switch (hoveringAt)
            {
                case Hoverable.NONE:
                    mainUI.gameUI.showHoverNothing();
                    break;
                case Hoverable.POINT:
                    mainUI.gameUI.showHoverNothing();
                    break;
                case Hoverable.SHOP:
                    if (s != null)
                    {
                        s.highlight = true;
                        mainUI.gameUI.showHoverShop(s);
                    }
                    break;
                case Hoverable.CHARACTER:
                    if (c != null)
                    {
                        c.highlight = true;
                        Customer customer = c.gameObject.GetComponent<Customer>();
                        Employee employee = c.gameObject.GetComponent<Employee>();
                        Owner owner = c.gameObject.GetComponent<Owner>();

                        if (customer != null) mainUI.gameUI.showHoverCustomer(customer);
                        else if (employee != null) mainUI.gameUI.showHoverEmployee(employee);
                        else if (owner != null) mainUI.gameUI.showHoverOwner(owner);
                    }
                    break;
                case Hoverable.DELIVERY_BOX:
                    if (d != null)
                    {
                        mainUI.gameUI.showHoverDeliveryBox(d);
                        d.highlight = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private bool IsOverUI()
    {
        bool isOverTaggedElement = false;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; ++i)
                {
                    if (results[i].gameObject.CompareTag("Button"))
                        isOverTaggedElement = true;
                }
            }
        }
        return isOverTaggedElement;
    }
}


