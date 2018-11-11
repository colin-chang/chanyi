using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chanyi.Dial.Utils
{
    public interface IDial
    {
        string UserId { get; set; }
        string Host { get; set; }
        string Port { get; set; }
        string Url { get; }

        /// <summary>
        /// 建立服务端会话
        /// </summary>
        /// <returns></returns>
        Task StartSession (Action<bool> completeHandler);

        /// <summary>
        /// 展示目前在线的Web用户
        /// </summary>
        /// <returns></returns>
        /// <param name="webUsers">Web users.</param>
        void ListWebUsers (IEnumerable<string> webUsers);

        /// <summary>
        /// 绑定Web用户BindWebUser
        /// </summary>
        /// <returns>The web user.</returns>
        /// <param name="userIds">User identifiers.</param>
        Task BindWebUser (IEnumerable<string> userIds, Action exceptionHandler);

        /// <summary>
        /// 重新绑定WEB用户
        /// </summary>
        Task ReBindWebUser (Action showOnlineUserDialog, Action exceptionHandler);

        /// <summary>
        /// 拨打指定URL对应的电话
        /// </summary>
        /// <param name="url"></param>
        Task Dial (string url);

        /// <summary>
        /// 拨打指定电话号码
        /// </summary>
        /// <returns>The phone number.</returns>
        /// <param name="phoneNumber">Phone number.</param>
        void CallPhoneNumber (string phoneNumber);

        /// <summary>
        /// 销毁连接和状态
        /// </summary>
        void Destroy ();


        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <returns>The dialog.</returns>
        /// <param name="message">Message.</param>
        void ShowDialog (string message);
    }
}

