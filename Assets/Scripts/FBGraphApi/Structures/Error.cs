using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FBGraphApi.Structures
{
    /// <summary>
    /// Do obsługi błędów zwracanych przez api
    /// </summary>
    [Serializable]
    public class Error
    {
        public string message;
        public string type;
        public int code;
        public string fbtrace_id;
    }

    public static class ErrorExtension
    {
        public static bool ErrorOccurred(this Error error)
        {
            return !string.IsNullOrEmpty(error.message)
                   && !string.IsNullOrEmpty(error.type)
                   && error.code != 0
                   && !string.IsNullOrEmpty(error.fbtrace_id);
        }
    }
}
