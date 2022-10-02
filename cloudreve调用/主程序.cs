using cloudreve调用.API;
using LoongEgg.LoongLogger;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using static cloudreve调用.Json.LoginJson;
using static cloudreve调用.Json.UploadFilesJson;
using static cloudreve调用.MODS.NetworkRequest;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数

string ApiUrl = "http://127.0.0.1:5212";
string? Cookie = null;
LoginDataJson logindata = new()

{
    UserName = "g9964957@gmail.com",
    Password = "SUao44578287",
    CaptchaCode = ""
};



Cookie = CloudreveAPI.Login(ApiUrl, logindata);//登入并获取Cookie


//var path = @"C:\Users\g9964\Pictures\screenshots\khl20220911185044994.png";
var path = @"C:\Users\g9964\Downloads\Compressed\nameless-deps-dist.zip";
//var path = @"D:\BaiduNetdiskDownload\virtio-win-0.1.221.iso";
//Console.WriteLine(CloudreveAPI.UpFile(ApiUrl, Cookie, "5gSn", path));

/*PUT_UploadFilesDataJson Upload = new();
string UploadJson = Upload.Updata(path, "5gSn", "/");
string? UploadReturnJson = HttpRequestToString(ApiUrl + "/api/v3/file/upload/", cookie: Cookie, httpMod: HttpMods.PUT, data: UploadJson);//发送上传请求

PUT_UploadFilesReturnJson? UploadFilesReturnJson = JsonSerializer.Deserialize<PUT_UploadFilesReturnJson>(UploadReturnJson);

//HttpRequestToString(ApiUrl + "/api/v3/file/upload/" + UploadFilesReturnJson?.data.sessionID + "/0", cookie: Cookie, httpMod: HttpMods.POST_UPDATA, data: path);*/


CloudreveAPI.UpFile(ApiUrl,Cookie, "5gSn", path,CloudFilesPath:"/");
