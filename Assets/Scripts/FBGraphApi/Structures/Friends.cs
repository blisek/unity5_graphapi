using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FBGraphApi.Structures
{
    [Serializable]
    public class Friends
    {
        public User[] data;
        public UserSummary summary;
    }

    [Serializable]
    public class UserSummary
    {
        public int total_count;
    }
}
