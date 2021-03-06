﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security;
using System.Threading;
using Newtonsoft.Json;
using RatherWeird.Utility;

namespace RatherWeird
{
    
    /// <summary>
    /// Interaktionslogik für Ra3Playerlist.xaml
    /// </summary>
    public partial class Ra3Playerlist : UserControl
    {
        private CancellationTokenSource _tokenSource;
        private readonly ObservableCollection<User> _ra3Users = new ObservableCollection<User>();
        
        public Ra3Playerlist()
        {
            InitializeComponent();

            lstView.ItemsSource = _ra3Users;
        }

        public void StartReoccuringTask()
        {
            if (_tokenSource != null && !_tokenSource.IsCancellationRequested)
            {
                // previous task was *NOT* cancelled so we just go home here
                Logger.Debug("Attempted to start a second task but that was supressed (misuse and such)");
                return;
            }

            Logger.Info("Starting Reoccuring task");
            _tokenSource = new CancellationTokenSource();
            _tokenSource.Token.ThrowIfCancellationRequested();
            Task.Run(async () =>
            {
                while (!_tokenSource.IsCancellationRequested)
                {
                    Logger.Debug("Attempting to download and insert player statistics");
                    CncGeneralInfo info = await GatherData();
                    if (info != null)
                    {
                        InsertData(info);
                    }
                    
                    Thread.Sleep(1000 * 60);
                }
            }, _tokenSource.Token);
        }

        public void StopReoccuringTask()
        {
            Logger.Info("Stopping Reoccurring task for shutdown");
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        private async Task<CncGeneralInfo> GatherData()
        {
            try
            {
                WebRequest req = WebRequest.CreateHttp(Constants.CncOnlinePlayerInfo);

                var res = await req.GetResponseAsync();
                HttpWebResponse response = (HttpWebResponse) res;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Error($"ER.. Could not get a proper status. Status Code was ${response.StatusCode}");
                    return null;
                }

                CncGeneralInfo info = new CncGeneralInfo();

                using (Stream streamResponse = response.GetResponseStream())
                using (StreamReader sr =
                    new StreamReader(streamResponse ?? throw new InvalidOperationException("streamResponse was null")))
                {
                    string body = await sr.ReadToEndAsync();
                    info = JsonConvert.DeserializeObject<CncGeneralInfo>(body);
                }

                return info;
            }
            catch (SecurityException err)
            {
                Logger.Error($"ER.. Could not connect to Server due to permission issues. err: {err.Message}");
            }
            catch (ProtocolViolationException err)
            {
                Logger.Error($"ER.. Could not get any response stream. err: {err.Message}");
            }
            catch (InvalidOperationException err)
            {
                Logger.Error($"ER.. Seems like the responsestream was null for some reason. err: {err.Message}");
            }
            catch (Exception err)
            {
                Logger.Fatal($"ER.. Seems like something unexpected happened. err: {err.Message}");
            }

            return null;
        }

        private int _oldPlayerCount;
        private void InsertData(CncGeneralInfo info)
        {
            _oldPlayerCount = _ra3Users.Count;
            lstView.Dispatcher.Invoke(() =>
            {
                var newEntries = info.Ra3.Users.Values.Except(_ra3Users);
                var oldEntries = _ra3Users.Except(info.Ra3.Users.Values);
                foreach (var newEntry in newEntries)
                {
                    AddSorted(_ra3Users, newEntry);
                }

                foreach (var o in oldEntries.ToList())
                {
                    _ra3Users.Remove(o);
                }
            });

            lstView.Dispatcher.Invoke(() =>
            {
                if (_oldPlayerCount != _ra3Users.Count)
                {
                    lblPlayers.Content = $"Players: {_ra3Users.Count}";
                }
            });
        }

