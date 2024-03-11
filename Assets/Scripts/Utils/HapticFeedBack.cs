using distriqt.plugins.vibration;
using UnityEngine;
using ZXS.Utils;

namespace ZXS.Utils
{
    public class HapticFeedBack: UnitySingleton<HapticFeedBack>
    {
#if UNITY_IOS
        private HapticAdvancedPlayer _player;
        HapticPattern pattern = new HapticPattern();
        public override void Awake()
        {
            if (Vibration.Instance.HapticEngine.IsSupported)
            {
              _InitHapticEngine();
              
            }else
            {
                Log.Info("=============Init============HapticEngine is not supported");
            
            }
            
        }

        void _InitHapticEngine()
        {
            _player = Vibration.Instance.HapticEngine.createAdvancedPlayer(pattern);
                
            _player?.SendParameters(
                new HapticDynamicParams()
                    .SetParameter(HapticDynamicParams.INTENSITY, 0.1)
                    .SetParameter(HapticDynamicParams.SHARPNESS, 0.2)
            );
              
            Vibration.Instance.HapticEngine.OnStopped += HapticEngine_OnStopped;
            _player.OnComplete += player_OnComplete;
        }
        void player_OnComplete(HapticPlayerEvent e)
        {
            if (_player == null)
            {
                _InitHapticEngine();
                
            }
        }
        void HapticEngine_OnStopped(HapticEngineEvent e)
        {
            _player?.Dispose();
            _player = null;
        }

#endif
        
        
        /// <summary>
        /// shot 震动时常
        /// 1:100，2：200，3:300
        /// </summary>
        /// <param name="shot"></param>
        public void DoVibration(int shot)
        {
            float duration = 0;
            switch (shot)
            {
                case 1:
                    duration = 20;
                    break;
                case 2:
                    duration = 200;
                    break;
                case 3:
                    duration = 300;
                    break;
            }
            
            
            
#if UNITY_ANDROID

            if (Vibration.isSupported)
            {
                if (PlayerPrefs.GetInt("Shock")==1)
                {
                    Vibration.Instance.Vibrate(200);
                } 
            }
           
#endif
            
#if UNITY_IOS

            if (PlayerPrefs.GetInt("Shock") == 1)
            {
                if (_player != null)
                {
                  StartCoroutine(IosVibration(duration));
                }else
                {
                  Log.Info("=============DoVibration============HapticEngine is not supported");
                }
               
            }   
#endif
        }
    private bool isBusy;
#if UNITY_IOS
        IEnumerator IosVibration(float duration)
        {
            if (isBusy)
            {
                _player.Stop();
                isBusy = false;
            }

            yield return new WaitForFixedUpdate();
            isBusy = true;
            if (_player == null){ _InitHapticEngine();}
            _player.Start(); 
            yield return new WaitForSeconds(duration / 1000);
            _player.Stop(); 
            isBusy = false;
          

            yield return null;
        }
#endif
    }
}