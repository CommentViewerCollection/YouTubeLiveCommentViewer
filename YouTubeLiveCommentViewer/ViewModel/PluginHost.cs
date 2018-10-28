using SitePlugin;
using Plugin;
using System.Collections.Generic;

namespace YouTubeLiveCommentViewer.ViewModel
{
    public class PluginHost : IPluginHost
    {
        public string SettingsDirPath => _options.SettingsDirPath;

        public double MainViewLeft => _options.MainViewLeft;

        public double MainViewTop => _options.MainViewTop;

        public bool IsTopmost => _options.IsTopmost;
        public string LoadOptions(string path)
        {
            var s = _io.ReadFile(path);
            return s;
        }

        public void SaveOptions(string path, string s)
        {
            _io.WriteFile(path, s);
        }

        public void PostCommentToAll(string comment)
        {
            throw new System.NotImplementedException();
        }

        public void PostComment(string guid, string comment)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IConnectionStatus> GetAllConnectionStatus()
        {
            throw new System.NotImplementedException();
        }

        private readonly MainViewModel _vm;
        private readonly IOptions _options;
        private readonly IIo _io;
        public PluginHost(MainViewModel vm, IOptions options, IIo io)
        {
            _vm = vm;
            _options = options;
            _io = io;
        }
    }
}
