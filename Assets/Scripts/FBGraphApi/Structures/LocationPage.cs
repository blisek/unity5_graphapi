using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FBGraphApi.Structures
{
    /// <summary>
    /// Do informacji związanych z miejscem pobytu
    /// (chociaż generalnie Page jest bardziej ogólnym typem)
    /// </summary>
    [Serializable]
    public class LocationPage
    {
        public string id;
        public string name;
    }
}
