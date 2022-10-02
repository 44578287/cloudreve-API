using LoongEgg.LoongLogger;
using System.Net;
using System.Text.Json;
using static cloudreve调用.Json.LoginJson;
using static cloudreve调用.Json.UploadFilesJson;
using static cloudreve调用.MODS.NetworkRequest;

namespace cloudreve调用.API
{
    internal class CloudreveAPI
    {
        /// <summary>
        /// 登入Cloudreve
        /// </summary>
        /// <param name="Url">Cloudreve服务器地址</param>
        /// <param name="LoginData">登入信息 可用LoginDataJson.LoginReturnJson(string UserName, string Password, string CaptchaCode) 替代</param>
        /// <param name="ScreenOut">屏幕显示输出</param>
        /// <returns>string 类型 Cookie</returns>
        public static string? Login(string Url, LoginDataJson LoginData, bool ScreenOut = true)
        {
            HttpWebResponse? ReturnData = HttpRequest(Url + "/api/v3/user/session", httpMod: HttpMods.POST, data: LoginData.LoginReturnJson(), LogOut: false);//登入并获取服务器响应
            if (ReturnData == null)//如果是null那就是连服务器都访问不上
            {
                Logger.WriteError("登入Cloudreve失败!因为上面的原因...");//打印至日志
                if (ScreenOut)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("登入Cloudreve失败!因为上面的原因...");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return null;
            }
            string? ReturnDataString = HttpReturnData.HttpWebDataToString(ReturnData, false);//返回内容
            Logger.WriteDebug(ReturnDataString);//打印至日志
            LoginReturnJson? LoginReturnJson = JsonSerializer.Deserialize<LoginReturnJson>(ReturnDataString);//返回内容Json序列化
            string? ReturnCookie = ReturnData?.Headers["Set-Cookie"];//返回Cookie

            if (ReturnCookie == null)//用Cookie判断是否有登入成功
            {
                Logger.WriteError("登入Cloudreve失败!因为:" + LoginReturnJson?.msg);//打印至日志
                if (ScreenOut)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("登入Cloudreve失败!因为:" + LoginReturnJson?.msg);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return null;
            }
            Logger.WriteInfor("登入Cloudrevet成功!");//打印至日志
            if (ScreenOut)
                Console.WriteLine("登入Cloudrevet成功!");
            return ReturnCookie;
        }
        /// <summary>
        /// 上传文件至云盘
        /// </summary>
        /// <param name="Url">Cloudreve服务器地址</param>
        /// <param name="cookie">登入返还的Cookie</param>
        /// <param name="policy">Cloudreve的存储策略</param>
        /// <param name="FilesPath">本地文件路径</param>
        /// <param name="CloudFilesPath">云盘上传路径(默认根目录"/")</param>
        /// <param name="sessionID">任务ID 可替PUT请求 前提任务ID没有被清除</param>
        /// <param name="SliceSize">上传分片大小</param>
        /// <param name="Slice">上传任务分片</param>
        /// <param name="ScreenOut">屏幕显示输出</param>
        /// <returns>string 类型 上传状态</returns>
        public static string? UpFile(string Url, string? cookie, string policy, string FilesPath, string CloudFilesPath = "/", string? sessionID = null, int SliceSize = 0, int Slice = 0, bool ScreenOut = true)
        {
            string? HttpRequestToStringData = null;
            if (sessionID == null)
            {
                PUT_UploadFilesDataJson Upload = new();
                string? UploadJson = Upload.Updata(FilesPath, policy, CloudFilesPath);
                string? UploadReturnJson = HttpRequestToString(Url + "/api/v3/file/upload/", cookie: cookie, httpMod: HttpMods.PUT, data: UploadJson);//发送上传请求
                if (UploadReturnJson == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送上传请求失败(PUT)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送上传请求失败(PUT)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                PUT_UploadFilesReturnJson? PUT_UploadFilesReturnJson = JsonSerializer.Deserialize<PUT_UploadFilesReturnJson>(UploadReturnJson);
                if (PUT_UploadFilesReturnJson?.code != 0)
                {
                    Logger.WriteError("上传失败(PUT)!因为:" + PUT_UploadFilesReturnJson?.msg + " #文件名:" + System.IO.Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("上传失败(PUT)!因为:" + PUT_UploadFilesReturnJson?.msg + " #文件名:" + System.IO.Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return UploadJson;
                }
                sessionID = PUT_UploadFilesReturnJson?.data.sessionID;
                double ExpectedSliceNumber = Math.Ceiling((double)(new FileInfo(FilesPath).Length / PUT_UploadFilesReturnJson?.data.chunkSize)!);
                Logger.WriteInfor("发送上传请求成功(PUT)!ID:" + sessionID + " 预估分片数量:" + ExpectedSliceNumber);//打印至日志

                List<byte[]> SliceCache = new();//文件分片缓存集合
                byte[] bytes;//存储读取结果  
                FileStream fs = new FileStream(FilesPath, FileMode.Open, FileAccess.Read);//打开文件
                long left = fs.Length;//尚未读取的文件内容长度
                long Remaining = 0;//分片末尾剩余
                if (new System.IO.FileInfo(FilesPath).Length < PUT_UploadFilesReturnJson?.data.chunkSize)//判断文件大小是否超过分片大小
                    bytes = new byte[new System.IO.FileInfo(FilesPath).Length];
                else
                {
                    bytes = new byte[(int)PUT_UploadFilesReturnJson?.data.chunkSize!];
                    Remaining = fs.Length % (int)PUT_UploadFilesReturnJson?.data.chunkSize!;
                }
                int maxLength = bytes.Length;//每次读取长度  
                int start = 0;//读取位置  
                int num;//实际返回结果长度  
                while (left > 0)//当文件未读取长度大于0时，不断进行读取  
                {
                    fs.Position = start;
                    num = 0;
                    if (left < maxLength)//末尾分片
                    {
                        byte[] RemainingByte = new byte[Remaining];
                        num = fs.Read(RemainingByte, 0, Convert.ToInt32(left));
                        SliceCache.Add(RemainingByte);
                    }
                    else
                    {
                        num = fs.Read(bytes, 0, maxLength);
                        SliceCache.Add(bytes);
                    }
                    if (num == 0)
                        break;
                    start += num;
                    left -= num;
                }
                fs.Close();
                for (int i = 0; i < SliceCache.Count; i++)
                {
                    HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/file/upload/" + sessionID + "/" + i, cookie: cookie, httpMod: HttpMods.POST_UPDATA, dataByte: SliceCache[i]);
                    Logger.WriteInfor("上传成功!(POST) 任务ID:" + sessionID + " 分片:" + i + " #文件名:" + System.IO.Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                }
            }
            else
            {
                HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/file/upload/" + sessionID + "/" + Slice, cookie: cookie, httpMod: HttpMods.POST_UPDATA, data: FilesPath);
            }

            if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
            {
                Logger.WriteError("发送上传请求失败(POST)!因为上面的原因...");//打印至日志
                if (ScreenOut)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("发送上传请求失败(POST)!因为上面的原因...");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return null;
            }
            POST_UploadFilesReturnJson? POST_UploadFilesReturnJson = JsonSerializer.Deserialize<POST_UploadFilesReturnJson>(HttpRequestToStringData);
            if (POST_UploadFilesReturnJson?.code != 0)
            {
                Logger.WriteError("上传失败(POST)!因为:" + POST_UploadFilesReturnJson?.msg + " 任务ID:" + sessionID + " #文件名:" + System.IO.Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);//打印至日志
                if (ScreenOut)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("上传失败(POST)!因为:" + POST_UploadFilesReturnJson?.msg + " 任务ID:" + sessionID + " #文件名:" + System.IO.Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (POST_UploadFilesReturnJson?.code == 400011)//如果找不到任物就回传null
                {
                    return null;
                }
                return sessionID;
            }
            Logger.WriteInfor("上传任务完成! #文件名:" + System.IO.Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
            if (ScreenOut)
                Console.WriteLine("上传任务完成! #文件名:" + System.IO.Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
            return HttpRequestToStringData;
        }
    }
}
