using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class CarMovementElement : MonoBehaviour
    {
        private Vector3 _moveDir;
        private Rigidbody _rb;
        public float moveSpeed;

        private CarElement _carElement;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _carElement = GetComponent<CarElement>();
            _moveDir = CarsController.instance.dir;
        }

        void FixedUpdate()
        {
            _rb.position += _moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("car"))
            {
                if (_carElement.num != other.gameObject.GetComponent<CarElement>().num)
                {
                    moveSpeed = 0;
                    _rb.isKinematic = true;
                    MainController.instance.SetActionType(GameState.Input);
                    gameObject.layer = 3;
                }
                else if(_carElement.num == other.gameObject.GetComponent<CarElement>().num && other.gameObject.layer == 3)
                    Merge(other.gameObject);
            }

            if (other.gameObject.CompareTag("wall"))
            {
                moveSpeed = 0;
                _rb.isKinematic = true;
                MainController.instance.SetActionType(GameState.Input);
                gameObject.layer = 3;
            }
        }

        void Merge(GameObject otherCar)
        {
            GetComponent<Collider>().enabled = false;
            otherCar.GetComponent<Collider>().enabled = false;
            CarsController.instance.SetupMergedCar(transform, _carElement.num * 2);
            gameObject.SetActive(false);
            otherCar.SetActive(false);
        }
    }
}