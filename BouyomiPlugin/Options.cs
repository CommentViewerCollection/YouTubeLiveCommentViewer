﻿using System;
using System.ComponentModel;
using Common;
namespace BouyomiPlugin
{
    class Options : DynamicOptionsBase
    {
        public bool IsEnabled { get { return GetValue(); } set { SetValue(value); } }
        public string BouyomiChanPath { get { return GetValue(); } set { SetValue(value); } }
        public bool IsReadHandleName { get { return GetValue(); } set { SetValue(value); } }
        public bool IsReadComment { get { return GetValue(); } set { SetValue(value); } }
        public bool IsAppendNickTitle { get { return GetValue(); } set { SetValue(value); } }
        public string NickTitle { get { return GetValue(); } set { SetValue(value); } }
        public bool Want184Read { get { return GetValue(); } set { SetValue(value); } }

        public bool IsYouTubeLiveConnect { get { return GetValue(); } set { SetValue(value); } }
        public bool IsYouTubeLiveDisconnect { get { return GetValue(); } set { SetValue(value); } }
        public bool IsYouTubeLiveComment { get { return GetValue(); } set { SetValue(value); } }
        public bool IsYouTubeLiveCommentNickname { get { return GetValue(); } set { SetValue(value); } }
        /// <summary>
        /// YouTubeLiveのスタンプのaltを読み上げるか
        /// </summary>
        public bool IsYouTubeLiveCommentStamp { get { return GetValue(); } set { SetValue(value); } }
        public bool IsYouTubeLiveSuperchat { get { return GetValue(); } set { SetValue(value); } }
        public bool IsYouTubeLiveSuperchatNickname { get { return GetValue(); } set { SetValue(value); } }

        protected override void Init()
        {
            Dict.Add(nameof(IsEnabled), new Item { DefaultValue = false, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(BouyomiChanPath), new Item { DefaultValue = "", Predicate = s => true, Serializer = s => s, Deserializer = s => s });
            Dict.Add(nameof(IsReadHandleName), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsReadComment), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsAppendNickTitle), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(NickTitle), new Item { DefaultValue = "さん", Predicate = s => true, Serializer = s => s, Deserializer = s => s });
            Dict.Add(nameof(Want184Read), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });

            Dict.Add(nameof(IsYouTubeLiveConnect), new Item { DefaultValue = false, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsYouTubeLiveDisconnect), new Item { DefaultValue = false, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsYouTubeLiveComment), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsYouTubeLiveCommentNickname), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsYouTubeLiveCommentStamp), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsYouTubeLiveSuperchat), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });
            Dict.Add(nameof(IsYouTubeLiveSuperchatNickname), new Item { DefaultValue = true, Predicate = b => true, Serializer = b => b.ToString(), Deserializer = s => bool.Parse(s) });

        }
    }
}
