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

    private void Awake()
    {
        purchasedSkin[0] = true;

        fruitsText.text = PlayerPrefs.GetInt("TotalFruitsCollected").ToString();
    }



    private void SkinPurchaseEquipManager()
    {   
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
        purchasedSkin[skinID] = true;
        SkinPurchaseEquipManager();
    }

    public void EquipSkin()
    {
        PlayerManager.PlayerManagerInstance.equippedSkinID = skinID;
        Debug.Log("Skin Equipped!");
    }
}
