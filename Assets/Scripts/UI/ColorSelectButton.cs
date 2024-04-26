using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorSelectButton : MonoBehaviour
{
    public Material toApply;
    public GameObject lockDisplay;
    public int cost = 0;

    private TextMeshProUGUI costText;
    private TextMeshProUGUI buyText;

    private GarageUI garageUI;

    private void Awake()
    {
        costText = GameObject.Find("CostText").GetComponent<TextMeshProUGUI>();
        buyText = GameObject.Find("BuyText").GetComponent<TextMeshProUGUI>();
        garageUI = GameObject.Find("GarageUI").GetComponent<GarageUI>();

        if (PlayerPrefs.GetInt(toApply.name) == 1)
            lockDisplay.gameObject.SetActive(false);

        string colorName = toApply.name.Substring(0, toApply.name.Length - 3);
        GetComponentInChildren<TextMeshProUGUI>().SetText(colorName + "\n(" + cost.ToString() + "C)");
    }

    public void OnButtonClick()
    {
        garageUI.selectionRectangle.SetActive(true);
        garageUI.selectionRectangle.transform.SetParent(transform);
        garageUI.selectionRectangle.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);



        int coins = PlayerPrefs.GetInt("player_money");
        bool alreadyBought = PlayerPrefs.GetInt(toApply.name) == 1;
        if (alreadyBought)
        {
            costText.SetText("Already unlocked!");
            buyText.SetText("Set!");
            garageUI.selectedMaterial = toApply;
            garageUI.selectedMatCost = 0;
            return;
        }

        costText.SetText("Cost: " + cost + " coins!");
        if (cost > coins)
        {
            buyText.SetText("Too much");
            garageUI.selectedMaterial = null;
        }
        else
        {
            buyText.SetText("Buy!");
            GameObject.Find("new_car").GetComponent<CarCustomizer>().SwitchMaterial(toApply);
            garageUI.selectedMaterial = toApply;
            garageUI.selectedMatCost = cost;
        }

        //   int coins = PlayerPrefs.GetInt("player_money");
        //   bool alreadyBought = PlayerPrefs.GetInt(toApply.name) == 1;
        //   if (coins < cost && !alreadyBought) 
        //       return;

        //   // Update the coin balance and text
        //   int newBalance = coins;
        //   if (!alreadyBought) 
        //    newBalance = coins - cost;

        //   PlayerPrefs.SetInt("player_money", newBalance);
        //   GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>().SetText("Coins: " + newBalance);

        //   // Unlock the color and hide the lock symbol
        //   GameManager gm = GameManager.GetInstance();
        //   gm.SetCarMaterial(toApply);
        //   gm.UnlockColor(toApply.name);
        //lockDisplay.gameObject.SetActive(false);
    }
}
