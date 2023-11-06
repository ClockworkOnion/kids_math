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

    private void Awake()
    {
        if (PlayerPrefs.GetInt(toApply.name) == 1)
		    lockDisplay.gameObject.SetActive(false);


        string colorName = toApply.name.Substring(0, toApply.name.Length - 3);
        GetComponentInChildren<TextMeshProUGUI>().SetText(colorName + "\n(" + cost.ToString() + "C)");
    }

    public void OnButtonClick() {
        int coins = PlayerPrefs.GetInt("player_money");
        bool alreadyBought = PlayerPrefs.GetInt(toApply.name) == 1;
        if (coins < cost && !alreadyBought) 
            return;

        // Update the coin balance and text
        int newBalance = coins;
        if (!alreadyBought) 
	        newBalance = coins - cost;

        PlayerPrefs.SetInt("player_money", newBalance);
        GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>().SetText("Coins: " + newBalance);

        // Unlock the color and hide the lock symbol
        GameManager gm = GameManager.GetInstance();
        gm.SetCarMaterial(toApply);
        gm.UnlockColor(toApply.name);
	    lockDisplay.gameObject.SetActive(false);
    }
}
