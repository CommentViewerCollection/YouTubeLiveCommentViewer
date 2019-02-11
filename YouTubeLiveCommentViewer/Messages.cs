using SitePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YouTubeLiveCommentViewer.ViewModel;
namespace YouTubeLiveCommentViewer
{
    internal class MainViewCloseMessage : GalaSoft.MvvmLight.Messaging.MessageBase { }
    internal class SetAddingCommentDirection : GalaSoft.MvvmLight.Messaging.MessageBase
    {
        public bool IsTop { get; set; }
    }
    class ShowOptionsViewMessage : GalaSoft.MvvmLight.Messaging.MessageBase
    {
        public IEnumerable<IOptionsTabPage> Tabs { get; }
        public ShowOptionsViewMessage(IEnumerable<IOptionsTabPage> tabs)
        {
            Tabs = tabs;
        }
    }
    class ShowUserViewMessage : GalaSoft.MvvmLight.Messaging.MessageBase
    {
        public UserViewModel Uvm { get; }
        public ShowUserViewMessage(UserViewModel uvm)
        {
            Uvm = uvm;
        }
    }
    class SetPostCommentPanel : GalaSoft.MvvmLight.Messaging.MessageBase
    {
        public UserControl Panel { get; }
        public SetPostCommentPanel(UserControl panel)
        {
            Panel = panel;
        }
    }
}
