using System;

namespace ToastifyAPI.Service
{
    [Serializable]
    public class ToastifyServiceMessage
    {
        public ToastifyServiceEvent @event;
        public object param;
    }
}