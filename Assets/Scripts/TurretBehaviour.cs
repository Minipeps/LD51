using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public int fireRate = 10;
    public int rotationSpeed = 1;
    public int projectileSpeed = 1;
    public int maxRange = 5;

    private GameObject _target;

    public GameObject turretTop;
    public GameObject projectilePrefab;
    public AudioClip[] clips;


    AudioSource turretAudioSource;

    private float shootingWaitBuffer = 0f;

    private bool hasTarget;

    void Awake()
    {
        turretAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingWaitBuffer > 1.0f / fireRate)
        {
            if (!hasTarget)
                hasTarget = SelectNewTarget();

            if (hasTarget)
                Shoot();
            shootingWaitBuffer = 0;
        }
        else
        {
            shootingWaitBuffer += Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (!_target)
        {
            Debug.Log("Lost target");
            hasTarget = false;
            return;
        }

        var direction = (_target.transform.position - transform.position);
        if (direction.magnitude < maxRange)
        {
            direction.Normalize();

            turretTop.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - 90f);
            var projectile = Instantiate(projectilePrefab, transform);
            projectile.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 90f);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);

            // Play sound
            turretAudioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
        else
        {
            Debug.Log("Lost target");
            hasTarget = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //BugAI bug;
        //if (!hasTarget && collision.gameObject.TryGetComponent(out bug))
        //{
        //    Debug.Log("Target aquired");
        //    _target = collision.gameObject;
        //    hasTarget = true;
        //}
    }

    private bool SelectNewTarget()
    {
        List<Vector2> directions = new List<Vector2>() {
            Vector2.up,
            Vector2.up + Vector2.left,
            Vector2.left,
            Vector2.left + Vector2.down,
            Vector2.down,
            Vector2.down + Vector2.right,
            Vector2.right,
            Vector2.right + Vector2.up };
        foreach( var dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxRange, LayerMask.GetMask("Bug"));
            if (hit.collider != null)
            {
                Debug.Log("Target aquired");
                _target = hit.collider.gameObject;
                return true;
            }
        }
        Debug.Log("No target found in range");
        return false;
    }
}
