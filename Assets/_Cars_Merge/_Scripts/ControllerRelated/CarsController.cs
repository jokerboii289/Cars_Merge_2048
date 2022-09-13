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
        public GameObject mergeFx;

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
            newcar.GetComponent<CarElement>().engineSmoke.SetActive(true);
            currentCar = GetCurrentCar();
            SetCarInUI();
        }

        void SetCarInUI()
        {
            int carNum = currentCar.GetComponent<CarElement>().num;
            nextCarNumText.text = carNum.ToString();
            nextCarImg.sprite = carImgPair[carNum];
            nextCarImg.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-107, 181f), 0.25f).From();
        }
        public void SetupMergedCar(Transform collidingCar, Transform targetCar, int num)
        {
            if(num > 64) return;
            dir = targetCar.forward;

            CarElement carElement = targetCar.GetComponent<CarElement>();
            Vector3 tilePos = carElement.tileOccupied.position;
            
            //calculating spawn rotation
            Vector3 spawnDir = targetCar.GetComponent<CarElement>().tileOccupied.position - collidingCar.GetComponent<CarElement>().tileOccupied.position;
            Vector3 spawnAngle = new Vector3();
            if(spawnDir.z == 0) spawnAngle = Vector3.up * 90;
            else if(spawnAngle.x == 0) spawnAngle = Vector3.zero;
            
            GameObject newcar = Instantiate(carNumPair[num], new Vector3(tilePos.x, targetCar.position.y, tilePos.z), Quaternion.Euler(spawnAngle));
            newcar.GetComponent<Collider>().enabled = false;
            newcar.GetComponent<CarMovementElement>().enabled = false;
            SoundsController.instance.PlaySound(SoundsController.instance.merge);

            float origY = newcar.transform.position.y;
            newcar.transform.DOLocalRotate(Vector3.one * 60, 0.5f).From();
            newcar.transform.DOScale(Vector3.one * 0.3f, 0.5f).From();
            newcar.transform.DOMoveY(newcar.transform.position.y + 2.5f, 0.5f).OnComplete(() =>
            {
                newcar.transform.DOMoveY(origY, 0.5f);
                newcar.GetComponent<Collider>().enabled = true;
                newcar.GetComponent<CarMovementElement>().enabled = true;
            });
            GameObject fx = Instantiate(mergeFx, targetCar.position, Quaternion.identity);
            fx.transform.parent = newcar.transform;
            targetCar.gameObject.SetActive(false);
            if (newcar.GetComponent<CarElement>().num == targetCarNum)
            {
                Material finalCarMat = newcar.transform.GetChild(0).GetComponent<Renderer>().material;
                Color origColor = finalCarMat.color;
                var sequence = DOTween.Sequence();
                sequence.Append(finalCarMat.DOColor(Color.black, 0.5f));
                sequence.Append(finalCarMat.DOColor(origColor, 0.5f));
                sequence.Append(finalCarMat.DOColor(Color.black, 0.5f));
                sequence.Append(finalCarMat.DOColor(origColor, 0.5f));
                
                MainController.instance.SetActionType(GameState.Levelwin);   
            }
        }
        public GameObject GetCurrentCar()
        {
            return cars[Random.Range(0, cars.Count -1 )];
        }
    }   
}
