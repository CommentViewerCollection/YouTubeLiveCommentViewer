using SitePlugin;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Common;
using System.Collections.Generic;
using System.Windows;

namespace YouTubeLiveCommentViewer.ViewModel
{
    public class UserViewModel : CommentDataGridViewModelBase
    {
        public string UserId { get { return User.UserId; } }
        public string Nickname
        {
            get { return _user.Nickname; }
            set
            {
                _user.Nickname = value;
            }
        }
        public IEnumerable<IMessagePart> NameItems => _user.Name;
        private readonly IUser _user;
        public override bool IsShowThumbnail { get => false; set { } }
        public override bool IsShowUsername { get => false; set { } }
        public override bool IsShowUserInfoMenuItem => false;
        public override bool IsShowThumbnailMenuItem => false;
        public override bool IsShowUserIdMenuItem => false;
        public override bool IsShowUsernameMenuItem => false;

        public IUser User { get { return _user; } }
        public UserViewModel(IUser user, IOptions option) : base(option)
        {
            _user = user;
        }
        public UserViewModel(IUser user, IOptions option, ICollectionView comments) 
            : base(option, comments)
        {
            _user = user;
        }
        public UserViewModel() : base(new DynamicOptionsTest())
        {
            if (IsInDesignMode)
            {
                _user = new UserTest("userid_123456")
                {
                    Nickname = "NICKNAME",
                    Name = new List<IMessagePart>
                    {
                         MessagePartFactory.CreateMessageText("test"),
                    },
                    BackColorArgb = "#FFCFCFCF",
                    ForeColorArgb = "#FF000000",
                };
            }
        }
    }
}
