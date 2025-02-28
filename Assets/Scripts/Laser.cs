using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private new Collider2D collider2D;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite laserOn;
    [SerializeField]
    private Sprite laserOff;

    [SerializeField]
    private float toggleInterval = 0.0f;
    [SerializeField]
    private float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (toggleInterval == 0.0f) { 
            toggleInterval = int.MaxValue; 
        }
        timeUntilNextToggle = toggleInterval;
    }

    private void FixedUpdate()
    {
        timeUntilNextToggle -= Time.fixedDeltaTime;

        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.fixedDeltaTime);

        if (timeUntilNextToggle > 0) { return; }

        isLaserOn = !isLaserOn;
        collider2D.enabled = isLaserOn;

        if (isLaserOn)
        {
            spriteRenderer.sprite = laserOn;
        }
        else
        {
            spriteRenderer.sprite = laserOff;
        }

        timeUntilNextToggle = toggleInterval;
    }
}
