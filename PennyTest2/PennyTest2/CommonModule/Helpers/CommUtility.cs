using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

namespace CommonModule
{
    /// <summary>
    /// 共用函數
    /// </summary>
    public static class CommUtility
    {
        #region Config Function
        /// <summary>
        /// 讀取Config 介面元件(原始型態)
        /// </summary>
        private static IConfig baseConfig = new ConfigHelper();

        /// <summary>
        /// 讀取Config 介面元件
        /// </summary>
        private static IConfig config = new ConfigHelper();

        /// <summary>
        /// 設定Config
        /// </summary>
        /// <param name="iConfig">IConfig介面的Config</param>
        public static void SetConfig(IConfig iConfig)
        {
            config = iConfig;
        }

        /// <summary>
        /// 讀取config 參數(T型態)(從Config)
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <param name="defValue">預設參數</param>
        /// <returns>T型態參數內容</returns>
        public static T GetBaseConfig<T>(string name, T defValue)
        {
            return baseConfig.GetConfig(name, defValue);
        }

        /// <summary>
        /// 讀取config 參數(T型態)(從Config)
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <returns>T型態參數內容</returns>
        public static T GetBaseConfig<T>(string name)
        {
            return baseConfig.GetConfig<T>(name);
        }

        /// <summary>
        /// 讀取config 參數(字串型態)
        /// </summary>
        /// <param name="name">參數名稱</param>
        /// <returns>參數內容</returns>
        public static string GetConfig(string name)
        {
            return GetConfig<string>(name);
        }

        /// <summary>
        /// 讀取config 參數(T型態)
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <returns>T型態參數內容</returns>
        public static T GetConfig<T>(string name)
        {
            return config.GetConfig<T>(name);
        }

        /// <summary>
        /// 讀取config 參數(T型態)
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <param name="defValue">預設參數</param>
        /// <returns>T型態參數內容</returns>
        public static T GetConfig<T>(string name, T defValue) //多載
        {
            return config.GetConfig(name, defValue);
        }

        /// <summary>
        /// 讀取config 參數(T型態)，當讀取失敗時，改讀取基礎Config
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <param name="defValue">預設參數</param>
        /// <returns>T型態參數內容</returns>
        public static T GetConfigWhenFailGetBaseConfig<T>(string name, T defValue)
        {
            try
            {
                return GetConfig<T>(name);
            }
            catch
            {
                return GetBaseConfig(name, defValue);
            }
        }

        /// <summary>
        /// 讀取config 參數(T型態)，當讀取失敗時，改讀取基礎Config
        /// </summary>
        /// <typeparam name="T">參數型態</typeparam>
        /// <param name="name">參數名稱</param>
        /// <returns>T型態參數內容</returns>
        public static T GetConfigWhenFailGetBaseConfig<T>(string name)
        {
            try
            {
                return GetConfig<T>(name);
            }
            catch
            {
                return GetBaseConfig<T>(name);
            }
        }

        #endregion

        /// <summary>
        /// 取得Web.config connectionStrings字串
        /// </summary>
        /// <param name="key">DB連線字串對應的Key值</param>
        /// <returns>DB連線字串</returns>
        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        /// <summary>
        /// 將字串內容轉為布林型態
        /// </summary>
        /// <remarks>
        /// Y,TRUE,1,YES 為true；N,NO,FALSE,0 為false
        /// 其餘傳回default_value
        /// </remarks>
        /// <param name="input_string">傳入參數</param>
        /// <param name="default_value">預設值</param>
        /// <returns>轉換後的布林型態</returns>
        public static bool StringToBool(string input_string, bool default_value = false)
        {
            try
            {
                if (input_string == null)
                {
                    return default_value;
                }

                switch (input_string.ToUpper())
                {
                    case "Y":
                    case "TRUE":
                    case "1":
                    case "YES":
                        return true;

                    case "NO":
                    case "N":
                    case "FALSE":
                    case "0":
                        return false;

                    default:
                        return default_value;
                }
            }
            catch
            {
            }

            return default_value;
        }

