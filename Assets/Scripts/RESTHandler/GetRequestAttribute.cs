using System;

namespace Assets.Scripts.RESTHandler
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class GetRequestAttribute : Attribute
    {
        public string RequestUrl { get; private set; }

        public string[] ParamNames { get; set; }

        public Type AnswerType { get; set; }

        public string CallbackArgumentName { get; private set; }

        public GetRequestAttribute(string requestUrl, string callbackArgumentName = "callback")
        {
            RequestUrl = requestUrl;
            CallbackArgumentName = callbackArgumentName;
            
            if(ParamNames == null)
                ParamNames = new string[0];
        }
    }
}
