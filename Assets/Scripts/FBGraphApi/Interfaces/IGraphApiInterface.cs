using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.FBGraphApi.Structures;
using Assets.Scripts.RESTHandler;
using UnityEngine;

namespace Assets.Scripts.FBGraphApi.Interfaces
{
    public interface IGraphApiInterface
    {
        [GetRequest("https://graph.facebook.com/v2.8/me/?fields=id,name,location,hometown,currency&access_token={0}", 
            AnswerType = typeof(User), ParamNames = new []{ "accessToken" })]
        void GetBasicInfo(Action<object> callback, string accessToken);

        [GetRequest("https://graph.facebook.com/v2.8/me/friends?fields=id,name&access_token={0}",
            AnswerType = typeof(Friends), ParamNames = new []{"accessToken"})]
        void GetUserFriends(Action<object> callback, string accessToken);

        [GetRequest("https://graph.facebook.com/v2.8/{0}/picture?redirect=false&access_token={1}",
            AnswerType = typeof(UserAvatar), ParamNames = new []{ "userId", "accessToken" })]
        void GetUserAvatar(Action<object> callback, string accessToken, string userId);

        [GetRequest("https://graph.facebook.com/v2.8/me/invitable_friends?fields=id,name&access_token={0}",
            AnswerType = typeof(Friends), ParamNames = new[] { "accessToken" })]
        void GetUserInvitableFriends(Action<object> callback, string accessToken);
    }
}
