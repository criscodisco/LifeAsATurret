using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    [Header("Player Health")]
    public int lives = 10;
    public Slider healthBar;

    [Header("Player Game Objects")]
    public GameObject player;
    public GameObject weapon;

    [Header("Game Over")]
    [SerializeField] public TextMeshProUGUI gameOverText;

    [Header("Animations")]
    [SerializeField] ParticleSystem finalGroundExplosion;
    private ParticleSystem finalGroundExplosionInstance;

    // Base takes damage
    public void Damage()
    {
        // Subtract a life when asteroid hits base and update this value on the health bar
        lives--;
        healthBar.value = lives;

        // Enter this condition if game is over
        if (lives <= 0)
        {
            // Create references to player and weapon Game Object tags
            player = GameObject.FindWithTag("Player");
            weapon = GameObject.FindWithTag("GunTag");

            // Set Game Over text to active
            gameOverText.gameObject.SetActive(true);

            // Disable scripts that provide player control
            player.GetComponent<FirstPersonController>().enabled = false;
            weapon.GetComponent<Weapon>().enabled = false;

            // Create the fire around player when game is over
            finalGroundExplosionInstance = Instantiate(finalGroundExplosion, player.transform.position + player.transform.forward * 2 + player.transform.up, Quaternion.identity);

            // Invoke ReloadLevel Method
            Invoke("ReloadLevel", 8.0f);
        }
    }

    public void ReloadLevel()
    {
        // Reload Level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
