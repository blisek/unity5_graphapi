using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FBGraphApi.Structures
{
    [Serializable]
    public class ProfilePictureSource
    {
        /// <summary>
        /// Czy jest to obrazek domyślny?
        /// </summary>
        public bool is_silhouette;

        public string url;
    }
}
