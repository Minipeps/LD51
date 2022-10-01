using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public int MAX_VALUE = 10;
    public Image bar;
    public Image bg;

    int progress = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int value)
    {
        progress = value;
        UpdateUI();
    }

    private void UpdateUI()
    {
    }
}
