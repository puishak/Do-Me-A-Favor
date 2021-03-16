using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Player Info Panel")]
    [SerializeField] infoLabel playerCustomerServed;
    [SerializeField] infoLabel playerMoney;
    [SerializeField] infoLabel playerEmployees;
    [SerializeField] infoLabel playerInventory;
    [SerializeField] infoLabel playerIssues;


    [Header("Hover Panels")]
    [SerializeField] GameObject shopInfoPanel;
    [SerializeField] GameObject ownerInfoPanel;
    [SerializeField] GameObject customerInfoPanel;
    [SerializeField] GameObject employeeInfoPanel;
    [SerializeField] GameObject deliveryBoxInfoPanel;

    [Header("Shop Info Panel")]
    [SerializeField] infoLabel shopPanelshopName;
    [SerializeField] infoLabel shopPanelMoney;
    [SerializeField] infoLabel shopPanelEmployees;
    [SerializeField] infoLabel shopPanelInventory;
    [SerializeField] infoLabel shopPanelOwner;
    [SerializeField] infoSlider shopPanelRelationship;

    [Header("Owner Info Panel")]
    [SerializeField] infoLabel ownerPanelName;
    [SerializeField] infoLabel ownerPanelShopName;
    [SerializeField] infoSlider ownerPanelRelationship;

    [Header("Customer Info Panel")]
    [SerializeField] infoLabel customerPanelName;

    [Header("Employee Info Panel")]
    [SerializeField] infoLabel employeePanelName;
    [SerializeField] infoLabel employeePanelShop;

    [Header("Delivery Box Info Panel")]
    [SerializeField] infoLabel deliveryBoxPanelShop;

    [Header("Interactable panels")]
    [SerializeField] GameObject ownShopInteractable;
    [SerializeField] GameObject otherShopInteractable;

    [Header("Issue Panels")]
    [SerializeField] IssuePanel ownShopIssues;
    [SerializeField] IssuePanel otherShopIssues;



    Game game;

    GameObject[] hoverPanels;
    GameObject[] interactablePanels;

    void Start()
    {
        hoverPanels = new GameObject[] { shopInfoPanel, ownerInfoPanel, customerInfoPanel, employeeInfoPanel, deliveryBoxInfoPanel };
        interactablePanels = new GameObject[] { ownShopInteractable, otherShopInteractable};
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
    }

    
    //Hover Panels
    private void showHoverPanel(GameObject panel = null)
    {
        foreach (GameObject pnl in hoverPanels)
        {
            if (pnl == panel)
            {
                pnl.SetActive(true);
            }
            else
            {
                pnl.SetActive(false);
            }
        }
    }


    public void showHoverShop(Shop shop)
    {
        shopPanelshopName.updateValue(shop.shopName);
        shopPanelRelationship.updateValue(shop.owner.relationship);
        shopPanelOwner.updateValue(shop.owner.characterName);
        shopPanelMoney.updateValue(shop.owner.money);
        shopPanelInventory.updateValue(shop.inventory);
        shopPanelEmployees.updateValue(shop.employees.Count);
        showHoverPanel(shopInfoPanel);
    }

    public void showHoverOwner(Owner owner)
    {
        ownerPanelName.updateValue(owner.characterName);
        ownerPanelRelationship.updateValue(owner.relationship);
        ownerPanelShopName.updateValue(owner.shop.shopName);
        showHoverPanel(ownerInfoPanel);
    }

    public void showHoverCustomer(Customer customer)
    {
        customerPanelName.updateValue(customer.characterName);
        showHoverPanel(customerInfoPanel);
    }

    public void showHoverEmployee(Employee employee)
    {
        employeePanelName.updateValue(employee.characterName);
        employeePanelShop.updateValue(employee.shop.shopName);
        showHoverPanel(employeeInfoPanel);
    }

    public void showHoverDeliveryBox(DeliveryBox box)
    {
        deliveryBoxPanelShop.updateValue(box.shop.shopName);
        showHoverPanel(deliveryBoxInfoPanel);
    }
    public void showHoverNothing()
    {
        showHoverPanel();
    }

    //interactable Panels
    private void showInteractablePanel(GameObject panel = null)
    {
        foreach (GameObject pnl in interactablePanels)
        {
            if (pnl == panel)
            {
                pnl.SetActive(true);
            }
            else
            {
                pnl.SetActive(false);
            }
        }
    }

    public void showInteractableOwnShop()
    {
        showInteractablePanel(ownShopInteractable);
        
    }

    public void showInteractableOtherShop(Shop shop)
    {
        showInteractablePanel(otherShopInteractable);
    }

    public void showInteractableNothing()
    {
        showInteractablePanel();
    }

    void Update()
    {
        
        playerCustomerServed.updateValue(game.player.shop.customerServed);
        playerMoney.updateValue(game.player.money);
        playerEmployees.updateValue(game.player.shop.employees.Count);
        playerInventory.updateValue(game.player.shop.inventory);
        playerIssues.updateValue(game.player.shop.issues.Count);
    }
}
