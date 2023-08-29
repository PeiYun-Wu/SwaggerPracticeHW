using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonModule
{
    /// <summary>
    /// 共用函數
    /// </summary>
    /// <summary>
    /// 計時函數
    /// </summary>
    public class TimeLogHelper
    {
        /// <summary>
        /// 起始時間
        /// </summary>
        private DateTime startTime = DateTime.Now;

        /// <summary>
        /// 上次使用時間
        /// </summary>
        private DateTime lastTime = DateTime.Now;

        /// <summary>
        /// 紀錄多個時間點
        /// </summary>
        private Dictionary<string, DateTime> timeDict = new Dictionary<string, DateTime>();

        /// <summary>
        /// 取得目前經過總時間
        /// </summary>
        /// <returns>回傳從啟動號後到現在經過多久(Seconds)</returns>
        public double GetTotalTime()
        {
            return CountTime(startTime);
        }

        /// <summary>
        /// 取得與上次間隔時間
        /// </summary>
        /// <returns>回傳上次呼叫後到這次經過多久(Seconds)</returns>
        public double GetBetweenLastTime()
        {
            double seconds = CountTime(lastTime);
            lastTime = DateTime.Now;
            return seconds;
        }

        /// <summary>
        /// 將經過總時間與經過上次時間寫入Log
        /// </summary>
        /// <param name="message">log中前導訊息</param>
        public void WriteTimeLog(string message)
        {
            LogHelper.WriteLog(LogLevel.Debug, $"{message}，經過時間：{GetBetweenLastTime()}，總時間：{GetTotalTime()}。");
        }

        /// <summary>
        /// 設定時間點與顯示訊息
        /// </summary>
        /// <param name="message">與時間相關顯示訊息</param>
        /// <param name="setTime">設定時間(未帶入或null使用DateTime.Now)</param>
        public void SetTimeMessage(string message, DateTime? setTime = null)
        {
            if (setTime == null)
                setTime = DateTime.Now;
            if (timeDict.ContainsKey(message))
            {
                //如果訊息已存在，就增加星號後遞迴，避免因key值重複錯誤
                SetTimeMessage(message + "*", setTime);
            }
            else
            { 
                timeDict.Add(message, setTime.Value);
            }
        }

        /// <summary>
        /// 將多個時間點相關訊息寫入Log
        /// </summary>
        /// <param name="endTime">總時間結束點(未帶入時，使用最後一筆的時間點)</param>
        public void WriteMultiTimeLog(DateTime? endTime = null)
        {
            string logMessage = string.Empty;
            DateTime lastAcitveTime = startTime;
            foreach (var timer in timeDict)
            {
                double betweenSeconds = CountTime(lastAcitveTime, timer.Value);
                lastAcitveTime = timer.Value;
                logMessage += $"，{timer.Key}，時間：{betweenSeconds}";
            }
            if (endTime == null)
                endTime = lastAcitveTime;
            double totalSeconds = CountTime(startTime, endTime);
            logMessage = $"總時間：{totalSeconds}" + logMessage;
            LogHelper.WriteLog(LogLevel.Debug, logMessage);
        }

        /// <summary>
        /// 計算兩個時間差
        /// </summary>
        /// <param name="beginTime">開始時間</param>
        /// <param name="endTime">結束時間(未帶入或null使用DateTime.Now)</param>
        /// <returns>回傳兩個時間中間相差的秒數(Seconds)</returns>
        public double CountTime(DateTime beginTime, DateTime? endTime = null)
        {
            if (endTime == null)
                endTime = DateTime.Now;
            TimeSpan time = endTime.Value - beginTime;
            return time.TotalSeconds;
        }
    }
}