        /// <summary>
        /// 將字串內容轉為布林型態
        /// </summary>
        /// <remarks>
        /// Y,TRUE,1,YES 為true；N,NO,FALSE,0 為false
        /// 其餘傳回default_value
        /// </remarks>
        /// <param name="input_string">傳入參數</param>
        /// <param name="output">轉換後的布林型態</param>
        /// <returns>true:轉換成功，false:轉換失敗</returns>
        public static bool TryStringToBool(string input_string, out bool output)
        {
            output = false;
            if (input_string == null)
            {
                return false;
            }

            switch (input_string.ToUpper())
            {
                case "Y":
                case "TRUE":
                case "1":
                case "YES":
                    output = true;
                    break;

                case "NO":
                case "N":
                case "FALSE":
                case "0":
                    output = false;
                    break;

                default:
                    return false;
            }
            return true;
        }

        // HttpContext.Items 物件的生命週期很短，只會出現在這一個 HTTP Request 裡面而已
        // http://blog.miniasp.com/post/2008/02/28/Use-HttpContext-Items-pass-data-between-HttpModule-and-HttpHandler.aspx

        /// <summary>
        /// GetContext.Items暫存資訊
        /// </summary>
        /// <param name="key">Items Key</param>
        /// <returns>Items Content</returns>
        public static string GetContext(string key)
        {
            object result = HttpContext.Current.Items[key];
            if (result == null)
            {
                return null;
            }

            return result.ToString();
        }

        /// <summary>
        /// GetContext.Items暫存資訊
        /// </summary>
        /// <typeparam name="T">回傳型態</typeparam>
        /// <param name="key">Items Key</param>
        /// <returns>Items Content</returns>
        public static T GetContext<T>(string key)
        {
            object result = HttpContext.Current.Items[key];
            if (result == null)
            {
                return default(T);
            }

            return (T)result;
        }

        /// <summary>
        /// SetContext.Items暫存資訊
        /// </summary>
        /// <param name="key">Items Key</param>
        /// <param name="value">Items Content</param>
        public static void SetContext(string key, object value)
        {
            HttpContext.Current.Items[key] = value;
        }

        /// <summary>
        /// 使用SQL IN語法時，SqlParameter擺入字串處理
        /// </summary>
        /// <remarks>將陣列變數傳入SQL IN 指定，自動產生變數@InValue1 ~@InValueN，
        /// 並將變數至寫入condition集合中</remarks>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="condition">SqlParameter 參數集合</param>
        /// <param name="values">輸入多個參數</param>
        /// <param name="valueName">SqlParameter 參數名稱(非必填)</param>
        /// <returns>判斷結果</returns>
        public static string SQLIN<T>(List<SqlParameter> condition, T[] values, string valueName = "InValue")
        {
            if (values == null || values.Length == 0)
            {
                return string.Empty;
            }

            string result = string.Empty;
            int idx = 1;
            foreach (T value in values)
            {
                string thisValueName = "@" + valueName + idx.ToString().Trim();
                result += "," + thisValueName;
                condition.Add(new SqlParameter(thisValueName, value));
                idx++;
            }

            return result.Substring(1);
        }

        /// <summary>
        /// 使用SQL LIKE語法時，SqlParameter擺入字串處理
        /// </summary>
        /// <remarks>
        /// 包含阻止使用特殊字元查詢
        /// </remarks>
        /// <param name="condition">查詢條件</param>
        /// <param name="value">SqlParameter 參數集合</param>
        /// <param name="valueName">SqlParameter 參數名稱(非必填)</param>
        /// <returns>字串處理結果</returns>
        public static string SQLLIKE(List<SqlParameter> condition, string value, string valueName = "InValue")
        {
            value = value.Replace("[", "[[]");
            value = value.Replace("_", "[_]");
            value = value.Replace("%", "[%]");
            string thisValueName = "@" + valueName;
            condition.Add(new SqlParameter(thisValueName, "%" + value + "%"));
            return thisValueName;
        }

        /// <summary>
        /// 轉換對應字串
        /// </summary>
        /// <remarks>
        /// 例:"1","借款","2","還款","預設字串"
        /// value = "1" return "借款"
        /// value = "2" return "還款"
        /// value = "3" (有預設字串時) return "預設字串"
        /// value = "3" (無預設字串時) return "3"
        /// </remarks>
        /// <param name="value">輸入字串</param>
        /// <param name="condition">條件及回應字串(預設字串)，例:"1","借款","2","還款","預設字串"</param>
        /// <returns>回應字串或預設字串</returns>
        public static string StrMapping(string value, params string[] condition)
        {
            if (condition == null)
            {
                return value;
            }

            for (int idx = 0; idx < condition.Length; idx += 2)
            {
                if (condition[idx] == value)
                {
                    if (condition.Length > idx + 1)
                    {
                        return condition[idx + 1];
                    }
                }
            }

            if (condition.Length % 2 == 1)
            {
                return condition[condition.Length - 1];
            }

            return value;
        }

