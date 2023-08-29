using CommonModule;
using PennyTest2.DataBase;
using PennyTest2.Models.Api;
using System;
using System.Data.Entity;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text.RegularExpressions;


namespace PennyTest2.Services
{
    /// <summary>
    /// 2.1/2.1.1/2.2_驗證/取得使用者
    /// </summary>
    public class LoginService : ITokenValidate
    {
        #region 2.1 getLogin驗證使用者帳號密碼

        /// <summary>
        /// 判斷使用者是否存在
        /// </summary>
        /// <param name="data">AD帳號</param>
        /// <returns>USERINFOBACKEND</returns>
        public static employee CheckUserIsExist(GetLoginRequest data) //getLogin登入時查看帳號是否存在
        {
            var query = new employee();
            using (var ecSrv = new PennyTest_Entities())
            {
                query = ecSrv.employee.Where(X => X.EMPNO == data.EMPNo).FirstOrDefault();
                //缺密碼
            }
            return query;
        }
        public static employee CheckRoleIsExist(LoginRoleResponse data) //刪除或新增使用者 先查看id是否存在
        {
            var query = new employee();
            using (var ecSrv = new PennyTest_Entities())
            {
                query = ecSrv.employee.Where(X => X.EMPNO == data.EMPNo).FirstOrDefault();

            }
            return query;
        }

        //登入成功系統發給的AccessToken' 
        public static void SetLogin(GetLoginRequest data)
        {
            AuthService authSrv = new AuthService();
            authSrv.CreateJsonWebToken(data.EMPNo);
            using (var ecSrv = new PennyTest_Entities())
            {
                var query = new TokenEmp();
                query.EMPNO = data.EMPNo;
                query.LOGINDATEUTC = DateTime.UtcNow;
                query.ACCESSTOKEN = Commuser.Token;
                query.CREATOR = data.EMPNo;
                query.CREATEDATETIMEUTC = DateTime.UtcNow;
                ecSrv.TokenEmp.Add(query);
                ecSrv.SaveChanges();
            }
        }

        /// <summary>
        /// 2.1 GetUserNameE_respone
        /// </summary>
        /// <param name="data">AD帳號</param>
        /// <returns>RGroupInfo</returns>
        public static string GetUserNameE(GetLoginRequest data)
        {
            var qUserNameE = string.Empty;
            using (var ecSrv = new PennyTest_Entities())
            {
                var q = ecSrv.employee
                    .Where(x => x.EMPNO == data.EMPNo)
                    .FirstOrDefault();
                if (q != null)
                {
                    qUserNameE = q.NAME;
                }
            }

            return qUserNameE;
        }

        /// <summary>
        /// 2.1.1 putLogout 使用者登出
        /// </summary>
        /// <param name="uid">uid</param>
        public static void PutLogout(string uid)
        {
            var qToken = Commuser.Token;
            using (var ecSrv = new PennyTest_Entities())
            {
                var query = ecSrv.TokenEmp.Where(
                    x => x.EMPNO == uid
                    && x.ACCESSTOKEN == qToken
                    ).FirstOrDefault();

                if (query != null)
                {
                    query.LOGOUTDATEUTC = DateTime.UtcNow;
                    query.MODIFER = uid;
                    query.MODIFYDATETIMEUTC = DateTime.UtcNow;
                    ecSrv.SaveChanges();
                }
            }
        }

        #endregion 2.1 getLogin驗證使用者帳號密碼



        #region 共用方法

        /// <summary>
        /// 登入所使用的 BROSWER和版本
        /// </summary>
        /// <returns>string</returns>
        public static string GetBrowser()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            return context.Request.UserAgent;
        }

        /// <summary>
        /// 登入所使用的GetHeaderToken
        /// </summary>
        /// <returns>string</returns>
        public static string GetHeaderToken()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var q = context.Request.Headers.GetValues("tokenID");
            return q[0];
        }

        /// <summary>
        /// 驗證token合法
        /// </summary>
        /// <param name="toekn">toekn</param>
        /// <returns>bool</returns>
        public bool ValidateDBToken(string toekn)
        {
            using (var ecSrv = new PennyTest_Entities())
            {
                var logB = ecSrv.TokenEmp.Where(x => x.ACCESSTOKEN.Equals(toekn)).First();
                if (logB != null)
                {
                    //ecSrv.Entry(logB).State = EntityState.Modified; //修改
                    //ecSrv.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 驗證token有效
        /// </summary>
        /// <param name="toekn">toekn</param>
        /// <returns>bool</returns>
        public bool ValidateDBTokenEffective(string toekn)
        {
            using (var ecSrv = new PennyTest_Entities())
            {
                var logB = ecSrv.TokenEmp.Where(x => x.ACCESSTOKEN.Equals(toekn)).First();
                if (logB != null)
                {
                    //驗證Token有效期限
                    int tokenEffectiveMin = CommUtility.GetConfig("TokenEffectiveMin", 1440);

                    if (logB.CREATEDATETIMEUTC.Value.AddMinutes(tokenEffectiveMin) < DateTime.UtcNow)
                    {
                        return false;
                    }

                    //驗證登入有效時間
                    int loginEffectiveMin = CommUtility.GetConfig("LoginEffectiveMin", 30);

                    if (logB.MODIFYDATETIMEUTC.HasValue)
                    {
                        if (logB.MODIFYDATETIMEUTC.Value.AddMinutes(loginEffectiveMin) < DateTime.UtcNow)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (logB.CREATEDATETIMEUTC.Value.AddMinutes(loginEffectiveMin) < DateTime.UtcNow)
                        {
                            return false;
                        }
                    }

                    ecSrv.Entry(logB).State = EntityState.Modified; //修改
                    ecSrv.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 驗證token是否被登出
        /// </summary>
        /// <param name="toekn">toekn</param>
        /// <returns>bool</returns>
        public bool ValidateDBTokenLogout(string toekn)
        {
            using (var ecSrv = new PennyTest_Entities())
            {
                var logB = ecSrv.TokenEmp.Where(x => x.ACCESSTOKEN.Equals(toekn)).First();
                if (logB != null)
                {
                    //驗證Token狀態
                    if (logB.LOGOUTDATEUTC != null)  //改過
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 用戶資料驗證與資料取得
        /// </summary>
        /// <param name="account">畫面輸入之帳號</param>
        /// <param name="password">畫面輸入之密碼</param>
        /// <returns>bool</returns>
        private bool ValidateAD(string account, string password)
        {
            try
            {
                var context = new PrincipalContext(ContextType.Domain, CommUtility.GetConfig("ADDomainName"), account, password);
                var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, account);

                return true;
            }
            catch (DirectoryServicesCOMException exc)
            {
                Match match = Regex.Match(exc.ExtendedErrorMessage, @" data (?<errCode>[0-9A-Fa-f]+),");
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 共用方法

        //insert
        public static void PostEmployee(LoginRoleResponse req)
        {
            using (var ecSrv = new PennyTest_Entities())
            {
                var query = new employee();
                query.EMPNO = req.EMPNo;
                query.EMPPASS = req.EMPPass;
                query.NAME = req.EMPName;

                ecSrv.employee.Add(query);
                ecSrv.SaveChanges();
            }

        }
        //Delect
        public static void PutEmployee(LoginRoleResponse data)
        {

            using (var ecSrv = new PennyTest_Entities())
            {
                var q1 = (from A in ecSrv.employee
                          where A.EMPNO == data.EMPNo
                          select A).FirstOrDefault();

                if (q1 != null) //有使用者
                {
                    ecSrv.employee.Remove(q1);
                    ecSrv.SaveChanges();
                }


            }
        }
    }
}