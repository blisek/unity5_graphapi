using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FBGraphApi.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UserCard : MonoBehaviour
    {
        [SerializeField] private Text nameField;

        [SerializeField] private Text hometownField;

        [SerializeField] private Text locationField;

        [SerializeField] private Text currencyTxtField;

        [SerializeField] private Text currencyCost;

        [SerializeField] private Image avatar;

        [SerializeField] private Button nextButton;

        [SerializeField] private Button previousButton;

        public string Name
        {
            get { return nameField.text; }
            set { nameField.text = value; }
        }

        public string Hometown
        {
            get { return hometownField.text; }
            set { hometownField.text = value; }
        }

        public string Location
        {
            get { return locationField.text; }
            set { locationField.text = value; }
        }

        public string CurrencyName
        {
            get { return currencyTxtField.text; }
            set { currencyTxtField.text = value; }
        }

        public string CurrencyCost
        {
            get { return currencyCost.text; }
            set { currencyCost.text = value; }
        }

        private int _currentIndex = -1;
        private List<User> _users = new List<User>();

        public IList<User> Users
        {
            get { return _users; }
        }

        public void SetPicture(string url)
        {
            StartCoroutine(DownloadPictureCoroutine(url));
        }


        public void NextPerson()
        {
            if (_currentIndex + 1 >= _users.Count)
            {
                nextButton.enabled = false;
                return;
            }

            ++_currentIndex;
            RefreshFields();
        }

        public void PreviousPerson()
        {
            if (_currentIndex - 1 < 0)
            {
                previousButton.enabled = false;
                return;
            }

            --_currentIndex;
            RefreshFields();
        }

        private void RefreshFields()
        {
            var presentedUser = _users[_currentIndex];
            RefreshAvatar();
            Name = presentedUser.name ?? "-";
            Hometown = presentedUser.hometown.name ?? "-";
            Location = presentedUser.location.name ?? "-";
            CurrencyName = string.Format("{0} -> USD", presentedUser.currency.user_currency);
            CurrencyCost = presentedUser.currency.usd_exchange;
        }

        private void RefreshAvatar()
        {
            if(AppContext.GraphApiInterface == null)
                return;

            var user = _users[_currentIndex];
            AppContext.GraphApiInterface.GetUserAvatar(RefreshAvatarCallback, AppContext.AccessToken, user.id);
        }

        private void RefreshAvatarCallback(object pictureInfo)
        {
            var _userAvatar = pictureInfo as UserAvatar;
            if (_userAvatar == null)
            {
                Debug.LogWarning("Received object isn't expected type (UserAvatar)");
                return;
            }

            if (_userAvatar.data.is_silhouette)
            {
                Debug.LogWarning("Default avatar found. Skipping.");
                return;
            }

            StartCoroutine(DownloadPictureCoroutine(_userAvatar.data.url));
        }

        private IEnumerator DownloadPictureCoroutine(string url)
        {
            WWW www = new WWW(url);
            yield return www;
        
            var texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
        
            var spriteRect = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, spriteRect, new Vector2(.5f, .5f), 100);
            avatar.sprite = sprite;

            www.Dispose();
        }
    }
}
