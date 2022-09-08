using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ElementRelated;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Cars_Merge._Scripts.ControllerRelated
{
    public class CarsController : SingletonInstance<CarsController>
    {
        public List<GameObject> cars;
        public List<Sprite> carSprites;
        public Image nextCarImg, targetCarImg;
        
        [HideInInspector] public Vector3 dir;

        [HideInInspector] public GameObject currentCar;
        public TextMeshProUGUI targetNumText, nextCarNumText;
        
        public Dictionary<int, GameObject> carNumPair = new Dictionary<int, GameObject>();
        public Dictionary<int, Sprite> carImgPair = new Dictionary<int, Sprite>();
        public int targetCarNum;

        private void Start()
        {
            int value = 2;
            currentCar = GetCurrentCar();
            for (int i = 0; i < cars.Count; i++)
            {
                carNumPair.Add(value, cars[i]);
                carImgPair.Add(value, carSprites[i]);
                value *= 2;
            }
            SetCarInUI();
            targetNumText.text = targetCarNum.ToString();
            targetCarImg.sprite = carImgPair[targetCarNum];
        }
        
        //By arrow click
        public void SpawnCar(Transform refObj)
        {
            dir = refObj.forward;
            GameObject newcar = Instantiate(currentCar, refObj.position, refObj.rotation);
            currentCar = GetCurrentCar();
            SetCarInUI();
        }

        void SetCarInUI()
        {
            int carNum = currentCar.GetComponent<CarElement>().num;
            nextCarNumText.text = carNum.ToString();
            nextCarImg.sprite = carImgPair[carNum];
        }
        public void SetupMergedCar(Transform refObj, int num)
        {
            if(num > 64) return;
            dir = refObj.forward;
            GameObject newcar = Instantiate(carNumPair[num], refObj.position, refObj.rotation);
            SoundsController.instance.PlaySound(SoundsController.instance.merge);

            float origY = newcar.transform.position.y;
            newcar.transform.DOLocalRotate(Vector3.one * 180, 0.5f).From();
            newcar.transform.DOMoveY(newcar.transform.position.y + 3.5f, 0.5f).OnComplete(() =>
            {
                newcar.transform.DOMoveY(origY, 0.5f);
            });
            
            if(newcar.GetComponent<CarElement>().num == targetCarNum)
                MainController.instance.SetActionType(GameState.Levelwin);
        }
        public GameObject GetCurrentCar()
        {
            return cars[Random.Range(0, cars.Count)];
        }
    }   
}
