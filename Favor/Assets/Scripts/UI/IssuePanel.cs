using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IssuePanel : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject[] issues = new GameObject[8];
    [Header("Text Objects")]
    [SerializeField] TextMeshProUGUI[] texts = new TextMeshProUGUI[8];

    public Shop shop;
    Game game;

    public void OnEnable()
    {
        updateIssues();
    }

    public void updateIssues()
    {
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        foreach (GameObject obj in issues) obj.SetActive(false);

        for (int i = 0; i < Mathf.Min(shop.issues.Count, issues.Length); i++)
        {
            texts[0].SetText(shop.issues[0]);
            issues[0].SetActive(true);
        }
    }

    public void fix(int i)
    {
        shop.fixIssue(i, game.player.GetComponent<Owner>());
        updateIssues();
    }

}
