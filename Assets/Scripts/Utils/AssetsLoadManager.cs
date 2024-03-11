using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZXS.Utils
{
    public class AssetsLoadManager : UnitySingleton<AssetsLoadManager>
    {
        /// <summary>
        /// 初始化资源加载框架
        /// </summary>
        public void Init()
        {
        }


        public Dictionary<string, T> LoadAllAsset<T>(string resPathName) where T : Object
        {
            Dictionary<string, T> ts = new Dictionary<string, T>();
            var loadAll = Resources.LoadAll<T>(resPathName);
            foreach (var o in loadAll)
            {
                ts[o.name] = o;
            }
        
            return ts;
        }


        public T LoadAsset<T>(string resPathName, bool isPersistent = false) where T : Object
        {
            //用AssetBundel，暂时没有，后续再加

            T t = null;
            t = Resources.Load<T>(resPathName);
            //ResourceRequest resourceRequest = Resources.LoadAsync<T>(resPathName);
            //t = (T)resourceRequest.asset;
            return t;
        }

        public async void LoadAsset(string resPathName, Action action)
        {
            StartCoroutine(LoadAssets(resPathName, action));
        }

        IEnumerator LoadAssets(string resPathName, Action action)
        {
            ResourceRequest resourceRequest = Resources.LoadAsync(resPathName);
            while (true)
            {
                yield return null;
                if (resourceRequest.isDone)
                {
                    break;
                }
            }

            action.Invoke();
        }
    }
}
