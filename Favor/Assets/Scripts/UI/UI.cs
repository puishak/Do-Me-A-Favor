using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public CameraController cam;

    [Header("Panels")]
    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject characterPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;

    [Header("Character Panel")]
    [SerializeField] GameObject characterPanelName;
    [SerializeField] RadioButton characterPanelGender;

    [Header("Game Panel")]
    public GameUI gameUI;

    [Header("Pause Panel")]
    [SerializeField] infoLabel pausePanelName;
    [SerializeField] infoLabel pausePanelShopName;
    [SerializeField] infoLabel pausePanelCustomerServed;
    [SerializeField] infoLabel pausePanelMoney;
    [SerializeField] infoLabel pausePanelEmployees;
    [SerializeField] infoLabel pausePanelInventory;
    [SerializeField] infoLabel pausePanelIssues;


    [Header("Game Over Panel")]
    [SerializeField] infoLabel gameOverCustomerServed;


    public enum Option {
        WELCOME_PANEL,
        CHARACTER_PANEL,
        SHOP_PANEL,
        GAME_PANEL,
        PAUSE_PANEL,
        GAME_OVER_PANEL
    }

    public Option[] Options;

    public Option defaultPanel = Option.WELCOME_PANEL;

    public Dictionary<Option, GameObject> getPanel = new Dictionary<Option, GameObject>();


    public Game game;

    void Awake()
    {
        Options = new Option[] { Option.WELCOME_PANEL, Option.CHARACTER_PANEL, Option.SHOP_PANEL, Option.GAME_PANEL, Option.PAUSE_PANEL, Option.GAME_OVER_PANEL};

        getPanel = new Dictionary<Option, GameObject>()
        {
            {Option.WELCOME_PANEL, welcomePanel},
            {Option.CHARACTER_PANEL, characterPanel},
            {Option.SHOP_PANEL, shopPanel},
            {Option.GAME_PANEL, gamePanel},
            {Option.PAUSE_PANEL, pausePanel},
            {Option.GAME_OVER_PANEL, gameOverPanel}
        };
    }

    public void ShowPanel(Option panel)
    {

        Debug.Log(panel);
        //Change Panel
        foreach (Option opt in Options)
        {
            if (opt == panel)
            {
                getPanel[opt].SetActive(true);
            }
            else
            {
                getPanel[opt].SetActive(false);
            }
        }
        //change VCam priority
        switch (panel)
        {
            case Option.WELCOME_PANEL:
                cam.changeCam(CameraController.Option.WELCOME);
                break;
            case Option.CHARACTER_PANEL:
                cam.changeCam(CameraController.Option.CHOOSE_CHARACTER);
                updateCharacterPanel();
                break;
            case Option.SHOP_PANEL:
                cam.changeCam(game.findCam[game.playerShop]);
                break;
            case Option.GAME_PANEL:
                cam.changeCam(CameraController.Option.FOLLOW);
                break;
            case Option.PAUSE_PANEL:
                updatePausePanel();
                break;
            case Option.GAME_OVER_PANEL:
                cam.changeCam(CameraController.Option.GAME_OVER);
                updateGameOverPanel();
                break;
            default:
                break;
        }
    }

    void updateCharacterPanel()
    {
        characterPanelName.GetComponent<TMP_InputField>().text = game.playerName;
        if (game.playerGender == clothing.Gender.MALE) characterPanelGender.Male();
        else characterPanelGender.Female();
    }



    void updatePausePanel()
    {
        pausePanelName.updateValue(game.player.characterName);
        pausePanelShopName.updateValue(game.player.shop.shopName);
        pausePanelCustomerServed.updateValue(game.player.shop.customerServed);
        pausePanelMoney.updateValue(game.player.money);
        pausePanelEmployees.updateValue(game.player.shop.employees.Count);
        pausePanelInventory.updateValue(game.player.shop.inventory);
        pausePanelIssues.updateValue(game.player.shop.issues.Count);
    }

    void updateGameOverPanel()
    {
        gameOverCustomerServed.updateValue(game.player.shop.customerServed);
    }



    public void ExitButton()
    {
        Debug.Log("ExitButton");
        Application.Quit();
    }
    //Welcome Panel Buttons
    public void WelcomePanelBegin()
    {
        Debug.Log("WelcomePanelBegin");
        ShowPanel(Option.CHARACTER_PANEL);
    }

    //Character Panel Buttons
    public void CharacterPanelMale()
    {
        game.playerGender = clothing.Gender.MALE;
        Debug.Log("CharacterPanelMale");
    }
    public void CharacterPanelFemale()
    {
        game.playerGender = clothing.Gender.FEMALE;
        Debug.Log("CharacterPanelFemale");

    }
    public void CharacterPanelRandom()
    {
        game.RandomPlayerNameAndGender();
        updateCharacterPanel();
        Debug.Log("CharacterPanelRandom");

    }
    public void CharacterPanelChangeOutfit()
    {
        game.player.GetComponent<clothing>().changeClothes(game.playerGender);
        Debug.Log("CharacterPanelChangeOutfit");

    }
    public void CharacterPanelNext()
    {
        game.playerName = characterPanelName.GetComponent<TMP_InputField>().text;
        ShowPanel(Option.SHOP_PANEL);
        Debug.Log("CharacterPanelNext");

    }
    public void CharacterPanelBack()
    {
        ShowPanel(Option.WELCOME_PANEL);
        Debug.Log("CharacterPanelBack");

    }

    //Shop Panel Buttons
    public void ShopPanelPrevious()
    {
        game.ChangeShop(false);
        ShowPanel(Option.SHOP_PANEL);
        Debug.Log("ShopPanelPrevious");

    }
    public void ShopPanelForward()
    {
        game.ChangeShop(true);
        ShowPanel(Option.SHOP_PANEL);
        Debug.Log("ShopPanelForward");

    }
    public void ShopPanelStart()
    {
        game.StartGame();
        Debug.Log("ShopPanelStart");

    }
    public void ShopPanelBack()
    {
        ShowPanel(Option.CHARACTER_PANEL);
        Debug.Log("ShopPanelBack");

    }

    //Game Panel Buttons
    public void GamePanelPause()
    {
        game.PauseGame();
        Debug.Log("GamePanelPause");

    }
    
    //Pause Panel Buttons
    public void PausePanelHire()
    {
        game.player.shop.HireEmployee();
        updatePausePanel();
        Debug.Log("PausePanelHire");

    }
    public void PausePanelFire()
    {
        game.player.shop.FireEmployee();
        updatePausePanel();
        Debug.Log("PausePanelFire");

    }
    
    public void PausePanelResume()
    {
        game.ResumeGame();
        Debug.Log("PausePanelResume");

    }
    public void PausePanelQuit()
    {
        Debug.Log("PausePanelQuit");
        game.ResumeGame();
        game.GameOver();

    }

    //Game Over Panel Buttons
    public void GameOverPanelMainMenu()
    {
        Debug.Log("GameOverPanelQuit");
        game.ResetGame();

    }
}
