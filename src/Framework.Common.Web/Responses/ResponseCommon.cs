using System.Collections.Generic;
using Framework.Common.Result;

namespace Framework.Common.Web.Responses
{
    public class ResponseCommon
    {
        public ResponseCommon()
        {
            Notifications = new List<KeyValuePair<NotificationType, string>>();
        }

        public ResponseCommon(string message)
        {
            Message = message;
            Notifications = new List<KeyValuePair<NotificationType, string>>();
        }

        public ResponseCommon(List<KeyValuePair<NotificationType, string>> notifications)
        {
            Notifications = notifications;
        }

        public ResponseCommon(string message, List<KeyValuePair<NotificationType, string>> notifications)
        {
            Message = message;
            Notifications = notifications;
        }

        public string Message { get; set; }

        public List<KeyValuePair<NotificationType, string>> Notifications { get; set; }

        public void AddNotifications(List<KeyValuePair<NotificationType, string>> notifications)
        {
            Notifications.AddRange(notifications);
        }
    }

    public class ResponseCommon<T> : ResponseCommon
    {
        public ResponseCommon()
        {
        }

        public ResponseCommon(T data)
        {
            Data = data;
        }

        public ResponseCommon(string message) : base(message)
        {
        }

        public ResponseCommon(List<KeyValuePair<NotificationType, string>> notifications) : base(notifications)
        {
        }

        public ResponseCommon(T data, string message) : base(message)
        {
            Data = data;
        }

        public ResponseCommon(T data, List<KeyValuePair<NotificationType, string>> notifications) : base(notifications)
        {
            Data = data;
        }

        public ResponseCommon(string message,
            List<KeyValuePair<NotificationType, string>> notifications) : base(message, notifications)
        {
        }

        public ResponseCommon(T data, string message,
            List<KeyValuePair<NotificationType, string>> notifications) : base(message, notifications)
        {
            Data = data;
        }


        public T Data { get; set; }
    }
}
