using Grpc.Core;
using H.Framework.Core.Mapping;
using H.Framework.Core.Utilities;
using H.Framework.UMeng.Push;
using H.Framework.UMeng.Push.Bases;
using H.Framework.WPF.Control.Controls.ExtendedWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Zeus.RPC.Protocol;

namespace H.Framework.WPF.UITest
{
    /// <summary>
    /// GWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GWindow : ExtendedWindow
    {
        public GWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nonce = Utility.ObjectID;
            var curTime = DateTime.Now.ToLong().ToString();
            txt1.Text = nonce;
            txt2.Text = curTime;
            txt3.Text = HashEncryptHepler.SHA1Hash(txt0.Text + nonce + curTime).ToLower();
            var a = HashEncryptHepler.MD5Hash(HashEncryptHepler.MD5Hash("15604293775" + "I4kl$0bs"));
            //var b = Encoding.Default.GetString(Convert.FromBase64String("eVRkd3JodWpNWjJmemhNWmVkaVJKWjlBNk45RFZReWw="));
            //var a = HashEncryptHepler.EncryptAESToBase64("Helloworld", b, b.Substring(0, 16));
            //txt2.Text = HashEncryptHepler.DecryptAESToString(txt1.Text, b, b.Substring(0, 16));
            //var 啊1 = HashEncryptHepler.DecryptAESToString(啊, "qiK5jiZ7$rgBWVz1V*jJ!@ly7d2vxT9j", "AqIm%czX6M20mi8w");
            //Trace.WriteLine(int.Parse("001231").ToString());
            //var a = Regex.Matches("asdasd1/9/2019 3:9:9 PM", RegexExtensions.DateTimePattern());
            //var a = MsgServiceClient.Send();
            //var b = MsgServiceClient.UpdateUser();
            //var c = MsgServiceClient.AddUserLog();
            //var d = MsgServiceClient.UpdateUser();
            //var a = MsgServiceClient.Code();
            //PushAndroidMsg(PushType.CustomizedCast, true, null, "", "测试测试", "测试内容", "", "12606278");
            //PushIosMsg(PushType.CustomizedCast,false,null,"","测试测试","测试内容","","d9e81235a11e4328a6d73ac104ff57d6");
            //PushMessage(PushType.BroadCast, "", "", "测试", "测试umeng广播", "测试umeng", "1");
            //var a = HAHA();
            //var b = HA();
        }

