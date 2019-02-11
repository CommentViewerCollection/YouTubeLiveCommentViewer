using System.Collections.Generic;
using System.Threading.Tasks;
using SitePlugin;
using System;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace YouTubeLiveCommentViewer.ViewModel
{
    public class YouTubeLiveCommentViewModel : IYouTubeLiveCommentViewModel
    {
        public ICommand CommentCopyCommand { get; }
        public ICommand OpenUrlCommand { get; }
        public ICommand UsernameCopyCommand { get; }
        public ICommand NicknameCopyCommand { get; }

        private readonly YouTubeLiveSitePlugin.IYouTubeLiveMessage _message;
        private readonly IMessageMetadata _metadata;
        private readonly IMessageMethods _methods;
        private void SetNickname(IUser user)
        {
            if (!string.IsNullOrEmpty(user.Nickname))
            {
                _nickItems = new List<IMessagePart> { Common.MessagePartFactory.CreateMessageText(user.Nickname) };
            }
            else
            {
                _nickItems = null;
            }
        }
        private YouTubeLiveCommentViewModel(IMessageMetadata metadata, IMessageMethods methods)
        {
            _metadata = metadata;
            _methods = methods;
            _metadata.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(_metadata.BackColor):
                        RaisePropertyChanged(nameof(Background));
                        break;
                    case nameof(_metadata.ForeColor):
                        RaisePropertyChanged(nameof(Foreground));
                        break;
                    case nameof(_metadata.FontFamily):
                        RaisePropertyChanged(nameof(FontFamily));
                        break;
                    case nameof(_metadata.FontStyle):
                        RaisePropertyChanged(nameof(FontStyle));
                        break;
                    case nameof(_metadata.FontWeight):
                        RaisePropertyChanged(nameof(FontWeight));
                        break;
                    case nameof(_metadata.FontSize):
                        RaisePropertyChanged(nameof(FontSize));
                        break;
                }
            };
            if (_metadata.User != null)
            {
                var user = _metadata.User;
                user.PropertyChanged += (s, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case nameof(user.Nickname):
                            SetNickname(user);
                            RaisePropertyChanged(nameof(NameItems));
                            break;
                    }
                };
                SetNickname(user);
            }
            CommentCopyCommand = new RelayCommand(CopyComment);
            OpenUrlCommand = new RelayCommand(OpenUrl);
            UsernameCopyCommand = new RelayCommand(CopyUsername);
            NicknameCopyCommand = new RelayCommand(CopyNickname);
        }
        public YouTubeLiveCommentViewModel(YouTubeLiveSitePlugin.IYouTubeLiveComment comment, IMessageMetadata metadata, IMessageMethods methods)
            : this(metadata, methods)
        {
            _message = comment;

            _nameItems = comment.NameItems;
            MessageItems = comment.CommentItems;
            Thumbnail = comment.UserIcon;
            Id = comment.Id.ToString();
            PostTime = comment.PostTime;


        }
        public YouTubeLiveCommentViewModel(YouTubeLiveSitePlugin.IYouTubeLiveSuperchat item, IMessageMetadata metadata, IMessageMethods methods)
            : this(metadata, methods)
        {
            var comment = item;
            _message = comment;

            _nameItems = comment.NameItems;
            MessageItems = comment.CommentItems;
            Thumbnail = comment.UserIcon;
            Id = comment.Id.ToString();
            PostTime = comment.PostTime;
        }
        public YouTubeLiveCommentViewModel(YouTubeLiveSitePlugin.IYouTubeLiveConnected connected, IMessageMetadata metadata, IMessageMethods methods)
            : this(metadata, methods)
        {
            _message = connected;
            MessageItems = connected.CommentItems;
        }
        public YouTubeLiveCommentViewModel(YouTubeLiveSitePlugin.IYouTubeLiveDisconnected disconnected, IMessageMetadata metadata, IMessageMethods methods)
            : this(metadata, methods)
        {
            _message = disconnected;
            MessageItems = disconnected.CommentItems;
        }

        private IEnumerable<IMessagePart> _nickItems { get; set; }
        private IEnumerable<IMessagePart> _nameItems { get; set; }
        public IEnumerable<IMessagePart> NameItems
        {
            get
            {
                if(_nickItems != null)
                {
                    return _nickItems;
                }
                else
                {
                    return _nameItems;
                }
            }
        }

        public IEnumerable<IMessagePart> MessageItems { get; private set; }

        public SolidColorBrush Background => new SolidColorBrush(_metadata.BackColor);

        public ICommentProvider CommentProvider => _metadata.CommentProvider;

        public FontFamily FontFamily => _metadata.FontFamily;

        public int FontSize => _metadata.FontSize;

        public FontStyle FontStyle => _metadata.FontStyle;

        public FontWeight FontWeight => _metadata.FontWeight;

        public SolidColorBrush Foreground => new SolidColorBrush(_metadata.ForeColor);

        public string Id { get; private set; }

        public string Info { get; private set; }

        public bool IsVisible => true;

        public string PostTime { get; private set; }

        public IMessageImage Thumbnail { get; private set; }

        public string UserId => _metadata.User?.UserId;

        public IUser User => _metadata.User;

        public Task AfterCommentAdded()
        {
            return Task.CompletedTask;
        }
        public TextWrapping UserNameWrapping
        {
            get
            {
                //TODO:Wrapにも対応しないと。
                return TextWrapping.NoWrap;
            }
        }
        public bool ContainsUrl
        {
            get
            {
                return !string.IsNullOrEmpty(GetUrlFromSelectedComment());
            }
        }
        public bool HasNickname
        {
            get
            {
                return _nickItems != null;
            }
        }
        private string GetUrlFromSelectedComment()
        {
            var message = MessageItems.ToText();
            var match = Regex.Match(message, "(https?://([\\w-]+.)+[\\w-]+(?:/[\\w- ./?%&=]))?");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }
        private void OpenUrl()
        {
            var url = GetUrlFromSelectedComment();
            Process.Start(url);
            //SetSystemInfo("open: " + url, InfoType.Debug);
        }
        private void CopyNickname()
        {
            var username = _nickItems?.ToText();
            if(string.IsNullOrEmpty(username))
            {
                return;
            }
            try
            {
                System.Windows.Clipboard.SetText(username);
            }
            catch (System.Runtime.InteropServices.COMException) { }
        }
        private void CopyUsername()
        {
            var username = _nameItems.ToText();
            try
            {
                System.Windows.Clipboard.SetText(username);
            }
            catch (System.Runtime.InteropServices.COMException) { }
        }
        private void CopyComment()
        {
            var message = MessageItems.ToText();
            try
            {
                System.Windows.Clipboard.SetText(message);
            }
            catch (System.Runtime.InteropServices.COMException) { }
            //SetSystemInfo("copy: " + message, InfoType.Debug);
        }
        #region INotifyPropertyChanged
        [NonSerialized]
        private System.ComponentModel.PropertyChangedEventHandler _propertyChanged;
        /// <summary>
        /// 
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            _propertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
    static class MessageParts
    {
        public static string ToText(this IEnumerable<IMessagePart> parts)
        {
            string s = "";
            if (parts != null)
            {
                foreach (var part in parts)
                {
                    if (part is IMessageText text)
                    {
                        s += text;
                    }
                }
            }
            return s;
        }
        public static string ToTextWithImageAlt(this IEnumerable<IMessagePart> parts)
        {
            string s = "";
            if (parts != null)
            {
                foreach (var part in parts)
                {
                    if (part is IMessageText text)
                    {
                        s += text;
                    }
                    else if (part is IMessageImage image)
                    {
                        s += image.Alt;
                    }
                }
            }
            return s;
        }
    }
}
