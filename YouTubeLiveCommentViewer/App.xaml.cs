﻿using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using YouTubeLiveSitePlugin.Test2;
using Common;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Net.Http;
using System.Text;
using CommentViewerCommon;

namespace YouTubeLiveCommentViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        DynamicOptionsTest _options;
        IOTest _io;
        ILogger _logger;
        YouTubeLiveSiteContext _siteContext;
        private string GetOptionsPath()
        {
            return @"settings\options.txt";
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            base.OnStartup(e);
            _io = new IOTest();
            _logger = new LoggerTest();
            _options = new DynamicOptionsTest();
            try
            {
                var s = _io.ReadFile(GetOptionsPath());
                _options.Deserialize(s);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }

            try
            {
                if (System.IO.File.Exists("error.txt"))
                {
                    string errorContent;
                    using(var sr = new System.IO.StreamReader("error.txt"))
                    {
                        errorContent = sr.ReadToEnd();
                    }
                    SendErrorReport(errorContent);
                    System.IO.File.Delete("error.txt");
                }
            }
            catch { }

            _siteContext = new YouTubeLiveSiteContext(_options,new YouTubeLiveServer(), _logger);
            _siteContext.Init();
            var browserLoader = new BrowserLoader(_logger);
            var vm = new ViewModel.MainViewModel(_siteContext, _options, _io, _logger, browserLoader);
            var resource = Application.Current.Resources;
            var locator = resource["Locator"] as ViewModel.ViewModelLocator;
            locator.Main = vm;
        }



        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                var s = _options.Serialize();
                _io.WriteFile(GetOptionsPath(), s);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            try
            {
                _siteContext.Save();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            try
            {
                var s = _logger.GetExceptions();
                SendErrorReport(s);
            }
            catch (Exception ex)
            {
                //ここで例外が起きても送れない・・・。
                Debug.WriteLine(ex.Message);
            }
            base.OnExit(e);
        }
        private string GetUserAgent()
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var ver = asm.GetName().Version;
            var title = asm.GetName().Name;
            var s = $"{title} v{ver.Major}.{ver.Minor}.{ver.Build}";
            return s;
        }
        private void SendErrorReport(string errorData)
        {
            if (string.IsNullOrEmpty(errorData))
            {
                return;
            }
            var fileStreamContent = new StreamContent(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(errorData)));
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Add("User-Agent", GetUserAgent());
                formData.Add(fileStreamContent, "error",  GetUserAgent() + "_" + "error.txt");
                var t = client.PostAsync("http://int-main.net/upload", formData);
                var response = t.Result;
                if (!response.IsSuccessStatusCode)
                {
                }
                else
                {
                }
            }
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            _logger.LogException(ex, "UnhandledException");
            var s = _logger.GetExceptions();
            using(var sw=new System.IO.StreamWriter("error.txt", true))
            {
                sw.WriteLine(s);
            }
        }
    }
}
