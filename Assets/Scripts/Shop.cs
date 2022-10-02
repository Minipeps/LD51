using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
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

    private void UpdateButtons()
    {
        ShopButton[] buttons = GetComponentsInChildren<ShopButton>();
        foreach(var button in buttons)
        {
            if (CanAfford(button.price))
                button.Unlock();
        }
    }
}
