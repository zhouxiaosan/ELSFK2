using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXS.Game
{
    public class TextTipManager : MonoBehaviour
    {
        public static TextTipManager THIS;
        public GameObject tipLevelUp;


        private void Awake()
        {
            THIS = this;
        }
        
        
    }

}
