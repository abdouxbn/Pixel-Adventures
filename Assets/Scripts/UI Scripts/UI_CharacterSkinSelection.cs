using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_CharacterSkinSelection : MonoBehaviour
{   
    [Header("Components:")]
    [SerializeField] private Animator skinSelectionAnimator_UI;

    [Space]
    [Header("Buttons:")]    
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private GameObject equipButton;    

    [Space]
    [Header("Skins:")]
    [SerializeField] private int skinID;
    [SerializeField] private bool[] purchasedSkin;
    [SerializeField] private int[] skinPrice;

    [Space]
    [Header("Fruits:")]
    [SerializeField] private TextMeshProUGUI fruitsText;


    private void OnEnable()
    {
        SkinPurchaseEquipManager();
    }

    private void SkinPurchaseEquipManager()
    {   
        purchasedSkin[0] = true;

        for (int i = 1; i < purchasedSkin.Length; i++)
        {
            bool isSkinUnlocked = PlayerPrefs.GetInt($"SkinPurchased{i}") == 1;
            if (isSkinUnlocked)
            {
                purchasedSkin[i] = true;
            }
        }

        fruitsText.text = PlayerPrefs.GetInt("TotalFruitsCollected").ToString();
        //Set value to/opposite of purchasedSkin[SkinID]
        equipButton.SetActive(purchasedSkin[skinID]);
        purchaseButton.SetActive(!purchasedSkin[skinID]);
        // Set each skin price if not purchased
        if (!purchasedSkin[skinID])
        {
            purchaseButton.GetComponentInChildren<TMP_Text>().text = $"Price: {skinPrice[skinID]}";            
        }
        // Set Skin sprite and animation
        skinSelectionAnimator_UI.SetInteger("skinID", skinID);
    }

    public void NextSkin()
    {
        skinID++;
        if (skinID > 3) skinID = 0;
        SkinPurchaseEquipManager();
    }

    public void PreviousSkin()
    {
        skinID--;
        if (skinID < 0) skinID = 3;
        SkinPurchaseEquipManager();
    }

    public void PurchaseSkin()
    {   
        if(HaveEnoughFruits()) 
        {
            PlayerPrefs.SetInt($"SkinPurchased{skinID}", 1);
            purchasedSkin[skinID] = true;
            SkinPurchaseEquipManager();
        }
        else 
        {
            Debug.Log("Not enough money!");
        }
    }

    public void EquipSkin()
    {
        PlayerManager.PlayerManagerInstance.equippedSkinID = skinID;
        Debug.Log("Skin Equipped!");
    }

    public bool HaveEnoughFruits()
    {
        int totalFruits = PlayerPrefs.GetInt("TotalFruitsCollected");

        if (totalFruits > skinPrice[skinID])
        {
            totalFruits -= skinPrice[skinID];
            PlayerPrefs.SetInt("TotalFruitsCollected", totalFruits);
            return true;
        }
        return false;
    }
}
