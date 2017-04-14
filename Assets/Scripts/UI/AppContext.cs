using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.Scripts.FBGraphApi.Interfaces;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.UI
{
    public static class AppContext
    {
        public static IGraphApiInterface GraphApiInterface { get; set; }

        public static string AccessToken { get; set; }

        public static void GlobalExceptionHandler(Exception exception)
        {
            Debug.LogError(string.Format("Exception occurred: {0}", exception.Message));
        }
    }
}
