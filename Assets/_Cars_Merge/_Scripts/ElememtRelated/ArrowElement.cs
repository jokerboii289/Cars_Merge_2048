using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class ArrowElement : MonoBehaviour
    {
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
            if(newState==GameState.Input)
            {
                _canInstantiate = true;
            }
            if(newState==GameState.Movement)
            {
                _canInstantiate = false;
            }
            if(newState==GameState.Levelwin)
            {
                _canInstantiate = false;
            }
            if(newState==GameState.Levelfail)
            {
                _canInstantiate = false;
            }
        
        }

        private bool _canInstantiate;
        private void OnMouseDown()
        {
            if(!_canInstantiate || MainController.instance.GameState == GameState.Levelwin) return;
            CarsController.instance.SpawnCar(transform);
            MainController.instance.SetActionType(GameState.Movement);
            SoundsController.instance.PlaySound(SoundsController.instance.arrowTap);
            transform.DOScale(0.2f, 0.25f).OnComplete(() =>
            {
                transform.DOScale(0.5f, 0.25f);
            });
        }
    
    }   
}
