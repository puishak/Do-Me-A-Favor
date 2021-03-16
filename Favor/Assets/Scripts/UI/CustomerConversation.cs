using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerConversation : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] string wholesaleMarket;
    [SerializeField] string blackwoodsBar;
    [SerializeField] string coffeeCafe;
    [SerializeField] string pawnShop;
    [SerializeField] string palmSupermarket;
    [SerializeField] string pizzaria;
    [SerializeField] string quattrokiRestaurant;
    [SerializeField] string redFlowers;

    [SerializeField] string reply;

    [Header("Other")]
    [SerializeField] GameObject choosePanel;
    [SerializeField] GameObject conversationPanel;

    [SerializeField] TextMeshProUGUI lbl1;
    [SerializeField] TextMeshProUGUI lbl2;
    GameObject btnDone;

    Customer customer;
    Game game;

    private void Awake()
    {
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        
    }

    public void startConversation (Customer c)
    {
        Time.timeScale = 0f;
        customer = c;
        choosePanel.SetActive(true);
        conversationPanel.SetActive(false);
    }

    public void WholesaleMarket()
    {
        choose(game.WholesaleMarket, wholesaleMarket);
    }

    public void BlackwoodsBar()
    {
        choose(game.BlackwoodsBar, blackwoodsBar);
    }
    public void CoffeeCafe()
    {
        choose(game.CoffeeCafe, coffeeCafe);
    }
    public void PalmSupermarket()
    {
        choose(game.PalmSupermarket, palmSupermarket);
    }
    public void PawnShop()
    {
        choose(game.PawnShop, pawnShop);
    }
    public void Pizzaria()
    {
        choose(game.Pizzaria, pizzaria);
    }
    public void QuattrokiRestaurant()
    {
        choose(game.QuattrokiRestaurant, quattrokiRestaurant);
    }
    public void RedFlowers()
    {
        choose(game.RedFlowers, redFlowers);
    }

    private void choose(Shop shop, string str)
    {
        choosePanel.SetActive(false);
        lbl1.SetText(str);
        lbl2.SetText(reply);
        conversationPanel.SetActive(true);
        customer.Refer(shop, true);
    }
    

    public void ExitConversation()
    {
        Time.timeScale = 1f;
    }

}
