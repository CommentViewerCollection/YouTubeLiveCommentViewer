using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common.Wpf;
using GalaSoft.MvvmLight.Messaging;

namespace YouTubeLiveCommentViewer.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OptionsView optionsView;
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<MainViewCloseMessage>(this, message =>
            {
                this.Close();
            });
            Messenger.Default.Register<SetAddingCommentDirection>(this, message =>
            {
                _addingCommentToTop = message.IsTop;
            });
            Messenger.Default.Register<SetPostCommentPanel>(this, message =>
            {
                PostCommentPanelPlaceHolder.Children.Clear();

                var newPanel = message.Panel;
                if (newPanel == null)
                {
                    PostCommentPanelPlaceHolder.IsEnabled = false;
                }
                else
                {
                    PostCommentPanelPlaceHolder.IsEnabled = true;
                    newPanel.Margin = new Thickness(0);
                    newPanel.VerticalAlignment = VerticalAlignment.Stretch;
                    newPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    newPanel.Width = double.NaN;
                    newPanel.Height = double.NaN;
                    PostCommentPanelPlaceHolder.Children.Add(newPanel);
                }
            });
            Messenger.Default.Register<ShowOptionsViewMessage>(this, message =>
            {
                try
                {
                    if (optionsView == null)
                    {
                        optionsView = new OptionsView();
                        optionsView.Owner = this;
                    }
                    optionsView.Clear();
                    foreach (var tab in message.Tabs)
                    {
                        optionsView.AddTabPage(tab);
                    }
                    
                    var showPos = Tools.GetShowPos(Tools.GetMousePos(), optionsView);
                    optionsView.Left = showPos.X;
                    optionsView.Top = showPos.Y;
                    optionsView.Show();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                }
            });
            Messenger.Default.Register<Common.AutoUpdate.ShowUpdateDialogMessage>(this, message =>
            {
                try
                {
                    var updateView = new Common.AutoUpdate.UpdateView();
                    var showPos = Tools.GetShowPos(Tools.GetMousePos(), updateView);
                    updateView.Left = showPos.X;
                    updateView.Top = showPos.Y;
                    updateView.IsUpdateExists = message.IsUpdateExists;
                    updateView.CurrentVersion = message.CurrentVersion;
                    updateView.LatestVersionInfo = message.LatestVersionInfo;
                    updateView.Owner = this;
                    updateView.Logger = message.Logger;
                    updateView.ShowDialog();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
            Messenger.Default.Register<ShowUserViewMessage>(this, message =>
            {
                try
                {
                    var uvm = message.Uvm;
                    var userView = new UserView
                    {
                        DataContext = uvm
                    };
                    userView.Show();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        //protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        //{
        //    dataGrid.get
        //    //dataGrid.ItemContainerGenerator.ContainerFromIndex(e.)
        //    base.OnMouseRightButtonUp(e);
        //}
        private bool _addingCommentToTop;
        private bool _bottom = true;
        //private bool neverTouch = true;
        private void DataGridScrollChanged(object sender, RoutedEventArgs e)
        {
            //if (_addingCommentToTop)
            //    return;
            if (sender == null)
                return;
            ScrollViewer scrollViewer;
            if (sender is DataGrid dataGrid)
            {
                scrollViewer = dataGrid.GetScrollViewer();
            }
            else if (sender is ScrollViewer)
            {
                scrollViewer = sender as ScrollViewer;
            }
            else
            {
                return;
            }
            var a = e as ScrollChangedEventArgs;


            ////2017/09/11
            ////ExtentHeightは表示されていない部分も含めた全てのコンテントの高さ。
            ////ScrollChangedが呼び出されたのにExtentHeightChangeが0ということはアイテムが追加されていないのにも関わらずスクロールがあった。
            ////それはユーザが手動でスクロールした場合のみ起こること。
            //if (a.ExtentHeightChange == 0)
            //{
            //    //ユーザが手動でスクロールした
            //    _bottom = scrollViewer.IsBottom();
            //    //neverTouch = false;
            //}

            ////2017/09/11全体の高さが表示部に収まる間はスクロールがBottomにあるとみなすと、表示部に収まらなくなった瞬間にもBottomにあると判定されて、最初のスクロールが上手くいくかも。

            ////if (bottom && a.ExtentHeightChange != 0)
            //if (_bottom && Test(a))
            //{
            //    scrollViewer.ScrollToBottom();
            //}
            AutoScrollTest(scrollViewer, a.ExtentHeightChange);
        }
        private bool AutoScroll = true;
        private void AutoScrollTest(ScrollViewer sc, double extentHeightChange)
        {
            // User scroll event : set or unset autoscroll mode
            if (extentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (sc.VerticalOffset == sc.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set autoscroll mode
                    AutoScroll = true;
                    Debug.WriteLine("Autoscroll=true");
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset autoscroll mode
                    AutoScroll = false;
                    Debug.WriteLine("Autoscroll=false");
                }
            }

            // Content scroll event : autoscroll eventually
            if (AutoScroll && extentHeightChange != 0)
            {   // Content changed and autoscroll mode set
                // Autoscroll
                sc.ScrollToVerticalOffset(sc.ExtentHeight);
            }
        }
        private bool Test(ScrollChangedEventArgs e)
        {
            return e.ViewportHeightChange > 0 || e.ExtentHeightChange > 0 || e.ViewportHeightChange < 0 || e.ExtentHeightChange < 0;
        }
    }
    public static class DataGridBehavior
    {
        public static ScrollViewer GetScrollViewer(this DataGrid dataGrid)
        {
            return dataGrid.Template.FindName("DG_ScrollViewer", dataGrid) as ScrollViewer;
        }
    }
    public static class ScrollViewerBehavior
    {
        public static bool IsBottom(this ScrollViewer sv)
        {
            //var b = (sv.VerticalOffset * 1.01) > sv.ScrollableHeight;
            var b = (sv.VerticalOffset >= sv.ScrollableHeight
                || sv.ExtentHeight < sv.ViewportHeight);
            return b;
        }
    }
}