        /// <summary>
        /// 轉換對應字串
        /// </summary>
        /// <remarks>
        /// 例:"借款","還款"
        /// value = 0 return "借款"
        /// value = 1 return "還款"
        /// 當找不到時回傳Value
        /// </remarks>
        /// <param name="value">輸入字串</param>
        /// <param name="condition">條件及回應字串，例:"借款","還款"</param>
        /// <returns>回應字串或預設字串</returns>
        public static string IntMapping(int value, params string[] condition)
        {
            if (condition == null || value > condition.Length)
            {
                return value.ToString();
            }

            return condition[value];
        }

        /// <summary>
        /// 轉換對應字串
        /// </summary>
        /// <remarks>
        /// 例:"借款","還款"
        /// value = 0 return "借款"
        /// value = 1 return "還款"
        /// 當找不到時回傳Value
        /// </remarks>
        /// <param name="value">輸入字串</param>
        /// <param name="condition">條件及回應字串，例:"借款","還款"</param>
        /// <returns>回應字串或預設字串</returns>
        public static string IntMapping(byte value, params string[] condition)
        {
            return IntMapping((int)value, condition);
        }

        /// <summary>
        /// string to Int
        /// </summary>
        /// <param name="value">輸入字串</param>
        /// <param name="defValue">預設數值</param>
        /// <returns>轉換結果</returns>
        public static int ToInt(string value, int defValue = 0)
        {
            int ret = 0;
            if (int.TryParse(value, out ret))
            {
                return ret;
            }
            else
            {
                return defValue;
            }
        }

        /// <summary>
        /// string to decimal
        /// </summary>
        /// <param name="value">輸入字串</param>
        /// <param name="defValue">預設數值</param>
        /// <returns>轉換結果</returns>
        public static decimal ToDecimal(string value, decimal defValue = 0)
        {
            decimal ret = 0;
            if (decimal.TryParse(value, out ret))
            {
                return ret;
            }
            else
            {
                return defValue;
            }
        }

        /// <summary>
        /// string to int?
        /// </summary>
        /// <param name="value">輸入字串</param>
        /// <returns>轉換結果</returns>
        public static int? ToIntOrNull(object value)
        {
            int? rtnValue = null;
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                rtnValue = Convert.ToInt32(value.ToString());
            return rtnValue;
        }

        /// <summary>
        /// string to decimal?
        /// </summary>
        /// <param name="value">輸入字串</param>
        /// <returns>轉換結果</returns>
        public static decimal? ToDecimalOrNull(object value)
        {
            decimal? rtnValue = null;
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                rtnValue = Convert.ToDecimal(value.ToString());
            return rtnValue;
        }

        /// <summary>
        /// string to DateTme?
        /// </summary>
        /// <param name="value">輸入字串</param>
        /// <returns>轉換結果</returns>
        public static DateTime? ToDateTimeOrNull(object value)
        {
            DateTime? rtnValue = null;
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                rtnValue = Convert.ToDateTime(value.ToString());
            return rtnValue;
        }

        /// <summary>
        /// 以逗點隔開列表中資料
        /// </summary>
        /// <param name="list">列表資料</param>
        /// <returns>組合結果</returns>
        public static string SplitValue(List<string> list)
        {
            var result = string.Empty;
            foreach (var item in list)
            {
                result += item + ",";
            }

            return result.Substring(0, result.Length - 1);
        }

        /// <summary>
        /// 過濾DataTable空行
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>過濾結果</returns>
        public static DataTable DataTableFilter(DataTable dt)
        {
            List<DataRow> removeList = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool isNullRowData = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        isNullRowData = false;
                    }
                }