        public static void AddSorted<T>(IList<T> list, T item)
            where T : IComparable
        {
            int i = 0;
            while (i < list.Count && item.CompareTo(list[i]) > 0)
                i++;

            list.Insert(i, item);
        }

    }


    
    [DataContract]
    internal class CncGeneralInfo
    {
        [DataMember(Name = "bfme")]
        public Game Bfme { get; set; }
        [DataMember(Name = "bfme2")]
        public Game Bfme2 { get; set; }
        [DataMember(Name = "cnc3")]
        public Game Cnc3 { get; set; }
        [DataMember(Name = "cnc3kw")]
        public Game Cnc3Kw { get; set; }
        [DataMember(Name = "generals")]
        public Game Generals { get; set; }
        [DataMember(Name = "generalzh")]
        public Game Generalszh { get; set; }
        [DataMember(Name = "ra3")]
        public Game Ra3 { get; set; }
        [DataMember(Name = "rotwk")]
        public Game Rotwk { get; set; }
    }

    [DataContract]
    internal class Game
    {
        [DataMember(Name = "lobbies")]
        public Lobby Lobbies { get; set; }
        [DataMember(Name = "users")]
        public IDictionary<string, User> Users { get; set; }
        [DataMember(Name = "games")]
        public MetaMatch Games { get; set; }
    }
    [DataContract]
    internal class Lobby
    {
        [DataMember(Name = "chat")]
        public int Chat { get; set; }
        [DataMember(Name = "hosting")]
        public int Hosting { get; set; }
        [DataMember(Name = "playing")]
        public int Playing { get; set; }
    }

    [DataContract]
    internal class User : IComparable
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "pid")]
        public int Pid { get; set; }
        
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Nickname.GetHashCode() ^ Pid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as User;
            if (other == null)
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }

            if (!other.Id.Equals(Id))
            {
                return false;
            }

            if (!other.Pid.Equals(Pid))
            {
                return false;
            }

            if (!other.Nickname.Equals(Nickname))
            {
                return false;
            }

            return true;
        }

        public int CompareTo(object obj)
        {
            User other = obj as User;
            if (other == null)
            {
                return 0;
            }

            int iNickname = String.Compare(Nickname, other.Nickname, StringComparison.InvariantCulture);
            if (iNickname != 0)
                return iNickname;

            int iId = Id.CompareTo(other.Id);
            if (iId != 0)
                return iId;

            int iPid = Pid.CompareTo(other.Pid);
            if (iPid != 0)
                return iPid;

            return 0;
        }

    }

    [DataContract]
    internal class MetaMatch
    {
        [DataMember(Name = "playing")]
        public int[] Playing { get; set; }
        [DataMember(Name = "staging")]
        public int[] Staging { get; set; }
    }

    [DataContract]
    internal class Match
    {
        [DataMember(Name = "cmdCRC")]
        public string CmdCRC { get; set; }
        [DataMember(Name = "exeCRC")]
        public string ExeCRC { get; set; }
        [DataMember(Name = "gamever")]
        public string Gamever { get; set; }
        [DataMember(Name = "host")]
        public User Host { get; set; }
        [DataMember(Name = "iniCRC")]
        public string IniCRC { get; set; }
        [DataMember(Name = "map")]
        public string Map { get; set; }
        [DataMember(Name = "maxRealPlayers")]
        public string MaxRealPlayers { get; set; }
        [DataMember(Name = "maxplayers")]
        public string Maxplayers { get; set; }
        [DataMember(Name = "numObservers")]
        public string NumObservers { get; set; }
        [DataMember(Name = "numRealPlayers")]
        public string NumRealPlayers { get; set; }
        [DataMember(Name = "numplayers")]
        public string Numplayers { get; set; }
        [DataMember(Name = "obs")]
        public string Obs { get; set; }
        [DataMember(Name = "pings")]
        public string Pings { get; set; }
        [DataMember(Name = "pw")]
        public string Pw { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
        [DataMember(Name = "players")]
        public User[] Players { get; set; }
    }
}
