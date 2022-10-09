﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudreve.Json.User
{
    internal class DeleteUpFileListJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        /// <summary>
        /// 删除上传列队返回Json
        /// </summary>
        public class DeleteUpFileListReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string msg { get; set; }
        }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }
}
