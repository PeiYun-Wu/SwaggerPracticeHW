using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Linq;
using System.Xml.XPath;
using CommonModule;
using Newtonsoft.Json;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerService), "Register")]

namespace CommonModule
{
    /// <summary>
    ///  Swagger服務
    /// </summary>
    public class SwaggerService
    {
        /// <summary>
        ///  Swagger使用的Jwt
        /// </summary>
        public static string SwaggerJwt { get; set; }

        /// <summary>
        /// 註冊Swagger
        /// </summary>
        public static void Register()
        {
            if (CommUtility.GetBaseConfig("TestMode", false))
            {
                var thisAssembly = typeof(SwaggerService).Assembly;
                GlobalConfiguration.Configuration
                    .EnableSwagger(c =>
                        {
                            c.SingleApiVersion("v1" + DateTime.Now.ToString("yyyyMMddhhmm"), "EC Trading API");
                            c.IncludeXmlComments(GetXmlCommentsPath());
                            c.OperationFilter<AddCustomerHeaderParameter>();
                            var xmlDoc = XDocument.Load(GetXmlCommentsPath());
                            c.GroupActionsBy(apiDesc =>
                            {
                                var controllerName = apiDesc.ActionDescriptor.ControllerDescriptor.ControllerName;
                                var controllerFullName = apiDesc.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
                                var controllerType = apiDesc.ActionDescriptor.ControllerDescriptor.ControllerType.GenericTypeArguments.FirstOrDefault()?.FullName;

                                var member = xmlDoc.Root?.XPathSelectElement($"/doc/members/member[@name=\"T:{controllerType}\"]/summary") ??
                                    xmlDoc.Root?.XPathSelectElement($"/doc/members/member[@name=\"T:{controllerFullName}\"]/summary");

                                return member == null ? $"{controllerName}" : $"{member.Value}";
                            });
                        })
                    .EnableSwaggerUi(c => c.DisableValidator());

                CreateSwaggerHeader();
            }
        }

        /// <summary>
        /// 建置後的XML文件檔案
        /// </summary>
        /// <returns>XML檔案路徑</returns>
        private static string GetXmlCommentsPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"\Bin\API.XML";
        }

        /// <summary>
        /// 產生Swagger測試用的資料
        /// </summary>
        private static void CreateSwaggerHeader()
        {
            //產生 swagger Token
            AuthService authSrv = new AuthService();
            string userID = CommUtility.GetBaseConfig("SwaggerUserID", string.Empty);
            string header = SecurityHelper.Base64Encode(JsonConvert.SerializeObject(new JWTHeader()));
            string token = CommUtility.GetBaseConfig("SwaggerToken", string.Empty);
            JWTPayload jwtPayload = new JWTPayload()
            {
                Jti = token,
                Exp = DateTime.Now.AddYears(2).ToString("yyyy/MM/dd HH:mm:ss"),
                Uid = userID
            };
            string payLoad = SecurityHelper.Base64Encode(JsonConvert.SerializeObject(jwtPayload));
            string signature = authSrv.SignJsonWebToken(header, payLoad);
            SwaggerJwt = header + "." + payLoad + "." + signature;
        }
    }

    /// <summary>
    /// 自訂義標頭擴充Class
    /// </summary>
    public class AddCustomerHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// OverWrite Virtual Function
        /// </summary>
        /// <param name="operation">operation</param>
        /// <param name="schemaRegistry">schemaRegistry</param>
        /// <param name="apiDescription">apiDescription</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter { name = "tokenID", @in = "header", type = "string", required = true, @default = SwaggerService.SwaggerJwt });
            string token = CommUtility.GetBaseConfig("SwaggerToken", string.Empty);
            if (!string.IsNullOrEmpty(token))
                operation.parameters.Add(new Parameter { name = "swaggerToken", @in = "header", type = "string", required = false, @default = token });
        }
    }
}
