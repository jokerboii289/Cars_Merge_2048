using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class CarElement : MonoBehaviour
    {
        public TextMeshPro numText;
        public int num;

        private void Start()
        {
            numText.text = num.ToString();
        }
    }   
}