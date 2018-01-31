﻿using System.Collections.Generic;
using System.Linq;

namespace Framework.Common.Result
{
    public enum NotificationType : byte
    {
        Error,
        Success,
        Info,
        Warning
    }

    public abstract class ServiceResultBase
    {
        protected ServiceResultBase()
        {
            Notifications = new List<KeyValuePair<NotificationType, string>>();
            Errors = new Dictionary<string, ICollection<string>>();
        }

        public Dictionary<string, ICollection<string>> Errors { get; set; }

        public List<KeyValuePair<NotificationType, string>> Notifications { get; set; }

        public bool IsSuccess
        {
            get { return !Errors.Any() && Notifications.All(n => n.Key != NotificationType.Error); }
        }

        public void AddError(string key, string message)
        {
            if (Errors.ContainsKey(key))
            {
                Errors[key].Add(message);
                return;
            }

            Errors.Add(key, new List<string>() { message });
        }

        public void AddNotification(NotificationType type, string message)
        {
            Notifications.Add(new KeyValuePair<NotificationType, string>(type, message));
        }
    }

    public class ServiceResult : ServiceResultBase
    {
        
    }

    public class ServiceResult<T> :ServiceResultBase
    {
        public ServiceResult()
        {
            
        }

        public ServiceResult(T response) : base()
        {
            Response = response;
        }

        public T Response { get; set; }
    }
}
