using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BugAI bug;
        if (collision.gameObject.TryGetComponent(out bug))
        {
            bug.Hurt(damage);
            Destroy(gameObject, 0.01f);
        }
    }

}
