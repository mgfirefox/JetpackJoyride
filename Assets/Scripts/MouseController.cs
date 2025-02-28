using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    [SerializeField]
    private Texture2D coinIconTexture;

    [SerializeField]
    private GameObject checkIfGrounded;
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private ParticleSystem jetpackFlames;

    [SerializeField]
    private float jetpackForce = 30.0f;
    [SerializeField]
    private float velocityX = 10.0f;

    private bool isJetpackActive = false;
    private bool isGrounded = false;
    private bool isDead = false;

    private uint coinsCounter;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isJetpackActive = Input.GetButton("Fire1") && !isDead;

        isGrounded = Physics2D.OverlapCircle(checkIfGrounded.transform.position, 0.1f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        ParticleSystem.EmissionModule jetpackFlamesEmission = jetpackFlames.emission;
        jetpackFlamesEmission.enabled = !isGrounded;
        jetpackFlamesEmission.rateOverTime = isJetpackActive ? 300.0f : 75.0f;
    }

    private void FixedUpdate()
    {
        if (isDead) { return; }

        Vector2 newVelocity = rigidbody2D.velocity;
        newVelocity.x = velocityX;
        rigidbody2D.velocity = newVelocity;

        if (!isJetpackActive) { return; }
        rigidbody2D.AddForce(new Vector2(0, jetpackForce));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Laser": HitByLaser(); break;
            case "Coin": CollectCoin(collision.gameObject); break;
        }
    }

    private void HitByLaser()
    {
        isDead = true;
        animator.SetBool("isDead", isDead);
    }

    private void CollectCoin(GameObject coin)
    {
        coinsCounter++;
        Destroy(coin);
    }

    private void OnGUI()
    {
        ShowCoinsCounter();

        if (isDead && isGrounded)
        {
            ShowRestartButton();
        }
    }

    private void ShowCoinsCounter()
    {
        var coinIcon = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(coinIcon, coinIconTexture);

        var guiStyle = new GUIStyle();
        guiStyle.fontSize = 30;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.normal.textColor = Color.yellow;

        var coinsCounterLabel = new Rect(coinIcon.xMax, coinIcon.y, 60, 32);
        GUI.Label(coinsCounterLabel, coinsCounter.ToString(), guiStyle);
    }

    private void ShowRestartButton()
    {
        var restartButton = new Rect(Screen.width * 0.45f, Screen.height * 0.45f, Screen.width * 0.1f, Screen.height * 0.1f);

        if (GUI.Button(restartButton, "Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
