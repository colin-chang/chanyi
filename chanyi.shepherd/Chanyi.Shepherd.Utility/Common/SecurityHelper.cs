using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Chanyi.Shepherd.Utility.Common
{
    public static class SecurityHelper
    {
        /// <summary>
        /// 计算字符串的SHA256值
        /// </summary>
        /// <param name="msg">要取值的字符串</param>
        /// <returns>字符串的SHA256值</returns>
        public static string GetSHA1FromString(string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] sha256Byte = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < sha256Byte.Length; i++)
                    sb.Append(sha256Byte[i].ToString("x2"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// 计算文件的SHA256值
        /// </summary>
        /// <param name="path">要取值的文件</param>
        /// <returns>文件的SHA256值</returns>
        public static string GetSHA256FromFile(string path)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] sha256Byte = sha256.ComputeHash(File.OpenRead(path));//将文件以文件流方式传入
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < sha256Byte.Length; i++)
                    sb.Append(sha256Byte[i].ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
