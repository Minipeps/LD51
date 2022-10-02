using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBehaviour : MonoBehaviour
{
    int health = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetHealth()
    {
        health = 1000;
    }

    public int GetHealth()
    {
        return health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BugAI bug;
        if (collision.gameObject.TryGetComponent(out bug))
        {
            DamageCore(bug.attack);
            Destroy(bug.gameObject, 0.1f);
        }
    }

    private void DamageCore(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }
}
