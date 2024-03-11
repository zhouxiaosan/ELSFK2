using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXS.Utils;

namespace ZXS.Game
{
    public class ScoreAndTime : MonoBehaviour
    {
        public static ScoreAndTime THIS;
        private TextMeshProUGUI timeText;
        private TextMeshProUGUI scroeText;
        private TextMeshProUGUI targetText;
        public int scroeValue;
        private float startTime;
        private bool isTuPo;
        private Slider _slider;
        private float targetScore;
        public bool IsTuPo
        {
            get
            {
                if (isTuPo)
                {
                    isTuPo = false;
                    return true; 
                }
                else
                {
                    return isTuPo;
                }
                
            }
            set { isTuPo = value; }
        }
        
        
        private float fallSpeed = 0.5f;
        public float FallSpeed
        {
            get { return fallSpeed;}
            set { fallSpeed = value; }
        }
        
        private void Awake()
        {
            THIS = this;
            timeText = transform.Find("Time").GetComponent<TextMeshProUGUI>();
            scroeText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
            targetText = transform.Find("TargetScore").GetComponent<TextMeshProUGUI>();
            startTime = Time.time; // 游戏开始时的时间
            _slider = this.transform.Find("Slider").GetComponent<Slider>();
            _slider.value = 0;
            targetScore = 500;
            targetText.text = targetScore.ToString();
        }

        private void Update()
        {
            float t = Time.time - startTime; // 计算游戏开始到现在的时间差

            // 将时间差转换为小时、分钟和秒
            string hours = ((int) t / 3600).ToString();
            string minutes = ((int) (t % 3600) / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            // 更新UI Text内容
            timeText.text ="Time:"+ hours + ":" + minutes + ":" + seconds;

        }

        public void ScroeUp(int rowNum)
        {
            int oldScore = scroeValue;
            scroeValue += rowNum * 10*rowNum;
            scroeText.text ="Score:"+ scroeValue.ToString();

            if (oldScore > 30)
            {
                _slider.value = 1;
            }
            else
            {
                if (oldScore < 10 && scroeValue >= 10)
                {
                
                    Debug.Log("=============解锁星星");
                    isTuPo = true;
                    fallSpeed -= 0.1f;
                    TextTipManager.THIS.tipLevelUp.SetActive(true);
                    targetScore = 1000;
                    targetText.text = targetScore.ToString();
                }
                if (oldScore < 20 && scroeValue >= 20)
                {
                    Debug.Log("=============解锁凸字");
                    isTuPo = true;
                    fallSpeed -= 0.1f;
                    TextTipManager.THIS.tipLevelUp.SetActive(true);
                    targetScore = 1500;
                    targetText.text = targetScore.ToString();
                }
                if (oldScore < 30 && scroeValue >= 30)
                {
                    Debug.Log("=============解锁十字");
                    isTuPo = true;
                    fallSpeed -= 0.1f;
                    TextTipManager.THIS.tipLevelUp.SetActive(true);
                    _slider.value = 1;
                }
                _slider.value = scroeValue / targetScore;
            }
        
        }
    }
}

