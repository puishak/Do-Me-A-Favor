using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera Follow;
    [SerializeField]
    CinemachineVirtualCamera Welcome;
    [SerializeField]
    CinemachineVirtualCamera ChooseCharacter;
    [SerializeField]
    CinemachineVirtualCamera GameOver;
    [SerializeField]
    CinemachineVirtualCamera WholesaleMarket;
    [SerializeField]
    CinemachineVirtualCamera BlackwoodsBar;
    [SerializeField]
    CinemachineVirtualCamera CoffeeCafe;
    [SerializeField]
    CinemachineVirtualCamera PalmSupermarket;
    [SerializeField]
    CinemachineVirtualCamera PawnShop;
    [SerializeField]
    CinemachineVirtualCamera Pizzaria;
    [SerializeField]
    CinemachineVirtualCamera QuattrokiRestaurant;
    [SerializeField]
    CinemachineVirtualCamera RedFlowers;

    public enum Option 
    {
        FOLLOW,
        WELCOME,
        CHOOSE_CHARACTER,
        GAME_OVER,
        WHOLESALE_MARKET,
        BLACKWOODS_BAR,
        COFFEE_CAFE,
        PALM_SUPERMARKET,
        PAWN_SHOP,
        PIZZARIA,
        QUATTROKI_RESTAURANT,
        RED_FLOWERS
    }

    //public Option chooseCam;

    int ON = 20;
    int OFF = 10;

    Dictionary<Option, CinemachineVirtualCamera> findCam;

    // Start is called before the first frame update
    void Awake()
    {
        findCam = new Dictionary<Option, CinemachineVirtualCamera>()
        {
            {Option.FOLLOW, Follow },
            {Option.WELCOME, Welcome},
            {Option.CHOOSE_CHARACTER, ChooseCharacter},
            {Option.GAME_OVER, GameOver},
            {Option.WHOLESALE_MARKET, WholesaleMarket},
            {Option.BLACKWOODS_BAR, BlackwoodsBar},
            {Option.COFFEE_CAFE, CoffeeCafe},
            {Option.PALM_SUPERMARKET, PalmSupermarket},
            {Option.PAWN_SHOP, PawnShop},
            {Option.PIZZARIA, Pizzaria},
            {Option.QUATTROKI_RESTAURANT, QuattrokiRestaurant},
            {Option.RED_FLOWERS, RedFlowers}
        };
    }


    public void changeCam(Option c)
    {

        Debug.Log(c);
        CinemachineVirtualCamera v = findCam[c];

        foreach (CinemachineVirtualCamera vcam in findCam.Values)
        {
            if (vcam == v)
            {
                vcam.Priority = ON;
            }
            else
            {
                vcam.Priority = OFF;
            }
        }
    }

    public void showLocation(Character.Location loc)
    {
        switch (loc)
        {
            case Character.Location.OUTSIDE:
                changeCam(Option.FOLLOW);
                break;
            case Character.Location.RED_FLOWERS:
                changeCam(Option.RED_FLOWERS);
                break;
            case Character.Location.COFFEE_CAFE:
                changeCam(Option.COFFEE_CAFE);
                break;
            case Character.Location.PALM_SUPERMARKET:
                changeCam(Option.PALM_SUPERMARKET);
                break;
            case Character.Location.PAWN_SHOP:
                changeCam(Option.PAWN_SHOP);
                break;
            case Character.Location.BLACKWOODS_BAR:
                changeCam(Option.BLACKWOODS_BAR);
                break;
            case Character.Location.PIZZARIA:
                changeCam(Option.PIZZARIA);
                break;
            case Character.Location.WHOLESALE_MARKET:
                changeCam(Option.WHOLESALE_MARKET);
                break;
            case Character.Location.QUATTROKI_RESTAURANT:
                changeCam(Option.QUATTROKI_RESTAURANT);
                break;
            default:
                break;
        }
    }

}
