using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damage = 10;
    bool used = false;

    private float lifeTime = 0;

    private float MAX_LIFETIME = 10;

    private void Update()
    {
        if (lifeTime > MAX_LIFETIME)
            Destroy(gameObject, 0.01f);
        else
            lifeTime += Time.deltaTime;
    }

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
