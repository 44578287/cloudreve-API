using cloudreve.API;
using LoongEgg.LoongLogger;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using static cloudreve.Json.FileSourceJson;
using static cloudreve.Json.LoginJson;
using static cloudreve.Json.UploadFilesJson;
using static cloudreve.MODS.NetworkRequest;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数

string ApiUrl = "http://127.0.0.1:5212";
string? Cookie = null;
LoginDataJson logindata = new()
{
    UserName = "g9964957@gmail.com",
    Password = "admin",
    CaptchaCode = ""
};



Cookie = CloudreveAPI.Login(ApiUrl, logindata);//登入并获取Cookie


//var path = @"C:\Users\g9964\Pictures\screenshots\khl20220911185044994.png";
//var path = @"C:\Users\g9964\Downloads\Compressed\nameless-deps-dist.zip";
//var path = @"D:\BaiduNetdiskDownload\virtio-win-0.1.221.iso";
//Console.WriteLine(CloudreveAPI.UpFile(ApiUrl, Cookie, "5gSn", path));

/*PUT_UploadFilesDataJson Upload = new();
string UploadJson = Upload.Updata(path, "5gSn", "/");
string? UploadReturnJson = HttpRequestToString(ApiUrl + "/api/v3/file/upload/", cookie: Cookie, httpMod: HttpMods.PUT, data: UploadJson);//发送上传请求

PUT_UploadFilesReturnJson? UploadFilesReturnJson = JsonSerializer.Deserialize<PUT_UploadFilesReturnJson>(UploadReturnJson);

//HttpRequestToString(ApiUrl + "/api/v3/file/upload/" + UploadFilesReturnJson?.data.sessionID + "/0", cookie: Cookie, httpMod: HttpMods.POST_UPDATA, data: path);*/


//CloudreveAPI.UpFile(ApiUrl,Cookie, "5gSn", path,CloudFilesPath:"/");//上传文件
//Console.WriteLine(CloudreveAPI.DeleteUpFileList(ApiUrl, Cookie,"123"));
//Console.WriteLine(CloudreveAPI.DirectoryShow(ApiUrl, Cookie));
//Console.WriteLine(CloudreveAPI.GetDownloadUrl(ApiUrl, Cookie, "ePhm")?.data);
//Console.WriteLine(CloudreveAPI.GetFileSource(ApiUrl, Cookie, new() { "ePhm","kOfE1" }));

/*List<string> strings = new() { "7","2","3" };

Console.WriteLine(strings.Exists(strings => strings == "5"));
Console.WriteLine(strings.Find(strings => strings == "7"));*/

//Console.WriteLine(CloudreveAPI.GetConfig(ApiUrl, Cookie)?.data?.user.user_name);
//Console.WriteLine(CloudreveAPI.DeleteFiles(ApiUrl, Cookie, new() { "yOWSD" }));

//Console.WriteLine(CloudreveAPI.GetFilesData(ApiUrl, Cookie,FilesID: "kOfE"));


//Console.WriteLine(CloudreveAPI.GetCloudDriveSize(ApiUrl, Cookie));
Console.WriteLine(CloudreveAPI.GetFileShare(ApiUrl, Cookie,"ePhm"));