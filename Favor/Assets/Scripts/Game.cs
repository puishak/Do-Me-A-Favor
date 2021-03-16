using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool debugMode = false;
    public bool gameRunning;

    //Shop references
    [Header("Shops")]
    public Shop WholesaleMarket;
    public Shop BlackwoodsBar;
    public Shop CoffeeCafe;
    public Shop PalmSupermarket;
    public Shop PawnShop;
    public Shop Pizzaria;
    public Shop QuattrokiRestaurant;
    public Shop RedFlowers;


    //Player Reference
    [Header("Player")]
    public Owner player;

    //UI and Camera reference
    [Header("Game Play Elements")]
    public UI ui;
    public CameraController cam;

    //Gameplay variables
    [Header("Gameplay Variables")]
    public int startingMoney;
    public int AImoneyPerEmployee = 300;
    public int costPerFix = 50;
    public float minimumDistanceForConversation;
    public float maxCustomerWillingness;
    public float decreaseWillingnessOnReference;
    public float decreaseWillingnessOnNewAim;
    public int maxUnitBuy;
    public int deliverySize = 5;
    public int initMaxRelationship = 50;
    public int maxRelationship = 100;
    public int increaseRelationshipOnReferredCustomer = 5;
    public int increaseRelationshipOnFavor = 20;
    public int decreaseRelationshipOnDeclinedFavor = -15;
    public int maxCustomerAllowed = 50;
    public int customerGenerationInterval = 2;
    public int numInitCustomer = 10;
    public float issueChance = 0.05f;

    //Critical Lists and Arrays
    [Header("Lists and Arrays")]
    public List<string> possibleIssues = new List<string>();
    public Dictionary<Character.Location, Shop> findShop;
    public Dictionary<Shop, CameraController.Option> findCam;
    public Shop[] shops;
    public List<GameObject> wayPoints = new List<GameObject>();
    public List<string> maleNames = new List<string>();
    public List<string> femaleNames = new List<string>();

    //Prefabs
    [Header("Prefabs")]
    public GameObject customerPrefab;
    public GameObject employeePrefab;
    public GameObject ownerPrefab;
    public GameObject playerPrefab;
    public GameObject deliveryBoxPrefab;

    //ParentFolders
    [Header("Prefab Parents")]
    public GameObject customerParent;
    public GameObject employeeParent;
    public GameObject ownerParent;
    public GameObject deliveryBoxParent;

    //Player Info
    [Header("Player Info")]
    public clothing.Gender playerGender;
    public string playerName = "Sam Robinson";
    public Shop playerShop;

    void Awake()
    { 
        //Initiate all the critical lists and arrays
        findShop = new Dictionary<Character.Location, Shop>()
        {
            { Character.Location.WHOLESALE_MARKET, WholesaleMarket},
            { Character.Location.COFFEE_CAFE, CoffeeCafe},
            { Character.Location.BLACKWOODS_BAR, BlackwoodsBar},
            { Character.Location.PALM_SUPERMARKET, PalmSupermarket},
            { Character.Location.PAWN_SHOP, PawnShop},
            { Character.Location.PIZZARIA, Pizzaria},
            { Character.Location.QUATTROKI_RESTAURANT, QuattrokiRestaurant},
            { Character.Location.RED_FLOWERS, RedFlowers},
            { Character.Location.OUTSIDE, null},
    };
        findCam = new Dictionary<Shop, CameraController.Option>()
        {
            {WholesaleMarket, CameraController.Option.WHOLESALE_MARKET },
            {BlackwoodsBar, CameraController.Option.BLACKWOODS_BAR },
            {CoffeeCafe, CameraController.Option.COFFEE_CAFE},
            {PalmSupermarket, CameraController.Option.PALM_SUPERMARKET},
            {PawnShop, CameraController.Option.PAWN_SHOP },
            {Pizzaria, CameraController.Option.PIZZARIA},
            {QuattrokiRestaurant, CameraController.Option.QUATTROKI_RESTAURANT},
            {RedFlowers, CameraController.Option.RED_FLOWERS },
        };
        
        shops = new Shop[] { WholesaleMarket, BlackwoodsBar, CoffeeCafe, PalmSupermarket, PawnShop, Pizzaria, QuattrokiRestaurant, RedFlowers };
        GameObject[] waypointsFind = GameObject.FindGameObjectsWithTag("waypoint");


        foreach (GameObject g in waypointsFind)
        {
            wayPoints.Add(g);
        }

        //Find the Prefabs
    }

    void Start()
    {

        //Reset All variables


        //
        gameRunning = false;
        int whichShop = UnityEngine.Random.Range(0, shops.Length);
        playerShop = shops[whichShop];
        RandomPlayerNameAndGender();
        player.GetComponent<clothing>().changeClothes(playerGender);


        if (debugMode)
        {
            StartGame();
        }
        else
        {
            ui.ShowPanel(ui.defaultPanel);
        }
    }

    void Update()
    {
        if (gameRunning)
        {
            if (player.money < 0 || player.shop.issues.Count >= 10)
            {
                GameOver();

            }
            else
            {
                if (player.location == player.shop.location)
                {
                    ui.gameUI.showInteractableOwnShop();
                }
                else if (player.location != Character.Location.OUTSIDE)
                {
                    ui.gameUI.showInteractableOtherShop(findShop[player.location]);
                }
                else
                {
                    ui.gameUI.showInteractableNothing();
                }
                cam.showLocation(player.location);
            }
        }
    }
    public void changePlayerOutfit(clothing.Gender gender)
    {
        //change outfit of the player using the clothing 
        player.transform.GetComponent<clothing>().changeClothes(gender);
        Debug.Log("Changing Outfit....");
    }

    public void ChangeShop(bool dir)
    {

        int i = 0;
        while (i < shops.Length && shops[i] != playerShop)
        {
            i++;
        }

        if (dir && i >= shops.Length - 1)
        {
            playerShop = shops[0];
        }
        else if (!dir && i <= 0)
        {
            playerShop = shops[shops.Length - 1];
        }
        else if (dir)
        {
            playerShop = shops[i + 1];
        }
        else
        {
            playerShop = shops[i - 1];
        }
    }

    IEnumerator CustomerGenerator()
    {
        while (true)
        {
            if (GameObject.FindGameObjectsWithTag("Customer").Length < maxCustomerAllowed) 
                generateNewCustomer();
            yield return new WaitForSeconds(customerGenerationInterval);
        }
    }

    void generateNewCustomer()
    {
        clothing.Gender gender;
        if (UnityEngine.Random.value < 0.5) gender = clothing.Gender.MALE;
        else gender = clothing.Gender.FEMALE;
        string name = RandomNameGenerator(gender);

        int WhichPoint = UnityEngine.Random.Range(0, wayPoints.Count);
        Transform t = wayPoints[WhichPoint].transform;
        Vector3 position = new Vector3(t.position.x, 0, t.position.z);
        Quaternion rotation = t.rotation;
        
        GameObject gm = Instantiate(customerPrefab, position, rotation, customerParent.transform);
        

        Customer c = gm.GetComponent<Customer>();
        c.characterName = name;
        c.location = Character.Location.OUTSIDE;
        c.gender = gender;

        gm.GetComponent<clothing>().changeClothes(gender);
    }

    public void RandomPlayerNameAndGender()
    {
        if (UnityEngine.Random.value < 0.5)
        {
            playerGender = clothing.Gender.MALE;
        }
        else
        {
            playerGender = clothing.Gender.FEMALE;
        }

        playerName = RandomNameGenerator(playerGender);
    }

    public string RandomNameGenerator(clothing.Gender gender)
    {
        string output;
        var names = new List<string>();
        if (gender == clothing.Gender.MALE)
        {
            names = maleNames;
        }
        else
        {
            names = femaleNames;
        }

        int whichName = UnityEngine.Random.Range(0, names.Count);
        output = names[whichName];

        return output;
    }


    public void GameOver()
    {
        //Disable player controls
        player.GetComponent<PlayerControls>().enabled = false;
        Debug.Log("Game Over");
        ui.ShowPanel(UI.Option.GAME_OVER_PANEL);
        StopCoroutine(CustomerGenerator());
        
        gameRunning = false;
    }
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        
        //Enable Scripts
        player.GetComponent<PlayerControls>().enabled = true;
        foreach (Shop s in shops)
        {
            s.enabled = true;
        }

        //populate the city with customers
        for (int i = 0; i < numInitCustomer; i++)
        {
            generateNewCustomer();
        }
        StartCoroutine(CustomerGenerator());

        player.characterName = playerName;
        player.gender = playerGender;
        player.money = startingMoney;
        player.shop = playerShop;
        player.shop.owner = player;


        ui.ShowPanel(UI.Option.GAME_PANEL);
        gameRunning = true;
    }
    public void ResetGame()
    {
        Start();
    }
    public void PauseGame()
    {
        Debug.Log("Pausing Game...");
        ui.ShowPanel(UI.Option.PAUSE_PANEL);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Debug.Log("Resuming Game...");
        ui.ShowPanel(UI.Option.GAME_PANEL);
        Time.timeScale = 1f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