        /// <summary>
        /// ios android push msg
        /// </summary>
        /// <param name="pushType">消息类型</param>
        /// <param name="tag">用户标签 umeng设置</param>
        /// <param name="starttime">发送时间</param>
        /// /// <param name="title">通知标题</param>
        /// <param name="content">通知内容</param>
        /// <param name="description">通知文字描述</param>
        /// <param name="thirdparty_id">开发者自定义消息标识ID</param>
        public static void PushMessage(PushType pushType, string tag, string starttime, string title, string content, string description, string tels = null)
        {
            //"{""where"":{""and"":[{""or"":[{""tag"":""1""}]}]}}";
            JObject json = null;
            if (!string.IsNullOrWhiteSpace(tag))
            {
                string jsonStr = @"{""where"":{""and"":[{""or"":[{""tag"":""" + tag + @"""}]}]}}";
                json = JObject.Parse(jsonStr);
            }
            PushIosMsg(pushType, true, json, starttime, title, content, description, "");
            PushAndroidMsg(pushType, true, json, starttime, title, content, description, "");
            //if (!string.IsNullOrWhiteSpace(starttime))
            //{
            //    CronTask.SetTask(tag, starttime, title, content, description, tels);
            //}
        }

        /// <summary>
        /// ios push
        /// </summary>
        /// <param name="isProduc">推送模式 groupcast broadcast</param>
        public static void PushIosMsg(PushType pushType, bool isProduc, dynamic tagjsonobj, string starttime, string title, string content, string description, string alias)
        {
            var push = new UMengMessagePush("5d1c3c604ca3575ac2000a82", "idvcltvfmsd3qpvjnexdchrplgrvjgsc");//可以配置到web.config中
            var postJson = new PostUMengJson<IOSPayload>
            {
                PushType = pushType,
                ProductionMode = isProduc
            };
            postJson.Payload.Aps.Alert.Body = content;
            postJson.Payload.Aps.Alert.Title = title;
            postJson.Payload.Aps.Sound = "default";
            postJson.Alias = alias;
            postJson.Alias_Type = "LY_ZEUS";
            if (tagjsonobj != null)
            {
                postJson.Filter = tagjsonobj;//不能传字符串
            }
            if (!string.IsNullOrWhiteSpace(starttime))
            {
                postJson.Policy.Start_Time = starttime;
            }

            //postJson.Payload.Extra.Add("ActivityId", thirdparty_id);
            postJson.Description = description;
            //postJson.Thirdparty_Id = thirdparty_id;

            //ReturnJsonClass resu = push.SendMessage(postJson);
            var result = push.SendMessage(postJson);
        }

        public static void PushAndroidMsg(PushType pushType, bool isProduc, dynamic tagjsonobj, string starttime, string title, string content, string description, string alias)
        {
            var push = new UMengMessagePush("5d11d03e0cafb212c6000171", "fjyzd0evg971fyviuczkgzpoqjawnxyl");//可以配置到web.config中
            var postJson = new PostUMengJsonAndroid<AndroidPayload>
            {
                PushType = pushType,
                ProductionMode = isProduc
            };

            if (tagjsonobj != null)
            {
                postJson.Filter = tagjsonobj;//不能传字符串
            }
            if (!string.IsNullOrWhiteSpace(starttime))
            {
                postJson.Policy.Start_Time = starttime;
            }
            postJson.Alias = alias;
            postJson.Alias_Type = "LY_ZEUS";
            //postJson.Payload.Extra.Add("ActivityId", thirdparty_id);
            postJson.Payload.Display_Type = "notification";
            postJson.Payload.Body.Ticker = "系统消息";
            postJson.Payload.Body.Title = title; //"您的评论有回复了";
            postJson.Payload.Body.Text = content;// "我是内容";
            postJson.Payload.Body.After_Open = "go_app";
            postJson.Payload.Body.Custom = "comment-notify";
            postJson.Payload.Body.Sound = "";//如果该字段为空，采用SDK默认的声音

            postJson.Description = description;
            //postJson.Thirdparty_Id = thirdparty_id;

            //ReturnJsonClass resu = push.SendMessage(postJson);
            //push.AsynSendMessage(postJson, callBack);
            //ar result = push.SendMessage(postJson);
            push.AsynSendMessage(postJson, CallBack);
        }

        private static void CallBack(ReturnJsonClass result)
        {
        }            //private AA HA()

        //{
        //    return new AA { aa = "aaa" };
        //}

        //private AA HAHA()
        //{
        //    return new BB { aa = "aaa", bb = "bbb" };
        //}

        //private class AA
        //{
        //    public string aa { get; set; }
        //}

        //private class BB : AA
        //{
        //    public string bb { get; set; }
        //}
    }

    public static class MsgServiceClient
    {
        private static Channel _channel;
        private static UserRpcService.UserRpcServiceClient _client;
        private static NotificationRpcService.NotificationRpcServiceClient _client2;
        private static UserLogRpcService.UserLogRpcServiceClient _client3;
        private static FuturesCompanyRpcService.FuturesCompanyRpcServiceClient _client4;
        private static CommonRpcService.CommonRpcServiceClient _client5;

        static MsgServiceClient()
        {
            _channel = new Channel("127.0.0.1:40001", ChannelCredentials.Insecure);
            //_channel = new Channel("192.168.50.30:40001", ChannelCredentials.Insecure);
            //_channel = new Channel("192.168.99.109:40001", ChannelCredentials.Insecure);
            //_channel = new Channel("39.102.44.67:40001", ChannelCredentials.Insecure);
            _client = new UserRpcService.UserRpcServiceClient(_channel);
            _client2 = new NotificationRpcService.NotificationRpcServiceClient(_channel);
            _client3 = new UserLogRpcService.UserLogRpcServiceClient(_channel);
            _client4 = new FuturesCompanyRpcService.FuturesCompanyRpcServiceClient(_channel);
            _client5 = new CommonRpcService.CommonRpcServiceClient(_channel);
        }

        public static UsersResp GetUsers()
        {
            var req = new UsersReq();
            req.UserIDs.Add(18);
            return _client.GetUsers(req);
        }

        public static UserResp GetUser()
        {
            var req = new UserReq();
            req.Nickname = "用户692778";
            return _client.GetUser(req);
        }

        public static UserResp UpdateUser()
        {
            var req = new UserLevelReq();
            req.Username = "13321952950";
            req.UserLevel = UserLevel.Extreme;
            return _client.UpdateUserLevel(req);
        }

        public static NotificationResp Send()
        {
            //return _client2.SendAll(new NotificationAllReq { Type = "ZEUS", IsStock = true, IsNotify = true, Title = "RPC测试", Content = "RPC测试", Creator = "Rpc", IsInnerBroadcast = false, NotificationTypeId = 4 });
            var req = new NotificationByUserLevelReq { Req = new NotificationReq { Type = "ZEUS", IsStock = true, IsNotify = true, Title = "RPC测试", Content = "RPC测试", Creator = "Rpc", NotificationTypeId = 4 } };
            req.UserLevels.Add(10);
            req.UserLevels.Add(5);
            return _client2.SendByUserLevel(req);
        }

        public static UserLogResp AddUserLog()
        {
            return _client3.AddUserLog(new UserLogReq { Username = "13321952950", Action = TradeAction.AccountBalance, Platform = Platform.Pc });
        }

        public static FuturesCompanysResp Get()
        {
            return _client4.Get(new FuturesCompanysReq { Platform = "PC" });
        }

        public static VerificationCodeResp GetCode()
        {
            return _client5.GetVerificationCode(new VerificationCodeReq { Phone = "13321952950" });
        }

        public static VerificationCodeResp Code()
        {
            return _client5.VerifyCode(new VerificationCodeReq { Phone = "13321952950", VerificationCode = "598960" });
        }
    }

    //public class User
    //{
    //    public int Level { get; set; }
    //}
}