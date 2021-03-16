using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Game game;
    public string shopName;
    public Character.Location location;
    public Owner owner;
    public Shop referTo;
    public GameObject door;
    public int inventory = 50;
    public int customerServed = 0;
    public List<string> issues = new List<string>();

    int profitPerUnit = 5;
    int costPerUnit = 3;
    float issueChance = 0.2f;

    public Employee atService;

    public List<Employee> employees = new List<Employee>();
    public List<DeliveryBox> outstandingDeliveries = new List<DeliveryBox>();
    public List<DeliveryBox> activeDeliveries = new List<DeliveryBox>();


    void Awake()
    {
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
    }

    void Start()
    {
        StartCoroutine(GenerateIssues());
        StartCoroutine(PayEmployees());
        CreateOwner();
        HireEmployee();

    }

    private void CreateOwner()
    {
        if (owner == null)
        {
            clothing.Gender gender;
            if (UnityEngine.Random.value < 0.5) gender = clothing.Gender.MALE;
            else gender = clothing.Gender.FEMALE;
            string name = game.RandomNameGenerator(gender);

            int WhichPoint = UnityEngine.Random.Range(0, game.wayPoints.Count);
            Transform t = game.wayPoints[WhichPoint].transform;
            Vector3 position = new Vector3(t.position.x, 0, t.position.z);
            Quaternion rotation = t.rotation;

            GameObject gm = Instantiate(game.ownerPrefab, position, rotation, game.ownerParent.transform);


            owner = gm.GetComponent<Owner>();
            owner.characterName = name;
            owner.location = Character.Location.OUTSIDE;
            owner.gender = gender;
            owner.shop = this;

            gm.GetComponent<clothing>().changeClothes(gender);

        }
    }

    public void HireEmployee()
    {

        clothing.Gender gender;
        if (UnityEngine.Random.value < 0.5) gender = clothing.Gender.MALE;
        else gender = clothing.Gender.FEMALE;
        string name = game.RandomNameGenerator(gender);

        int WhichPoint = UnityEngine.Random.Range(0, game.wayPoints.Count);
        Transform t = game.wayPoints[WhichPoint].transform;
        Vector3 position = new Vector3(t.position.x, 0, t.position.z);
        Quaternion rotation = t.rotation;

        GameObject gm = Instantiate(game.employeePrefab, position, rotation, game.employeeParent.transform);

        
        Employee employee = gm.GetComponent<Employee>();
        employee.characterName = name;
        employee.location = Character.Location.OUTSIDE;
        employee.gender = gender;
        employee.shop = this;

        gm.GetComponent<clothing>().changeClothes(gender);

        employees.Add(employee);
    }

    public void FireEmployee()
    {
        if (employees.Count > 0)
        {
            Employee toBeFired = employees[employees.Count - 1];
            if (atService == toBeFired)
            {
                StopService();
            }
            employees.Remove(toBeFired);
            toBeFired.getFired();
        }
    }

    public bool ReceiveCustomer(int unitBaught, Customer customer, bool referredByPlayer)
    {
        if (atService != null && inventory > unitBaught)
        {
            if (referredByPlayer && game.player.shop != this)
            {
                owner.relationship += game.increaseRelationshipOnReferredCustomer;
            }
            
            inventory -= unitBaught;
            owner.ChangeMoney(unitBaught * profitPerUnit);

            if (referTo != null) customer.Refer(referTo, game.player.shop == this);
            
            customerServed++;
            return true;
            
        }
        else
        {
            return false;
        }
    }

    public void GenerateDelivery(int amount)
    {
        
        for (int i = 0; i < amount; i++)
        {
            if (GameObject.FindGameObjectsWithTag("DeliveryPoint") != null)
            {
                GameObject gm = Instantiate(game.deliveryBoxPrefab, game.deliveryBoxParent.transform);
                activeDeliveries.Add(gm.GetComponent<DeliveryBox>());
                gm.GetComponent<DeliveryBox>().shop = this;
            
                owner.ChangeMoney(-game.deliverySize * costPerUnit);
            }
            else
            {
                Debug.Log("Out of delivery points");
            }
        }

    }

    public void ReceiveDelivery(DeliveryBox delivery)
    {
        inventory += game.deliverySize;

        foreach(DeliveryBox d in activeDeliveries)
        {
            if(delivery == d)
            {
                DeliveryDone(d);
                return;
            }
        }

    }

    public void StartService(Employee emp)
    {
        if (employees.Contains(emp))
        {

            if (atService != null)
            {
                StopService();
            }
            atService = emp;
        }
    }

    public void StopService()
    {
        atService.stopService();
        atService = null;
    }

    public void StartDelivery(DeliveryBox delivery)
    {
        if (outstandingDeliveries.Contains(delivery))
        {
            outstandingDeliveries.Remove(delivery);
            if (!activeDeliveries.Contains(delivery))
            {
                activeDeliveries.Add(delivery);
            }
        }
    }

    public void DropDelivery(DeliveryBox delivery)
    {
        if (activeDeliveries.Contains(delivery))
        {
            activeDeliveries.Remove(delivery);
            if (!outstandingDeliveries.Contains(delivery))
            {
                outstandingDeliveries.Add(delivery);
            }
        }
    }

    public void DeliveryDone(DeliveryBox delivery)
    {
        if (outstandingDeliveries.Contains(delivery))
        {
            outstandingDeliveries.Remove(delivery);
        }
        if (activeDeliveries.Contains(delivery))
        {
            activeDeliveries.Remove(delivery);
        }
    }

    IEnumerator PayEmployees()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);

            if (employees.Count > 0)
            {
                foreach(Employee emp in employees)
                {
                    owner.ChangeMoney(-emp.sallaryPerMinute);
                }
            }

        }
    }

    public void fixIssue(int index, Owner o)
    {
        if (issues[index] != null)
        {
            issues.RemoveAt(index);
            o.ChangeMoney(game.costPerFix);
        }
    }

    IEnumerator GenerateIssues()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            if (UnityEngine.Random.value <= game.issueChance)
            {
                //set these active after adding possible issues
                int whichIssue = UnityEngine.Random.Range(0, game.possibleIssues.Count);
                issues.Add(game.possibleIssues[whichIssue]);
            }

        }
    }

    [SerializeField]
    GameObject objHighlight;
    public bool highlight;
    public bool pressed;
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

}
