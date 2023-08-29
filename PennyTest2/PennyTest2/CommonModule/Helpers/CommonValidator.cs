using System;
using System.IO;

namespace CommonModule
{
    /// <summary>
    /// 常用檢核
    /// </summary>
    public class CommonValidator
    {
        /// <summary>
        /// 是否為數字
        /// </summary>
        /// <param name="str">確認字串</param>
        /// <returns>判斷結果</returns>
        public static bool IsNum(string str)
        {
            decimal result = 0;
            bool ret = decimal.TryParse(str, out result);
            return ret;
        }

        /// <summary>
        /// 確認是否為Y或N
        /// </summary>
        /// <param name="str">確認字串</param>
        /// <returns>判斷結果 true為是，false為不是</returns>
        public static bool CheckYOrN(string str)
        {
            if ("Y".Equals(str) || "N".Equals(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判斷是否為日期格式
        /// </summary>
        /// <param name="inDate">確認字串</param>
        /// <returns>判斷結果</returns>
        public static bool IsValidDate(string inDate)
        {
            if (inDate == null)
            {
                return false;
            }

            DateTime result = new DateTime();

            // parse the inDate parameter
            bool ret = DateTime.TryParse(inDate, out result);

            return ret;
        }

        /// <summary>
        /// 判斷檔名是否有容易造成異常字眼
        /// </summary>
        /// <param name="fileName">檔名</param>
        /// <param name="extensionList">需判斷副檔名</param>
        /// <returns>判斷結果</returns>
        public static bool IsValidFileName(string fileName, string[] extensionList)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            string[] checkNameList = { "~", "\"", "/", "." };
            foreach (var item in checkNameList)
            {
                if (name.Contains(item))
                {
                    return false;
                }
            }

            foreach (var item in extensionList)
            {
                if (Path.GetExtension(fileName).EndsWith(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
