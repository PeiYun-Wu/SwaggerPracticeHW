namespace PennyTest2.Models.Api
{
    public class GetLoginRequest //登入功能使用
    {

        public string EMPPass { get; set; }

        public string EMPNo { get; set; } 
       }
    public class LoginRoleResponse //新增刪除使用
    {
   
        public string EMPPass { get; set; }

        public string EMPNo { get; set; }

        public string EMPName { get; set; }
    }
   
}