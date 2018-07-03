using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using SitePlugin;
using System.Threading;
using System.Collections.ObjectModel;
using Plugin;
using ryu_s.BrowserCookie;
using System.Diagnostics;
using System.Windows.Threading;
using System.Net;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Text.RegularExpressions;
using Common;

namespace YouTubeLiveCommentViewer
{
    /// <summary>
    /// MainViewModelとUserViewModelの共通項
    /// </summary>
    public abstract class CommentDataGridViewModelBase : ViewModelBase
    {
        public ICollectionView Comments { get; protected set; }
        public System.Windows.Controls.ScrollUnit ScrollUnit
        {
            get
            {
                if(_options.IsPixelScrolling)
                {
                    return System.Windows.Controls.ScrollUnit.Pixel;
                }
                else
                {
                    return System.Windows.Controls.ScrollUnit.Item;
                }
            }
        }
        public Brush HorizontalGridLineBrush
        {
            get { return new SolidColorBrush(_options.HorizontalGridLineColor); }
        }
        public Brush VerticalGridLineBrush
        {
            get { return new SolidColorBrush(_options.VerticalGridLineColor); }
        }
        public TextWrapping UserNameWrapping
        {
            get
            {
                if (_options.IsUserNameWrapping)
                    return TextWrapping.Wrap;
                else
                    return TextWrapping.NoWrap;
            }
        }
        public abstract bool IsShowUserInfoMenuItem { get; }
        public abstract bool IsShowThumbnailMenuItem { get; }
        public abstract bool IsShowUsernameMenuItem { get; }
        public abstract bool IsShowUserIdMenuItem { get; }
        #region Thumbnail
        public double ThumbnailWidth
        {
            get { return _options.ThumbnailWidth; }
            set { _options.ThumbnailWidth = value; }
        }
        public virtual bool IsShowThumbnail
        {
            get { return _options.IsShowThumbnail; }
            set { _options.IsShowThumbnail = value; }
        }
        public int ThumbnailDisplayIndex
        {
            get { return _options.ThumbnailDisplayIndex; }
            set { _options.ThumbnailDisplayIndex = value; }
        }
        #endregion

        #region Username
        public int UsernameDisplayIndex
        {
            get { return _options.UsernameDisplayIndex; }
            set { _options.UsernameDisplayIndex = value; }
        }
        public double UsernameWidth
        {
            get { return _options.UsernameWidth; }
            set { _options.UsernameWidth = value; }
        }
        public virtual bool IsShowUsername
        {
            get { return _options.IsShowUsername; }
            set { _options.IsShowUsername = value; }
        }
        #endregion

        #region UserId
        public int UserIdDisplayIndex
        {
            get { return _options.UserIdDisplayIndex; }
            set { _options.UserIdDisplayIndex = value; }
        }
        public double UserIdWidth
        {
            get { return _options.UserIdWidth; }
            set { _options.UserIdWidth = value; }
        }
        public bool IsShowUserId
        {
            get { return _options.IsShowUserId; }
            set { _options.IsShowUserId = value; }
        }
        #endregion

        #region PostTime
        public int PostTimeDisplayIndex
        {
            get { return _options.PostTimeDisplayIndex; }
            set { _options.PostTimeDisplayIndex = value; }
        }
        public double PostTimeWidth
        {
            get { return _options.PostTimeWidth; }
            set { _options.PostTimeWidth = value; }
        }
        public bool IsShowPostTime
        {
            get { return _options.IsShowPostTime; }
            set { _options.IsShowPostTime = value; }
        }
        #endregion

        #region Message
        public double MessageWidth
        {
            get { return _options.MessageWidth; }
            set { _options.MessageWidth = value; }
        }
        public bool IsShowMessage
        {
            get { return _options.IsShowMessage; }
            set { _options.IsShowMessage = value; }
        }
        public int MessageDisplayIndex
        {
            get { return _options.MessageDisplayIndex; }
            set { _options.MessageDisplayIndex = value; }
        }
        #endregion

        public Color SelectedRowBackColor
        {
            get { return _options.SelectedRowBackColor; }
            set { _options.SelectedRowBackColor = value; }
        }
        public Color SelectedRowForeColor
        {
            get { return _options.SelectedRowForeColor; }
            set { _options.SelectedRowForeColor = value; }
        }

        #region SelectedComment
        private ICommentViewModel _selectedComment;
        public ICommentViewModel SelectedComment
        {
            get
            {
                return _selectedComment;
            }
            set
            {
                if (_selectedComment == value)
                    return;
                _selectedComment = value;
            }
        }
        #endregion

        protected virtual void SetInfo(string message, InfoType type)
        {
            Debug.WriteLine(message);
        }
        private readonly IOptions _options;

        public CommentDataGridViewModelBase(IOptions options)
            :this(options, CollectionViewSource.GetDefaultView(new ObservableCollection<ICommentViewModel>()))
        {
        }
        public CommentDataGridViewModelBase(IOptions options,ICollectionView comments)
        {
            _options = options;
            Comments = comments;
            options.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(options.IsShowPostTime):
                        RaisePropertyChanged(nameof(IsShowPostTime));
                        break;
                }
            };
        }


    }
}
