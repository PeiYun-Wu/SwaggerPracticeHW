
using CommonModule;
using PennyTest2.DataBase;
using PennyTest2.Models.Api;
using PennyTest2.Services; //連到loginService的using
using System.Web.Http;
using static PennyTest2.Filter.BankEndFilter;

namespace PennyTest2.Controllers
{
    public class LoginController : ApiController
    {
        //getLogin
        /// <summary>
        /// 2.1 getLogin驗證使用者帳號密碼
        /// </summary>
        /// <param name="req">RequestObj_GetLoginRequest</param>
        /// <returns>ResponseObj</returns>
        [IgnoreApiPermit]
        [HttpPost]
        [Route("Login/GetLogin")]
    
        public ResponseObj GetLogin(RequestObj<GetLoginRequest> req)//let swagger Parameters key in GetLoginRequest(model)
        {
            GetLoginRequest data = req.RequestData;
            Commuser.Uid = data.EMPNo; 
            employee ub = LoginService.CheckUserIsExist(data);  //跑到service裡面查看user是不是存在

            if (ub == null) {
                throw new ErrInfoException(ResultStatus.UserAccountError); //使用者不存在
            }

            LoginRoleResponse res = new LoginRoleResponse {
                EMPNo = req.RequestData.EMPNo,
                EMPName = LoginService.GetUserNameE(data),  //去service抓取該帳號的名字
                EMPPass = req.RequestData.EMPPass
            };

            LoginService.SetLogin(data); //登入成功系統發送的Accesstoken

            ResponseObj obj = new ResponseObj(res); //LoginRoleResponse
            return obj;


        }

        /// <summary>
        /// 取得起始token
        /// </summary>
        /// <returns>ResponseObj</returns>
        [IgnoreMtkAuthenticate]
        [IgnoreApiPermit]
        [IgnoreBankEndFilter]
        //[IgnoreLog] 因把LogFilter刪了
        [Route("Login/getCM001")]
        public ResponseObj GetToken()
        {
            AuthService authSrv = new AuthService();
            authSrv.CreateJsonWebToken(string.Empty);
            return new ResponseObj(ResultStatus.SuccessCode);
        }

        //登出
        /// <summary>
        /// 2.1.1 putLogout 使用者登出
        /// </summary>
        /// <param name="req">RequestObj</param>
        /// <returns>ResponseObj</returns>
        [HttpPost]
        [Route("Login/PutLogout")]
        public ResponseObj PutLogout(RequestObj<object> req)
        {
            var uid = Commuser.Uid;
            LoginService.PutLogout(uid);
            GetLoginRequest data = new GetLoginRequest();
            data.EMPNo = uid;
            data.EMPPass = string.Empty;
            return new ResponseObj(ResultStatus.SuccessCode);
        }


        //新增使用者
        [HttpPost]
        [Route("Login/PostEmployee")]
        public ResponseObj PostEmployee(RequestObj<LoginRoleResponse> req)
        {
            LoginRoleResponse data = req.RequestData;
            Commuser.Uid = data.EMPNo;  
            employee ub = LoginService.CheckRoleIsExist(data);

            if (ub != null)//使用者已存在就不給繼續新增
            {
                throw new ErrInfoException(ResultStatus.HadAccountId); 
            }

            LoginService.PostEmployee(req.RequestData);
            return new ResponseObj(ResultStatus.SuccessCode);
            

        }

        //刪除使用者
        [HttpPost]
        [Route("Login/PutEmployee")]
        public ResponseObj PutEmployee(RequestObj<LoginRoleResponse> req)
        {
            LoginRoleResponse data = req.RequestData;
            Commuser.Uid = data.EMPNo;
            employee ub = LoginService.CheckRoleIsExist(data);

            if (ub == null)//使用者不存在就不能刪除
            {
                throw new ErrInfoException(ResultStatus.NoAccountId);
            }

            LoginService.PutEmployee(req.RequestData);
            return new ResponseObj(ResultStatus.SuccessCode);
        }



    }
}