using Assets.Scripts.FBGraphApi.Exceptions;
using Assets.Scripts.FBGraphApi.Interfaces;
using Assets.Scripts.FBGraphApi.Structures;
using Assets.Scripts.RESTHandler;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InputTokenPanel : MonoBehaviour
    {
        public InputField inputField;
        public GameObject coroutinesContext;
        public GameObject userInputPanel;
        private UserCard userCard;

        void Start()
        {
            userCard = userInputPanel.GetComponent<UserCard>();
        }

        public void OnStartButtonClick()
        {
            var apiProxy = WebApiHandlerBuilder.ConstructWebApiHandler<IGraphApiInterface>(
                coroutinesContext.GetComponent<DummyScript>(), AppContext.GlobalExceptionHandler);

            var accessToken = inputField.text;
            AppContext.GraphApiInterface = apiProxy;
            AppContext.AccessToken = accessToken;

            apiProxy.GetBasicInfo(u =>
            {
                var user = u as User;
                userInputPanel.SetActive(true);
                userCard.Users.Add(user);
                LoadFriends(apiProxy, accessToken);
                userCard.NextPerson();
                gameObject.SetActive(false);
            }, accessToken);
        }

        private void LoadFriends(IGraphApiInterface gai, string token)
        {
            gai.GetUserFriends(friendsList =>
            {
                var friends = friendsList as Friends;

                if (friends.summary.total_count <= 0)
                {
                    Debug.LogWarning("No friends found.");
                    return;
                }

                foreach (var friend in friends.data)
                    userCard.Users.Add(friend);
            }, token);
        }
    }
}
