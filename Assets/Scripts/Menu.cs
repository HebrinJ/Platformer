using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button[] Lvls;
    public Text coinText;
    public Slider musicSlider, soundSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Lvl"))
            for (int i = 0; i < Lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                    Lvls[i].interactable = true;
                else
                    Lvls[i].interactable = false;
            }

        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetInt("MusicVolume", 5);
        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetInt("SoundVolume", 5);

        musicSlider.value = PlayerPrefs.GetInt("MusicVolume");
        soundSlider.value = PlayerPrefs.GetInt("SoundVolume");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";
    
    PlayerPrefs.SetInt("MusicVolume", (int)musicSlider.value);
    PlayerPrefs.SetInt("SoundVolume", (int)soundSlider.value);
    }

    

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);

    }

    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Close()
    {
        Application.Quit();
    }
}
///