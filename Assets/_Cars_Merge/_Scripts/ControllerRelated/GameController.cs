using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Cars_Merge._Scripts.ControllerRelated
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Vibration.Init();
        }

        private void OnEnable()
        {
            MainController.GameStateChanged += GameManager_GameStateChanged;
        }
        private void OnDisable()
        {
            MainController.GameStateChanged -= GameManager_GameStateChanged;
        }
        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if(newState==GameState.Create)
            {
                
            }
        }

        private void Update()
        {
             if(Input.GetMouseButtonDown(1))
             {
                 On_RetryButtonClicked();
             }
        }

        public void On_NextButtonClicked()
        {
            if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
            }
            PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
            Vibration.Vibrate(27);
        }

        public void On_RetryButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Vibration.Vibrate(27);
        }
        
    }   
}
