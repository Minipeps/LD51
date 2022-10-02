using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public float fireRate = 10;
    public int rotationSpeed = 1;
    public int projectileSpeed = 1;
    public int maxRange = 5;

    private GameObject _target;

    public GameObject turretTop;
    public GameObject projectilePrefab;
    public AudioClip[] clips;

    public Item type;

    AudioSource turretAudioSource;

    private float shootingWaitBuffer = 0f;

    private bool hasTarget;

    private List<Vector2> rayDirections;

    void Awake()
    {
        turretAudioSource = GetComponent<AudioSource>();
        rayDirections = new List<Vector2>();
        for (float theta = 0; theta < 360; theta += 15)
        {
            rayDirections.Add(new Vector2(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target && hasTarget)
        {
            var direction = (_target.transform.position - transform.position).normalized;
            turretTop.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - 90f);
        }

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
            hasTarget = false;
            return;
        }

        var direction = (_target.transform.position - transform.position);
        if (direction.magnitude < maxRange)
        {
            direction.Normalize();

            var projectile = Instantiate(projectilePrefab, transform);
            projectile.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - 90f);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);

            // Play sound
            turretAudioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
        else
        {
            hasTarget = false;
        }
    }

    private bool SelectNewTarget()
    {
        foreach( var dir in rayDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxRange, LayerMask.GetMask("Bug"));
            if (hit.collider != null)
            {
                _target = hit.collider.gameObject;
                return true;
            }
        }
        return false;
    }
}
