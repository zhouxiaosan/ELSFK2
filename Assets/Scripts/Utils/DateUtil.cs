using System;
using System.Globalization;
using UnityEngine;

namespace ZXS.Utils
{
    public class DateUtil
    {
        // 格式化时间的函数
        /// <summary>
        /// 把 MS 装成 MM:SS:ss格式
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static string Ms2String(long milliseconds)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(milliseconds);
            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                time.Minutes,
                time.Seconds,
                time.Milliseconds / 10);
        }
        
        /// <summary>
        /// 把 S 装成 00H 00M 00S格式
        /// isUpper 为false则显示  00h 00m 00s格式
        /// </summary>
        public static string Sec2String(long seconds, bool isUpper = true)
        {
            string h = isUpper ? "H" : "h";
            string m = isUpper ? "M" : "m";
            string s = isUpper ? "S" : "s";
            string space = " ";

            TimeSpan time = TimeSpan.FromSeconds(seconds);
            string formattedTime = string.Empty;

            if (time.Hours > 0)
            {
                formattedTime += string.Format("{0}{1}", time.Hours, h);
                if (time.Minutes > 0)
                {
                    formattedTime += string.Format("{0}{1}{2}", space, time.Minutes, m);
                }

                if (time.Seconds > 0)
                {
                    formattedTime += string.Format("{0}{1}{2}", space, time.Seconds, s);
                }
            }
            else if (time.Minutes > 0)
            {
                formattedTime += string.Format("{0}{1}", time.Minutes, m);
                if (time.Seconds > 0)
                {
                    formattedTime += string.Format("{0}{1}{2}", space, time.Seconds, s);
                }
            }
            else
            {
                formattedTime += string.Format("{0}{1}", time.Seconds, s);
            }

            return formattedTime;
        }

    }
}