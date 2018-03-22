using System;

namespace ToastifyService
{
    [Serializable]
    public class ToastifyServiceMessage
    {
        public ToastifyServiceEvent @event;
        public object param;
    }
}