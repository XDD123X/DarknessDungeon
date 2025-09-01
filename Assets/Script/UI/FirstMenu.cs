using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstMenu : MonoBehaviour
{
    // Main menu
    [SerializeField] private GameObject mainMenu;
    // Loading menu
    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private Image loadingBar;

    [SerializeField] private GameObject setting;
    [SerializeField] private Toggle tgMusic;
    [SerializeField] private Slider slMusic;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        setting.SetActive(false);
        loadingMenu.SetActive(false);

        if (!PlayerPrefs.HasKey("EnableMusic"))
        {
            PlayerPrefs.SetInt("EnableMusic", 1);
        }
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 1);
        }
        PlayerPrefs.Save();
        tgMusic.isOn = PlayerPrefs.GetInt("EnableMusic") == 1;
        slMusic.value = PlayerPrefs.GetFloat("Music");
    }

    private void FixedUpdate()
    {
        audioSource.mute = !tgMusic.isOn;
        audioSource.volume = slMusic.value;
    }
    public void PlayGame()
    {
        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        StartCoroutine(onLoading());
    }

    public void Setting()
    {
        tgMusic.isOn = PlayerPrefs.GetInt("EnableMusic") == 1;
        slMusic.value = PlayerPrefs.GetFloat("Music");
        setting.SetActive(true);
    }

    public void OffSetting()
    {
        PlayerPrefs.SetInt("EnableMusic", tgMusic.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Music", slMusic.value);
        PlayerPrefs.Save();
        setting.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator onLoading()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

            loadingBar.fillAmount = progressValue;
            yield return null;
        }
    }
}
