using H.Framework.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.UMeng.Push.Bases
{
    /// <summary>
    /// 发送到友盟的json实体类
    /// </summary>
    public class PostUMengJson<T> where T : class, new()
    {
        public PostUMengJson()
        {
            Policy = new Policy();
            Payload = new T();
        }

        /// <summary>
        /// 必填 应用唯一标识
        /// </summary>
        public string Appkey { get; set; }

        /// <summary>
        /// 注意：该值由UMengMessagePush自动生成，无需主动赋值
        ///
        /// 必填 时间戳，10位或者13位均可，时间戳有效期为10分钟
        /// </summary>
        public string Timestamp => TimeHelper.UtcSeconds(DateTime.Now).ToString();

        /// <summary>
        /// 必填 消息发送类型,其值可以为:
        /// <example>
        ///unicast-单播
        ///listcast-列播(要求不超过500个device_token)
        ///filecast-文件播
        ///(多个device_token可通过文件形式批量发送）
        ///broadcast-广播
        ///groupcast-组播
        ///(按照filter条件筛选特定用户群, 具体请参照filter参数)
        ///customizedcast(通过开发者自有的alias进行推送),
        ///包括以下两种case:
        ///- alias: 对单个或者多个alias进行推送
        ///- file_id: 将alias存放到文件后，根据file_id来推送
        ///</example>
        /// </summary>
        public string Type { get; private set; }

        [JsonIgnore]
        public PushType PushType
        {
            set
            {
                Type = value.ToString().ToLower();
            }
        }

        /// <summary>
        /// 当type=unicast时, 必填, 表示指定的单个设备
        /// 当type=listcast时, 必填, 要求不超过500个, 以英文逗号分隔
        /// </summary>
        public string Device_Tokens { get; set; }

        /// <summary>
        /// 可选
        /// 当type=customizedcast时必填，alias的类型,
        /// alias_type可由开发者自定义,
        /// 开发者在SDK中调用setAlias(alias, alias_type)时所设置的alias_type
        /// </summary>
        public string Alias_Type { get; set; }

        /// <summary>
        /// 可选 当type=customizedcast时,
        /// 开发者填写自己的alias。 要求不超过50个alias,多个alias以英文逗号间隔。
        /// 在SDK中调用setAlias(alias, alias_type)时所设置的alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 当type=filecast时，必填，file内容为多条device_token，以回车符分割
        /// 当type=customizedcast时，选填(此参数和alias二选一)
        /// file内容为多条alias，以回车符分隔。注意同一个文件内的alias所对应
        /// 的alias_type必须和接口参数alias_type一致。
        /// 使用文件播需要先调用文件上传接口获取file_id，参照"文件上传"
        /// </summary>
        public string File_Id { get; set; }

        /// <summary>
        /// 当type=groupcast时，必填，用户筛选条件，如用户标签、渠道等，参考附录G。
        /// </summary>
        public dynamic Filter { get; set; }

        /// <summary>
        /// 必填 消息内容(Android最大为1840B,IOS最大为2012B),
        /// </summary>
        public T Payload { get; set; }

        /// <summary>
        /// 可选 发送策略
        /// </summary>
        public Policy Policy { get; set; }

        /// <summary>
        /// 可选，正式/测试模式。默认为true
        /// 测试模式只对“广播”、“组播”类消息生效，其他类型的消息任务（如“文件播”）不会走测试模式
        /// 测试模式只会将消息发给测试设备。测试设备需要到web上添加。
        /// </summary>
        public string Production_Mode { get; private set; }

        public bool ProductionMode
        {
            set
            {
                Production_Mode = value.ToString().ToLower();
            }
        }

        /// <summary>
        /// 可选 发送消息描述，建议填写。
        /// </summary>
        public string Description { get; set; }

        //public string Thirdparty_Id { get; set; }
    }

    public class IOSPayload
    {
        public IOSPayload()
        {
            Aps = new Aps();
            Extra = new SerializableDictionary<string, string>();
        }

        /// <summary>
        /// 必填，严格按照APNs定义来填写
        /// </summary>
        public Aps Aps { get; set; }

        /// <summary>
        /// ios 自定义key-value
        /// 可选 用户自定义key-value。只对"通知"(display_type=notification)生效。
        /// 可以配合通知到达后,打开App,打开URL,打开Activity使用。
        /// </summary>
        public SerializableDictionary<string, string> Extra { get; set; }
    }

    public class AndroidPayload
    {
        public AndroidPayload()
        {
            Body = new ContentBody();
            Extra = new SerializableDictionary<string, string>();
        }

        /// <summary>
        /// 必填 消息类型，值可以为:notification-通知，message-消息
        /// </summary>
        public string Display_Type { get; set; }

        /// <summary>
        /// 必填 消息体
        /// </summary>
        public ContentBody Body { get; set; }

        /// <summary>
        /// andro自定义key-value
        /// 可选 用户自定义key-value。只对"通知"
        /// (display_type=notification)生效。
        /// 可以配合通知到达后,打开App,打开URL,打开Activity使用。
        /// </summary>
        public SerializableDictionary<string, string> Extra { get; set; }
    }

    public class Aps
    {
        public Aps()
        {
            Alert = new AlertScheme();
        }

        /// <summary>
        /// 当content-available=1时(静默推送)，可选; 否则必填。
        /// 可为JSON类型和字符串类型
        /// JSON类型和字符串类型
        /// Json格式:{"title":"title","subtitle":"subtitle","body":"body"}
        /// </summary>
        public AlertScheme Alert { get; set; }

        /// <summary>
        /// 可选
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// 可选，注意: ios8才支持该字段。
        /// </summary>
        public string Category { get; set; }
    }

    public class AlertScheme
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Body { get; set; }
    }

    public class ContentBody
    {
        /// <summary>
        /// 必填 通知栏提示文字
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// 必填 通知标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 必填 通知文字描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 自定义通知图标:
        /// 可选，状态栏图标ID，R.drawable.[smallIcon]，
        /// 如果没有，默认使用应用图标。
        /// 图片要求为24*24dp的图标，或24*24px放在drawable-mdpi下。
        /// 注意四周各留1个dp的空白像素
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 可选，通知栏拉开后左侧图标ID，R.drawable.[largeIcon]，
        /// 图片要求为64*64dp的图标，
        /// 可设计一张64*64px放在drawable-mdpi下，
        /// 注意图片四周留空，不至于显示太拥挤
        /// </summary>
        public string LargeIcon { get; set; }

        /// <summary>
        /// 可选，通知栏大图标的URL链接。该字段的优先级大于largeIcon。
        /// 该字段要求以http或者https开头。
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 自定义通知声音:
        /// 可选，通知声音，R.raw.[sound]。
        /// 如果该字段为空，采用SDK默认的声音，即res/raw/下的
        /// umeng_push_notification_default_sound声音文件。如果
        /// SDK默认声音文件不存在，则使用系统默认Notification提示音。
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// 自定义通知样式:
        /// 可选，默认为0，用于标识该通知采用的样式。使用该参数时，
        /// 开发者必须在SDK里面实现自定义通知栏样式。
        /// </summary>
        public int Builder_Id { get; set; }

        // 通知到达设备后的提醒方式，注意，"true/false"为字符串
        /// <summary>
        /// 可选，收到通知是否震动，默认为"true"
        /// </summary>
        public string Play_Vibrate { get; set; }

        /// <summary>
        /// 可选，收到通知是否闪灯，默认为"true"
        /// </summary>
        public string Play_Lights { get; set; }

        /// <summary>
        /// 可选，收到通知是否发出声音，默认为"true"
        /// </summary>
        public string Play_Sound { get; set; }

        /// <summary>
        /// 必填 点击"通知"的后续行为，默认为打开app。
        /// 取值：
        /// "go_app": 打开应用
        /// "go_url": 跳转到URL
        /// "go_activity": 打开特定的activity
        /// "go_custom": 用户自定义内容。
        /// </summary>
        public string After_Open { get; set; }

        /// <summary>
        /// 当after_open=go_url时，必填。
        /// 通知栏点击后跳转的URL，要求以http或者https开头
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 当after_open=go_activity时，必填。
        /// 通知栏点击后打开的Activity
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// 当display_type=message时, 必填
        /// 当display_type=notification且
        /// after_open=go_custom时，必填
        /// 用户自定义内容，可以为字符串或者JSON格式。
        /// </summary>
        public string Custom { get; set; }
    }

    /// <summary>
    /// 可选，发送策略
    /// </summary>
    public class Policy
    {
        public Policy()
        {
            Max_Send_Num = 1000;
        }

        /// <summary>
        /// 可选，定时发送时，若不填写表示立即发送。
        /// 定时发送时间不能小于当前时间
        /// 格式: "yyyy-MM-dd HH:mm:ss"。
        /// 注意，start_time只对任务类消息生效。
        /// </summary>
        public string Start_Time { get; set; }

        /// <summary>
        /// 可选，消息过期时间，其值不可小于发送时间或者
        /// start_time(如果填写了的话)，
        /// 如果不填写此参数，默认为3天后过期。格式同start_time
        /// </summary>
        public string Expire_Time { get; set; }

        /// <summary>
        /// 可选，发送限速，每秒发送的最大条数。最小值1000
        /// 开发者发送的消息如果有请求自己服务器的资源，可以考虑此参数。
        /// </summary>
        public int Max_Send_Num { get; set; }

        /// <summary>
        /// 可选，开发者对消息的唯一标识，服务器会根据这个标识避免重复发送。
        /// 有些情况下（例如网络异常）开发者可能会重复调用API导致
        /// 消息多次下发到客户端。如果需要处理这种情况，可以考虑此参数。
        /// 注意, out_biz_no只对任务类消息生效。
        /// </summary>
        public string Out_Biz_No { get; set; }
    }
}