                if (isNullRowData)
                {
                    removeList.Add(dt.Rows[i]);
                }
            }

            for (int i = 0; i < removeList.Count; i++)
            {
                dt.Rows.Remove(removeList[i]);
            }

            return dt;
        }

        /// <summary>
        /// nubmer取小數點到decimals位四捨五入
        /// </summary>
        /// <param name="number">輸入數值</param>
        /// <param name="decimals">小數位數</param>
        /// <returns>CompanyCode</returns>
        public static decimal GetPoint(decimal number, int decimals = 0)
        {
            //四捨五入
            return Math.Round(number, decimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// nubmer取小數點到decimals位四捨五入
        /// </summary>
        /// <param name="number">輸入數值</param>
        /// <param name="decimals">小數位數(預設為0)</param>
        /// <param name="fillZero">小數位數不足的是否要補0(預設為要)</param>
        /// <returns>CompanyCode</returns>
        public static string GetPointToString(decimal number, int decimals = 0, bool fillZero = true)
        {
            //四捨五入
            decimal result = GetPoint(number, decimals);
            if (fillZero)
            {
                string format = "0.".PadRight(decimals + 2, '0');
                return result.ToString(format);
            }
            else
                return result.ToString();
        }

        /// <summary>
        /// 複製元件(使用Json轉換)
        /// </summary>
        /// <typeparam name="T">物件類型</typeparam>
        /// <param name="objectSource">要複製的元件</param>
        /// <returns>擁有相同參數的元件</returns>
        public static T Clone<T>(T objectSource) where T : class
        {
            string jsonString = JsonConvert.SerializeObject(objectSource);
            T obj = JsonConvert.DeserializeObject<T>(jsonString);
            return obj;
        }

        /// <summary>
        /// 以Entity產生Native Sql insert 語法
        /// </summary>
        /// <param name="data">資料Entity</param>
        /// <param name="paramList">參數列表</param>
        /// <param name="idx">參數代號</param>
        /// <returns>Insert Sql 語法</returns>
        public static string BuildInsertSQL(object data, List<SqlParameter> paramList, int idx = 0)
        {
            Type dataType = data.GetType();
            string filedsName = string.Empty;
            string paramsName = string.Empty;
            foreach (var prop in dataType.GetProperties())
            {
                if (prop.Name == "Serial")
                {
                    // Serial 不產生insert 語法，由資料庫自動生成遞增序號
                    continue;
                }

                if (prop.Name == "Creator")
                {
                    string dbUser = Commuser.DBUser;
                    prop.SetValue(data, dbUser);
                }

                if (prop.Name == "CreateDate")
                {
                    prop.SetValue(data, DateTime.Now);
                }

                if (prop.GetValue(data) == null)
                {
                    // 資料內容為null時，不產生insert 語法，由DB自行放入null 
                    continue;
                }

                filedsName += prop.Name + ',';

                string varName = prop.Name + idx.ToString().Trim();
                paramsName += '@' + varName + ',';
                SqlParameter param = new SqlParameter(varName, prop.GetValue(data));
                paramList.Add(param);
            }

            filedsName = filedsName.TrimEnd(',');
            paramsName = paramsName.TrimEnd(',');

            string tableName = dataType.Name;

            // 取得自訂義Table Name
            var customAttributesData = dataType.GetCustomAttributesData();
            var tableAttr = customAttributesData.Where(x => x.AttributeType.Name == "TableAttribute");

            if (tableAttr.Count() != 0)
            {
                var tableNameAttr = tableAttr.Select(x => x.ConstructorArguments.FirstOrDefault()).FirstOrDefault();
                if (tableNameAttr.Value != null)
                {
                    tableName = tableNameAttr.Value.ToString();
                }
            }

            return $"INSERT INTO {tableName} ({filedsName}) VALUES ({paramsName});";
        }

        /// <summary>
        /// 變數型態轉換
        /// </summary>
        /// <typeparam name="T">轉換變數型態</typeparam>
        /// <param name="value">變數內容</param>
        /// <param name="defValue">預設值</param>
        /// <returns>T 型態變數，轉換失敗回傳預設值</returns>
        public static T ConvertValue<T>(string value, T defValue)
        {
            Type valueType = typeof(T);
            switch (valueType.Name)
            {
                case "Int32":
                    int intValue = 0;
                    if (int.TryParse(value, out intValue))
                        return (T)(object)intValue;
                    else
                        return defValue;
                case "Decimal":
                    decimal decimalValue = 0;
                    if (decimal.TryParse(value, out decimalValue))
                        return (T)(object)decimalValue;
                    else
                        return defValue;
                case "String":
                    if (value == null)
                        return defValue;
                    else
                        return (T)(object)value;
                case "Boolean":
                    return (T)(object)StringToBool(value, (bool)(object)defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 變數型態轉換
        /// </summary>
        /// <typeparam name="T">轉換變數型態</typeparam>
        /// <param name="value">變數內容</param>
        /// <param name="output">T 型態變數</param>
        /// <returns>true:轉換成功，false:轉換失敗</returns>
        public static bool TryConvertValue<T>(string value, out T output)
        {
            output = default(T);
            if (value == null)
                return false;

            Type valueType = typeof(T);
            switch (valueType.Name)
            {
                case "Int32":
                    int intValue = 0;
                    if (int.TryParse(value, out intValue))
                        output = (T)(object)intValue;
                    else
                        return false;
                    break;
                case "Decimal":
                    decimal decimalValue = 0;
                    if (decimal.TryParse(value, out decimalValue))
                        output = (T)(object)decimalValue;
                    else
                        return false;
                    break;
                case "String":
                    output = (T)(object)value;
                    break;
                case "Boolean":
                    bool boolValue = false;

                    bool result = TryStringToBool(value, out boolValue);
                    output = (T)(object)boolValue;
                    return result;
            }
            return true;
        }

        /// <summary>
        /// 讀取Client IP資料，抓取X_FORWARDED_FOR參數 寫Log使用
        /// </summary>
        /// <returns>config 參數</returns>
        public static string GetClientIPInfo()
        {
            string clientIP = HttpContext.Current.Request.UserHostAddress;
            try
            {
                var http_X_FORWARDED_FOR = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrWhiteSpace(http_X_FORWARDED_FOR))
                    return clientIP;
                else
                    return $"{http_X_FORWARDED_FOR}>={clientIP}";
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogLevel.Warn, "GetClientIPInfo Exception:", ex);
                return $"GetClientIPInfo Exception:{ex.Message}";
            }
        }

        /// <summary>
        /// GetLocaHostIP
        /// </summary>
        /// <returns>LocalIP</returns>
        public static string GetLocaHostIP()
        {
            string localIP = string.Empty;
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(string.Empty);
                foreach (IPAddress iPAddress in ipHostEntry.AddressList)
                {
                    if (iPAddress.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        localIP = iPAddress.ToString();
                    }
                }
                return localIP;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogLevel.Warn, "GetLocaHostIP Exception:", ex);
                return $"GetLocaHostIP Exception:{ex.Message}";
            }
        }

        /// <summary>
        /// 使Model類似的兩個物件以toObj欄位為主，將fromObj資料寫入toObj
        /// </summary>
        /// <param name="fromObj">資料來源物件</param>
        /// <param name="toObj">轉換結果物件</param>
        public static void MatchObject(object fromObj, object toObj)
        {
            Type dataType = toObj.GetType();
            foreach (var prop in dataType.GetProperties())
            {
                // 判斷來源是否有相同欄位
                var fromData = fromObj.GetType().GetProperty(prop.Name);
                if (fromData != null)
                {
                    // 資料形態一樣就對轉
                    if (prop.PropertyType == fromData.PropertyType)
                    {
                        prop.SetValue(toObj, fromData.GetValue(fromObj));
                    }

                    // 欄位名稱開頭為Is
                    if (prop.Name.StartsWith("Is"))
                    {
                        if (prop.PropertyType == fromObj.GetType().GetProperty(prop.Name).PropertyType)
                        {
                            prop.SetValue(toObj, fromData.GetValue(fromObj));
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(toObj, ((int)fromData.GetValue(fromObj)).TransYN(prop.GetValue(toObj)));
                        }
                        else if (prop.PropertyType == typeof(int))
                        {
                            prop.SetValue(toObj, ((string)fromData.GetValue(fromObj)).TransYN(prop.GetValue(toObj)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 0,1轉換Y,N
        /// </summary>
        /// <param name="value">0,1</param>
        /// <param name="defaultValue">非YN則不進行變更</param>
        /// <returns>Y,N</returns>
        public static string TransYN(this int value, object defaultValue = null)
        {
            switch (value)
            {
                case 0:
                    return "N";
                case 1:
                    return "Y";
                default:
                    return (string)defaultValue;
            }
        }

        /// <summary>
        /// Y,N轉換0,1
        /// </summary>
        /// <param name="value">Y,N</param>
        /// <param name="defaultValue">非YN則不進行變更</param>
        /// <returns>0,1</returns>
        public static int TransYN(this string value, object defaultValue = null)
        {
            switch (value.ToUpper())
            {
                case "Y":
                    return 1;
                case "N":
                    return 0;
                default:
                    return (int)defaultValue;
            }
        }

        /// <summary>
        /// 安全的除法
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        /// <returns>結果(分母為0,回傳0)</returns>
        public static decimal SafeDivision(decimal numerator, decimal denominator)
        {
            if (denominator == 0)
                return 0M;
            else
                return (numerator / denominator);
        }
    }
}