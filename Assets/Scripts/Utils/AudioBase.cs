using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXS.Game
{
    public class AudioBase : MonoBehaviour
    {
        private AudioSource _audioSource;
        public static AudioBase Instance;
        
        public AudioClip itemMove;  //格子移动
        public AudioClip itemChange;  //格子变形
        public AudioClip itemEliminate;  //格子消除
        public AudioClip itemFall;  //格子掉落
        public AudioClip itemPostpone;  //消除后补位
        public AudioClip itemOut;  // 元素出场
        
        void Awake()
        {

            _audioSource = GetComponent<AudioSource>();
            if (transform.parent == null)
            {
                transform.parent = Camera.main?.transform;
                transform.localPosition = Vector3.zero;
            }
            // DontDestroyOnLoad(gameObject);
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

        }
        
        
        public void PlayOneShot(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
 
}
