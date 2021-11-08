using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class ShopController : MonoBehaviour
{
    private int RingPrice = 250;
    private int LifePrice = 100;

    [Space(10)]
    public RingList RingList;
    public RingGalleryController RingGalleryController;
    public MainMenuManager MainMenuManager;

    [Header("UI Display items")]
    [Space(10)]
    public GameObject ShopInformationPanel;
    public TextMeshProUGUI ShopInformationText;

    [Space(10)]
    public GameObject PurchaseRingPanel;
    public TextMeshProUGUI RingButtonPriceText;
    public TextMeshProUGUI RingPurchasePriceText;
    public GameObject RingRevealSection;
    public TextMeshProUGUI RingRevealNameText;
    public Image RingRevealImage;

    [Space(10)]
    public GameObject PurchaseLifePanel;
    public TextMeshProUGUI LifeButtonPriceText;
    public TextMeshProUGUI LifePurchasePriceText;

    public void Start()
    {
        RingButtonPriceText.text = RingPrice.ToString();
        RingPurchasePriceText.text = RingPrice.ToString();

        LifeButtonPriceText.text = LifePrice.ToString();
        LifePurchasePriceText.text = LifePrice.ToString();
    }

    public void CheckAvailablePurchases()
    {

    }

    public void BuyRing()
    {
        var availableRings = RingList.Rings.Select(ring => ring.key).Except(UserDataManager.GetUnlockedRings());

        //string debug = "";
        //foreach (string r in availableRings) debug += r + " ";
        //Debug.Log(debug);

        if (availableRings.Count() == 0)
            return;

        var randomRing = availableRings.ElementAt(Random.Range(0, availableRings.Count()));
        Debug.Log(randomRing);

        UserDataManager.LocalUserData.UnlockedRings.Add(randomRing);
        UserDataManager.UpdateUnseenRings(1);
        UserDataManager.UpdatePearls(-RingPrice);

        RingRevealNameText.text = RingNameLocalizer.GetNameByKey(randomRing);
        RingRevealImage.sprite = RingList.GetRenderByKey(randomRing);
        RingRevealSection.SetActive(true);

        RingGalleryController.UpdateRingGallery();
        MainMenuManager.RefreshUserDataValues();
    }

    public void BuyLife()
    {
        UserDataManager.UpdateLives(1);
        UserDataManager.UpdatePearls(-LifePrice);
        MainMenuManager.RefreshUserDataValues();
    }

    public void PurchaseRingAvailable()
    {
        if (UserDataManager.LocalUserData.UnlockedRings.Count >= RingList.Rings.Count)
        {
            ShopInformationText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UIText", "NoMoreRings").Result;
            ShopInformationPanel.SetActive(true);
        }
        else if (UserDataManager.LocalUserData.TotalPearls < RingPrice)
        {
            ShopInformationText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UIText", "NotEnoughPearls").Result;
            ShopInformationPanel.SetActive(true);
        }
        else
            PurchaseRingPanel.SetActive(true);
    }

    public void PurchaseLifeAvailable()
    {
        if (UserDataManager.LocalUserData.TotalPearls < LifePrice)
        {
            ShopInformationPanel.SetActive(true);
            ShopInformationText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UIText", "NotEnoughPearls").Result;
        }
        else
            PurchaseLifePanel.SetActive(true);
    }
}
