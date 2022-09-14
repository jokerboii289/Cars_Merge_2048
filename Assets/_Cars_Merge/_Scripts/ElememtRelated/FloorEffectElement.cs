using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class FloorEffectElement : MonoBehaviour
    {
        public static FloorEffectElement instance;
        
        public List<Transform> tiles;
        public List<Transform> arrows;
        public List<Transform> walls;
 
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            StartCoroutine(TileEffect());
        }

        IEnumerator TileEffect()
        {
            for (int i = 0; i < walls.Count; i++)
            {
                Vector3 origAngle = walls[i].transform.eulerAngles;
                walls[i].DORotate(new Vector3(60, origAngle.y, origAngle.z), 0.25f).From();
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].transform.DORotate(Vector3.right * 45, 0.25f).From();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.4f);
            for (int i = 0; i < arrows.Count; i++)
            {
                Vector3 origAngle = arrows[i].transform.eulerAngles;
                arrows[i].transform.DORotate(new Vector3(origAngle.x, origAngle.y+ 180, origAngle.z), 0.25f).From();
            }
        }
    }   
}
