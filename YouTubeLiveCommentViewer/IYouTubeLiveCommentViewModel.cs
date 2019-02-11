using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SitePlugin;

namespace YouTubeLiveCommentViewer
{
    public interface IYouTubeLiveCommentViewModel: INotifyPropertyChanged
    {
        SolidColorBrush Background { get; }
        ICommentProvider CommentProvider { get; }
        FontFamily FontFamily { get; }
        int FontSize { get; }
        FontStyle FontStyle { get; }
        FontWeight FontWeight { get; }
        SolidColorBrush Foreground { get; }
        string Id { get; }
        string Info { get; }
        bool IsVisible { get; }
        IEnumerable<IMessagePart> MessageItems { get; }
        IEnumerable<IMessagePart> NameItems { get; }
        string PostTime { get; }
        IMessageImage Thumbnail { get; }
        string UserId { get; }
        IUser User { get; }

        event PropertyChangedEventHandler PropertyChanged;

        Task AfterCommentAdded();
    }
}