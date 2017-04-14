using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Assets.Scripts.FBGraphApi.Exceptions;
using Assets.Scripts.FBGraphApi.Structures;
using UnityEngine;

namespace Assets.Scripts.RESTHandler
{
    public static class WebApiHandlerBuilder
    {
        public static T ConstructWebApiHandler<T>(MonoBehaviour context, Action<Exception> exceptionsHandler) where T : class
        {
            if(context == null)
                throw new ArgumentNullException("context");
            if(exceptionsHandler == null)
                throw new ArgumentNullException("exceptionsHandler");
            return new WebApiCallsProxy(typeof(T), context, exceptionsHandler).GetTransparentProxy() as T;
        }
    }

    internal class WebApiCallsProxy : RealProxy
    {
        private readonly Dictionary<string, ParsedMethodInfo> _parsedMethodInfos;
        private readonly MonoBehaviour _context;
        private readonly Action<Exception> _exceptionsHandler;

        public WebApiCallsProxy(Type type, MonoBehaviour context, Action<Exception> exceptionsHandler) : base(type)
        {
            _context = context;
            _exceptionsHandler = exceptionsHandler;
            var methods = type.GetMethods();
            _parsedMethodInfos = new Dictionary<string, ParsedMethodInfo>(methods.Length);
            foreach (var methodInfo in methods)
                ParseMethodInfo(methodInfo);
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCallMessage = msg as IMethodCallMessage;
            if(methodCallMessage != null)
                return HandleMethodCall(methodCallMessage);

            throw new SystemException("This proxy handle only method calls.");
        }

        private IMessage HandleMethodCall(IMethodCallMessage methodCall)
        {
            try
            {
                ParsedMethodInfo pmi;
                if (!_parsedMethodInfos.TryGetValue(methodCall.MethodName, out pmi))
                {
                    // TODO: log it
                    throw new SystemException(string.Format("Method {0} is unsupported", methodCall.MethodName));
                }

                if (pmi.ArgumentsOrder.Length > 0)
                    return HandleRequestWithParameters(methodCall, pmi);
                else
                    return HandleRequestWithoutParameters(methodCall, pmi);

            }
            catch (Exception ex)
            {
                return new ReturnMessage(ex, methodCall);
            }
        }

        private IMessage HandleRequestWithoutParameters(IMethodCallMessage methodCall, ParsedMethodInfo pmi)
        {
            var fullRequest = pmi.RequestUrl;
            var callback = FindCallbackMethod(methodCall, pmi.CallbackArgumentName);
            if(callback == null)
                throw new SystemException("No callback reference found!");
            _context.StartCoroutine(DownloadDataCoroutine(callback, pmi.ReturnType, fullRequest));

            return new ReturnMessage(null, null, 0, methodCall.LogicalCallContext, methodCall);
        }

        private IMessage HandleRequestWithParameters(IMethodCallMessage methodCall, ParsedMethodInfo pmi)
        {
            var deliveredArguments = new string[methodCall.InArgCount];
            for (var i = 0; i < deliveredArguments.Length; ++i)
                deliveredArguments[i] = methodCall.GetArgName(i);

            var arguments = new object[pmi.ArgumentsOrder.Length];
            for (var i = 0; i < arguments.Length; ++i)
            {
                var arg = pmi.ArgumentsOrder[i];
                var index = Array.FindIndex(deliveredArguments, expectedArgPosition => expectedArgPosition == arg);
                if (index >= 0)
                    arguments[i] = methodCall.GetInArg(index);
                else
                    arguments[i] = null;
            }
            
            Action<object> callback = FindCallbackMethod(methodCall, pmi.CallbackArgumentName);
            if(callback == null)
                throw new SystemException("No callback reference found!");
            var fullRequest = string.Format(pmi.RequestUrl, arguments);
            _context.StartCoroutine(DownloadDataCoroutine(callback, pmi.ReturnType, fullRequest));

            return new ReturnMessage(null, null, 0, methodCall.LogicalCallContext, methodCall);
        }

        private Action<object> FindCallbackMethod(IMethodCallMessage methodCall, string callbackMethodName)
        {
            var argCount = methodCall.InArgCount;
            for(var index = 0; index < argCount; ++index)
                if (methodCall.GetInArgName(index) == callbackMethodName)
                    return methodCall.GetInArg(index) as Action<object>;
            return null;
        }
        

        private void ParseMethodInfo(MethodInfo methodInfo)
        {
            var getRequestAttribute = methodInfo.GetCustomAttribute(typeof(GetRequestAttribute)) as GetRequestAttribute;
            if (getRequestAttribute == null)
            {
                throw new SystemException(string.Format("Method {0} is unsupported", methodInfo.Name));
            }

            if (getRequestAttribute.RequestUrl == null)
                throw new SystemException("Request url is missing");

            var parsedMethodInfo = new ParsedMethodInfo()
            {
                RequestUrl = getRequestAttribute.RequestUrl,
                ArgumentsOrder = getRequestAttribute.ParamNames,
                CallbackArgumentName = getRequestAttribute.CallbackArgumentName,
                ReturnType = getRequestAttribute.AnswerType
            };
            _parsedMethodInfos.Add(methodInfo.Name, parsedMethodInfo);
        }

        private IEnumerator DownloadDataCoroutine(Action<object> callback, Type expectedType, string fullRequest)
        {
            object parsedResponse = null;
            WWW request = new WWW(fullRequest);
            yield return request;
            
            try
            {
                var text = request.text;
                var errorMsg = JsonUtility.FromJson(text, typeof(ErrorWrapper)) as ErrorWrapper;
                if (errorMsg != null && errorMsg.error.ErrorOccurred())
                {
                    _exceptionsHandler(new GraphApiCallException(errorMsg.error.code, errorMsg.error.message));
                    yield break;
                }
                else if (string.IsNullOrEmpty(text))
                {
                    _exceptionsHandler(new GraphApiCallException(-1, "Unknown error occurred"));
                    yield break;
                }

                parsedResponse = JsonUtility.FromJson(text, expectedType);
                request.Dispose();
            }
            catch (Exception ex)
            {
                _exceptionsHandler(ex);
            }
            callback(parsedResponse);
        }

        private struct ParsedMethodInfo
        {
            public string RequestUrl { get; set; }
            public string[] ArgumentsOrder { get; set; }
            public string CallbackArgumentName { get; set; }
            public Type ReturnType { get; set; }
        }
    }
}
