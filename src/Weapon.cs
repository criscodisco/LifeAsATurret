using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Weapon : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] LayerMask hittableLayer;

    [Header("Weapon Particle Systems")]
    [SerializeField] ParticleSystem muzzleFlashL;
    [SerializeField] ParticleSystem muzzleFlashR;
    [SerializeField] ParticleSystem hitExplosion;
    ParticleSystem hitExplosionInstance;

    [Header("Shooting Parameters")]
    [SerializeField] float weaponRange;   
    [SerializeField] float fireRate;
    bool canShoot = true;
    float thresholdTime;

    [Header("Sounds")]
    [SerializeField] private AudioClip weaponFireSound;

    [Header("Camera")]
    Camera mainCam;

    [Header("Animations")]
    public Animator turretRotationAnimation;

    [Header("Enemy")]
    private Enemy enemy;
    private GameObject objectHit;

    [Header("Score System")]
    private int enemyKillCounter = 0;
    private int playerScore = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        mainCam = Camera.main;
        turretRotationAnimation = GetComponent<Animator>();
        // Set lifetime of explosion when raycast hits target
        hitExplosion.startLifetime = .5f;
    }

    void Start()
    {
        // Lock cursor within game window (Allows complete control of turret with mouse in WebGL Build)
        Cursor.lockState = CursorLockMode.Locked;

        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        // Quit game
        if (Input.GetKeyDown("escape") || Input.GetKeyDown("q"))
        {
            Application.Quit();
        }
        // Checks to see if enough time has elapsed for player to fire weapon again
        if (thresholdTime < Time.time)
        {
            canShoot = true;
            thresholdTime = Time.time + fireRate;
        }
        else
        {
            canShoot = false;
        }

        // Fires weapon if left mouse button or right trigger on Gamepad is pressed
        if ((Input.GetMouseButton(0) || Mathf.Abs(Input.GetAxis("XBOX_RT")) > 0.5f) && canShoot)
        {
            Shoot();
        }
        else
        {
            // Stops turret animation if player stops firing
            turretRotationAnimation.SetBool("gunFiring", false);
        }

        // Keep track of player score and update it on the UI
        playerScore = enemyKillCounter * 100;
        scoreText.text = playerScore.ToString();     
    }

    private void Shoot()
    {
        HandleMuzzleFlash();
        HandleGunAnimation();
        FireWeaponSound();
        HandleRaycast(); 
    }

    private void HandleRaycast()
    {
        // Enter the following condition if Raycast from player weapon hits target
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, weaponRange, hittableLayer))
        {   
            // Create explosion on target if raycast hits it successfully
            hitExplosionInstance = Instantiate(hitExplosion, hit.point + hit.normal, Quaternion.LookRotation(hit.normal));
            if (hit.transform.gameObject != null)
            {
                // Fracture asteroid and increment kill counter
                hit.transform.gameObject.GetComponent<Fracture>().FractureObject();
                enemyKillCounter++;
            }
        }
    }

    private void HandleMuzzleFlash()
    {   // Play the animations for the left and right guns on turret   
        muzzleFlashL.Play();   
        muzzleFlashR.Play();
    }

    void FireWeaponSound()
    {
        // Play fire weapon sound
        AudioSource.PlayClipAtPoint(weaponFireSound, transform.position);
    }
   
    void HandleGunAnimation()
    {
        // Play gun rotation on turret animation
        turretRotationAnimation.SetBool("gunFiring", true);
    }    
}
