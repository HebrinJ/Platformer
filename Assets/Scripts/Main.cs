using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    public Player player;
    public Text Cointext;
    public Image[] Hearts;
    public Sprite isLife, nonLife;
    public GameObject WinScreen, LoseScreen;
    float timer = 0f;
    public Text timeText;
    public Image HasKeyHUD;
    public Sprite HasKeySprite;
    public SoundEffector soundEffector;
    public AudioSource musicSource, soundSource;
    
    private void Start()
    {
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume")/10;
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume")/10;
    }
    public void ReloadLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Update()
    {
        Cointext.text = player.GetCoins().ToString();

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (player.GetHP() > i)
                Hearts[i].sprite = isLife;
            else Hearts[i].sprite = nonLife;
        }

        timer += Time.deltaTime;
        timeText.text = timer.ToString("F2").Replace(",", ":");

        if (player.GetKey())
        {
            HasKeyHUD.sprite = HasKeySprite;
        }
    }

    public void Win()
    {
        soundEffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);

        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);

        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        else
            PlayerPrefs.SetInt("coins", player.GetCoins());

    }

    public void Lose()
    {
        soundEffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);
    }

    public void MenuLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("Menu");
    }

    public void NextLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        player.enabled = true;
    }

    
    /*public void Debug()
    {
        print ("Its Work!");
    }*/
}
