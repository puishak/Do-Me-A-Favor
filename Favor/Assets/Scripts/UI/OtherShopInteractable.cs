using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherShopInteractable : MonoBehaviour
{
    [SerializeField] IssuePanel issuePanel;

    Shop shop;
    Game game;
    private void Start()
    {
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();

        shop = game.findShop[game.player.location];
        issuePanel.shop = shop;
        issuePanel.updateIssues();
    }



}
