using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXS.Utils;

namespace ZXS.Game
{
    public class ItemSquare : MonoBehaviour
    {
        Image image;
        public int row;
        public int col;
        public bool _isFull;

        public Color oldColor;
        private Transform boom;
        Sprite _sprite;

        private void Awake()
        {
            image = GetComponent<Image>();
            _sprite = AssetsLoadManager.Instance.LoadAsset<Sprite>("Textures/c1");
            boom = transform.Find("Boom");
        }

        public void SetRowCol(int row, int col,Color _clr)
        {
            this.row = row;
            this.col = col;
            oldColor = _clr;
            image.color = _clr;
        }

        public void showBoom()
        {
            boom.gameObject.SetActive(true);
           // AudioBase.Instance.PlayOneShot(AudioBase.Instance.itemEliminate);
            StartCoroutine(autoHideBoom());
        }

        
        IEnumerator autoHideBoom()
        {
            yield return new WaitForSeconds(1f);
            boom.gameObject.SetActive(false);
        }
        public void SetImageFull(bool isFull)
        {
            _isFull = isFull;
            
            if (isFull)
            {
                image.color = Color.white;
                image.sprite = _sprite;
            }
            else
            {
                image.color = oldColor;
                image.sprite = null;
            }
        }
        
    }
  
}
