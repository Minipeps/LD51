using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    Button btn;
    public Text priceTag;

    public int price;

    // Start is called before the first frame update
    void Awake()
    {
        btn = GetComponent<Button>();
        btn.interactable = false;
    }

    public void IsAvailable(bool value)
    {
        btn.interactable = value;
        priceTag.color = value ? Color.white : Color.red;
    }

    public void SetInteractable(bool value)
    {
        btn.interactable = value;
    }
}
