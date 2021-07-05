using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManagers
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject aboutPanel;
        
        [SerializeField] private Button playButton;
        [SerializeField] private Button aboutButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private Button exitButton;

        
        private void Start()
        {
            mainMenuPanel.SetActive(true);
            aboutPanel.SetActive(false);
            
            playButton.onClick.AddListener(PlayDemo);
            aboutButton.onClick.AddListener(OpenAboutPanel);
            backToMainMenuButton.onClick.AddListener(CloseAboutPanel);
            exitButton.onClick.AddListener(Exit);
        }

        private void PlayDemo()
        {
            SceneManager.LoadScene("SampleScene");
        }

        private void OpenAboutPanel()
        {
            mainMenuPanel.SetActive(false);
            aboutPanel.SetActive(true);
        }

        private void CloseAboutPanel()
        {
            mainMenuPanel.SetActive(true);
            aboutPanel.SetActive(false);
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}
