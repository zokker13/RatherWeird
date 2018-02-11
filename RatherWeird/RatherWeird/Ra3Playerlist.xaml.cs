using System;
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
using System.Threading;
using Newtonsoft.Json;

namespace RatherWeird
{
    
    /// <summary>
    /// Interaktionslogik für Ra3Playerlist.xaml
    /// </summary>
    public partial class Ra3Playerlist : UserControl
    {
        private CancellationTokenSource _tokenSource;
        private ObservableCollection<User> _ra3Users = new ObservableCollection<User>();
        public Ra3Playerlist()
        {
            InitializeComponent();

            lstView.ItemsSource = _ra3Users;
        }

        public void LaunchReoccuringTask()
        {
            _tokenSource = new CancellationTokenSource();

            async void Repeated(Task _)
            {
                CncGeneralInfo info = await GatherData();
                InsertData(info);
                await Task.Delay(1000 * 60, _tokenSource.Token).ContinueWith(_2 => Repeated(_2), _tokenSource.Token);
            }

            Task.Delay(1000, _tokenSource.Token).ContinueWith((Action<Task>) Repeated, _tokenSource.Token);
        }

        public void StopReoccuringTask()
        {
            _tokenSource.Cancel();
        }

        private async Task<CncGeneralInfo> GatherData()
        {
            CncGeneralInfo info;
            WebRequest req = WebRequest.CreateHttp(Constants.CncOnlinePlayerInfo);

            var res = await req.GetResponseAsync();
            HttpWebResponse response = (HttpWebResponse) res;

            using (Stream streamResponse = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(streamResponse))
            {
                string body = sr.ReadToEnd();
                info = JsonConvert.DeserializeObject<CncGeneralInfo>(body);
            }
            
            return info;

        }

        private void InsertData(CncGeneralInfo info)
        {
            lstView.Dispatcher.Invoke(() =>
            {
                foreach (var ra3User in info.ra3.users)
                {
                    if (!_ra3Users.Contains(ra3User.Value))
                    {
                        _ra3Users.Add(ra3User.Value);
                    }
                }


                for (int i = _ra3Users.Count - 1; i <= 0; i--)
                {
                    if (!info.ra3.users.Values.Contains(_ra3Users[i]))
                    {
                        _ra3Users.RemoveAt(i);
                    }
                }
            });
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                WebRequest req = (WebRequest) asynchronousResult.AsyncState;
                WebResponse resp = req.EndGetResponse(asynchronousResult);
                HttpWebResponse response = (HttpWebResponse) resp;
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();
                // Close the stream object
                streamResponse.Close();
                streamRead.Close();
                // Release the HttpWebResponse
                response.Close();

                //Do whatever you want with the returned "responseString"
                CncGeneralInfo info = JsonConvert.DeserializeObject<CncGeneralInfo>(responseString);
                
               
                

            }
            catch (Exception err)
            {
                Console.WriteLine($"Sux: {err}");
            }
        }
        internal static CncGeneralInfo ReadToObject(string json)
        {
            CncGeneralInfo deserializedUser = new CncGeneralInfo();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as CncGeneralInfo;
            ms.Close();
            return deserializedUser;
        }
    }

    public class Player
    {
        public string Name { get; set; }
    }

    [DataContract]
    internal class CncGeneralInfo
    {
        [DataMember] internal Game bfme;
        [DataMember] internal Game bfme2;
        [DataMember] internal Game cnc3;
        [DataMember] internal Game cnc3kw;
        [DataMember] internal Game generals;
        [DataMember] internal Game generalszh;
        [DataMember] internal Game ra3;
        [DataMember] internal Game rotwk;

        public override string ToString()
        {
            return "Game: " + ra3.users.Keys.Count;
        }
    }

    [DataContract]
    internal class Game
    {
        [DataMember] internal Lobby lobbies;
        [DataMember] internal IDictionary<string, User> users;
        [DataMember] internal MetaMatch games;
    }
    [DataContract]
    internal class Lobby
    {
        [DataMember] internal int chat;
        [DataMember] internal int hosting;
        [DataMember] internal int playing;
    }

    [DataContract]
    internal class User
    {
        [DataMember] internal int id;
        [DataMember] internal string nickname;
        [DataMember] internal int pid;

        public string Nickname => nickname;
        
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

            if (!other.id.Equals(id))
            {
                return false;
            }

            if (!other.pid.Equals(pid))
            {
                return false;
            }

            if (!other.nickname.Equals(nickname))
            {
                return false;
            }

            return true;
        }
    }

    [DataContract]
    internal class MetaMatch
    {
        [DataMember] internal Match[] playing;
        [DataMember] internal Match[] staging;
    }

    [DataContract]
    internal class Match
    {
        [DataMember] internal string cmdCRC;
        [DataMember] internal string exeCRC;
        [DataMember] internal string gamever;
        [DataMember] internal User host;
        [DataMember] internal string iniCRC;
        [DataMember] internal string map;
        [DataMember] internal string maxRealPlayers;
        [DataMember] internal string maxplayers;
        [DataMember] internal string numObservers;
        [DataMember] internal string numRealPlayers;
        [DataMember] internal string numplayers;
        [DataMember] internal string obs;
        [DataMember] internal string pings;
        [DataMember] internal string pw;
        [DataMember] internal string title;
        [DataMember] internal string version;
        [DataMember] internal User[] players;
    }
}
