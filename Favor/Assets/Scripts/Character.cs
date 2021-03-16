using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public enum Location
    {
        OUTSIDE,
        RED_FLOWERS,
        COFFEE_CAFE,
        PALM_SUPERMARKET,
        PAWN_SHOP,
        BLACKWOODS_BAR,
        PIZZARIA,
        WHOLESALE_MARKET,
        QUATTROKI_RESTAURANT
    }
    [SerializeField]
    float walkSpeed;
    public string characterName;
    public clothing.Gender gender;

    public Game game;
    public NavMeshAgent agent;
    
    public Location location;
    public GameObject body;
    public Animator ani;
    public Vector3 aimPoint;
    public Character aimCharacter;
    public DeliveryBox aimDeliveryBox;
    public Character inConversationWith;

    [SerializeField]
    GameObject objHighlight;
    public bool highlight;
    public bool pressed;

    public bool executeWalking;
    public bool executePickingUp;
    public bool executeTalking;


    public bool walk;
    public Location walkInTarget;
    public bool talk;
    public bool walkIn;
    public bool pickUp;
    bool inPickup;
    bool inConversation;

    public bool newAim;

    Coroutine pickupStart;

    public GameObject crowbar;



    public void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        ani = body.GetComponent<Animator>();
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();


        
    }




    public void walkTo(Shop location, bool _walkIn)
    {
        walk = true;

        aimPoint = location.door.transform.position;
        newAim = true;
        if (_walkIn)
        {
            walkIn = true;
            walkInTarget = location.location;
        }
        
    }

    public void walkTo(Character location)
    {
        if (!isCarrying())
        {
            walk = true;

            aimCharacter = location;
            talk = true;

        }
    }

    public void walkTo(Vector3 location)
    {
        walk = true;

        aimPoint = location;
        newAim = true;

    }

    public void doorTriggered(Location loc)
    {
        //Called by door script
        
        if (loc == walkInTarget && walkIn)
        {
            if (isCarrying()) dropDelivery();
            location = walkInTarget;
            walkIn = false;

            agent.speed = 0;

            ani.SetInteger("arms", 5);
            ani.SetInteger("legs", 5);

            newAim = false;

            body.SetActive(false);
            objHighlight.SetActive(false);

        }
    }


    public void walkOut()
    {
        //Character should walk out and completely reappear on the screen
        if (location != Location.OUTSIDE)
        {
            Location current = location;
            location = Location.OUTSIDE;
            walkIn = false;
            if (game.findShop.TryGetValue(current, out Shop result))
            {
                Transform door = result.door.transform;

                agent.enabled = false;

                transform.position = new Vector3(door.position.x, 0, door.position.z);

                agent.enabled = true;


                //agent.Warp(new Vector3(door.position.x, 0, door.position.z));

            }

            walkIn = false;

            stopWalking();
        }

        body.SetActive(true);
    }

    public void Loiter()
    {
        int WhichPoint = UnityEngine.Random.Range(0, game.wayPoints.Count);
        walkTo(game.wayPoints[WhichPoint].transform.position);
    }

    public void invokeConversation(Character c)
    {
        Debug.Log("Conversing with " + c.characterName);
        talk = false;
        inConversation = true;
        inConversationWith = c;

        stopWalking();
        c.receiveConversation(transform.GetComponent<Character>());
        transform.LookAt(c.transform);
    }

    public void receiveConversation(Character c)
    {
        talk = false;
        inConversation = true;
        inConversationWith = c;

        stopWalking();
        transform.LookAt(c.transform);
    }

    public void pickUpDelivery(DeliveryBox Delivery)
    {
        pickUp = true;

        aimPoint = Delivery.transform.position;
        aimDeliveryBox = Delivery;
        newAim = true;
    }

    public bool isCarrying()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "DeliveryBox")
            {
                return true;
            }
        }
        return false;
    }

    public void dropDelivery()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "DeliveryBox")
            {
                transform.GetChild(i).GetComponent<DeliveryBox>().drop();
            }
        }
    }

    void pickupExecute()
    {


        aimDeliveryBox.getPickedUp(transform);

        inPickup = false;
        pickUp = false;
        newAim = false;

    }


    public void Update()
    {
        if (talk)
        {
            if (Vector3.Magnitude(transform.position - aimCharacter.transform.position) > game.minimumDistanceForConversation)
            {
                walkTo(aimCharacter.transform.position);
            }
            else
            {
                invokeConversation(aimCharacter.GetComponent<Character>());
            }
        }
        if (newAim)
        {
            if (walk)
            {

                if (Vector3.Distance(transform.position, aimPoint) > 0.25f)
                {
                    startWalking();
                }

                if (Vector3.Distance(transform.position, aimPoint) < 0.25f)
                {
                    stopWalking();
                }

            }
            if (pickUp && !inPickup)
            {
                if (Vector3.Distance(transform.position, aimPoint) > 1f)
                {
                    startWalking();
                }

                if (Vector3.Distance(transform.position, aimPoint) < 1f)
                {
                    agent.speed = 0;



                    if (!inPickup)
                    {
                        inPickup = true;

                        pickupExecute();
                    }

                }
            }
        }

        if (pressed)
        {
            //Do some click animation maybe??
        }
        else if(highlight)
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

    private void startWalking()
    {
        agent.speed = walkSpeed;
        try
        {
            agent.SetDestination(aimPoint);
        }
        catch (System.Exception)
        { 
            Debug.Log("Set Destination is called from " + gameObject);
            Debug.Log("Character Name: " + characterName);
            throw;
        }
        ani.SetInteger("arms", 1);
        ani.SetInteger("legs", 1);
    }

    private void stopWalking()
    {
        agent.speed = 0;

        ani.SetInteger("arms", 5);
        ani.SetInteger("legs", 5);

        newAim = false;
    }

}
