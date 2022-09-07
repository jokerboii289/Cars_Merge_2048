using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ElementRelated;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Cars_Merge._Scripts.ControllerRelated
{
    public class CarsController : SingletonInstance<CarsController>
    {
        public List<GameObject> cars;
        [HideInInspector] public Vector3 dir;

        [HideInInspector] public GameObject currentCar;
        public TextMeshProUGUI targetNumText, nextCarNumText;
        
        public Dictionary<int, GameObject> carNumPair = new Dictionary<int, GameObject>();

        private void Start()
        {
            int value = 2;
            currentCar = GetCurrentCar();
            nextCarNumText.text = currentCar.GetComponent<CarElement>().num.ToString();
            for (int i = 0; i < cars.Count; i++)
            {
                carNumPair.Add(value, cars[i]);
                value *= 2;
            }

            targetNumText.text = "16";
        }

        public void SpawnCar(Transform refObj)
        {
            dir = refObj.forward;
            GameObject newcar = Instantiate(currentCar, refObj.position, refObj.rotation);
            currentCar = GetCurrentCar();
            nextCarNumText.text = currentCar.GetComponent<CarElement>().num.ToString();
        }

        public void SetupMergedCar(Transform refObj, int num)
        {
            if(num > 64) return;
            currentCar = carNumPair[num];
            SpawnCar(refObj);
        }
        public GameObject GetCurrentCar()
        {
            return cars[Random.Range(0, cars.Count)];
        }
    }   
}
