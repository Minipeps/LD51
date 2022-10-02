using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Item
{
    Wall,
    TurretBase,
    TurretFast,
    TurretHeavy,
    TurretBoss
};

public class Shop : MonoBehaviour
{
    public static int ItemPrice(Item itemType) => itemType switch
    {
        Item.Wall => 10,
        Item.TurretBase => 150,
        Item.TurretFast => 250,
        Item.TurretHeavy => 500,
        Item.TurretBoss => 10000,
    };
    
    public ShopButton selectedButton;

    int bank;

    public int GetBank() => bank;

    public void SetBank(int value)
    {
        bank = value;
        UpdateButtons();
    }

    public void AddToBank(int value)
    {
        bank += value;
        UpdateButtons();
    }
    public bool CanAfford(int price) => price <= bank;

    public bool CanAffordCurrentItem() => CanAfford(selectedButton.price);

    public void SetSelectedItem(ShopButton button)
    {
        selectedButton = button;
    }

    public void BuyItem() => AddToBank(-selectedButton.price);
    public void SellItem() => AddToBank(selectedButton.price);

    public void SellItem(Item itemType) => AddToBank(ItemPrice(itemType));

    private void UpdateButtons()
    {
        ShopButton[] buttons = GetComponentsInChildren<ShopButton>();
        foreach(var button in buttons)
        {
            button.IsAvailable(CanAfford(button.price));
        }
    }
}
