using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ownInteractablePanel : MonoBehaviour
{
    [SerializeField] IssuePanel issuePanel;

    Shop shop;
    private void Start()
    {
        shop = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>().player.shop;
        issuePanel.shop = shop;
        issuePanel.updateIssues();
    }

    public void ReferCustomerTo()
    {

    }

    public void GenerateDeliveries()
    {
        shop.GenerateDelivery(1);
    }

    public void HireEmployees()
    {
        shop.HireEmployee();
    }

    public void FireEmployees()
    {
        shop.FireEmployee();
    }
}
