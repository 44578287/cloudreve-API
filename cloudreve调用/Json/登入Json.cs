namespace cloudreve调用.Json
{
    internal class LoginJson
    {
        /// <summary>
        /// 登入资料处理
        /// </summary>
        public class LoginDataJson
        {
            /// <summary>
            /// 账号
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string? Password { get; set; }
            /// <summary>
            /// 二步验证 没有留空即可
            /// </summary>
            public string? CaptchaCode { get; set; }
            /// <summary>
            /// 合成登入信息
            /// </summary>
            /// <param name="LoginJson">传入登入信息</param>
            /// <returns>string 类型 登入Json信息</returns>
            public static string LoginReturnJson(LoginDataJson LoginJson)
            {
                return System.Text.Json.JsonSerializer.Serialize(LoginJson);
            }
            /// <summary>
            /// 合成登入信息
            /// </summary>
            /// <param name="UserName">账号</param>
            /// <param name="Password">密码</param>
            /// <param name="CaptchaCode">二步验证 没有留空即可</param>
            /// <returns>string 类型 登入Json信息</returns>
            public static string LoginReturnJson(string UserName, string Password, string CaptchaCode)
            {
                return LoginReturnJson(new LoginDataJson() { UserName = UserName, Password = Password, CaptchaCode = CaptchaCode });//合成登入Json
            }
            /// <summary>
            /// 合成登入信息
            /// </summary>
            public string LoginReturnJson()
            {
                return LoginReturnJson(this);//合成登入Json
            }
        }
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class @group
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool allowShare { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool allowRemoteDownload { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool allowArchiveDownload { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool shareDownload { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool compress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool webdav { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int sourceBatch { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string user_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string nickname { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string avatar { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string created_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string preferred_theme { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool anonymous { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public @group @group { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> tags { get; set; }
        }
        /// <summary>
        /// 登入服务器响应内容
        /// </summary>
        public class LoginReturnJson
        {
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Data? data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string msg { get; set; }
        }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }
}
