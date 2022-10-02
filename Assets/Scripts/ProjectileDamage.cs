using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damage = 10;
    bool used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BugAI bug;
        if (!used && collision.gameObject.TryGetComponent(out bug))
        {
            used = true;
            bug.Hurt(damage);
            Destroy(gameObject, 0.01f);
        }
    }

}
