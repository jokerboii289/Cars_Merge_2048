using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
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
                PlayCrashEffect();
            }
        }

        void Merge(GameObject otherCar)
        {
            GetComponent<Collider>().enabled = false;
            otherCar.GetComponent<Collider>().enabled = false;
            CarsController.instance.SetupMergedCar(otherCar.transform, _carElement.num * 2);
            GameObject mergeFx = otherCar.GetComponent<CarElement>().mergeFx;
            mergeFx.transform.parent = null;
            mergeFx.SetActive(true);
            gameObject.SetActive(false);
            otherCar.SetActive(false);
        }

        void PlayCrashEffect()
        {
            if(transform.localEulerAngles.y < 85 || transform.localEulerAngles.y < 0)
            {
                float origZPos = transform.localPosition.z;
                transform.DOLocalMoveZ(origZPos - 0.5f, 0.25f).OnComplete(() =>
                {
                    transform.DOLocalMoveZ(origZPos, 0.25f);
                });
            }else if(transform.localEulerAngles.y > 85)
            {
                float origXPos = transform.localPosition.x;
                transform.DOLocalMoveX(origXPos - 0.5f, 0.25f).OnComplete(() =>
                {
                    transform.DOLocalMoveX(origXPos, 0.25f);
                });
            }
            SoundsController.instance.PlaySound(SoundsController.instance.crash);
        }
    }
}