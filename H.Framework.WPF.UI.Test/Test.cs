using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Framework.Core.Utilities;

namespace H.Framework.WPF.UI.Test
{
    public class Test
    {
        public byte[] HexStringToBytes(string hexString)
        {
            if (hexString == null || hexString.Equals(""))
            {
                return null;
            }
            hexString = hexString.ToUpper();
            int length = hexString.Length / 2;
            char[] hexChars = hexString.ToCharArray();
            byte[] d = new byte[length];
            for (int i = 0; i < length; i++)
            {
                int pos = i * 2;
                d[i] = (byte)(CharToByte(hexChars[pos]) << 4 | CharToByte(hexChars[pos + 1]));
            }
            return d;
        }

        private byte CharToByte(char c)
        {
            return (byte)"0123456789ABCDEF".IndexOf(c);
        }

        public void SaveMessage(byte[] message)
        {
            var allrevlist = new List<Dictionary<string, string>>();
            int maxlength = message.Length;
            int idx = 0;

            while ((message[idx] & 0xFF) == 171 && (message[idx + 1] & 0xFF) == 239)
            {
                var oneM = new Dictionary<string, string>();
                idx += 2;
                int start = idx;
                //后续包长度
                int packsize = message[idx] & 0xFF;
                oneM.Add("packsize", packsize.ToString());
                idx += 1;
                //包头帧控制字
                int packheader = message[idx] & 0xFF;
                oneM.Add("packheader", packheader.ToString());
                idx += 1;
                //信号强度
                int strength = message[idx];
                oneM.Add("strength", strength.ToString());
                idx += 1;
                //卡片地址
                int cardaddress = (message[idx] & 0xFF) + ((message[idx + 1] & 0xFF) * 0x100);
                idx += 2;
                oneM.Add("address", cardaddress.ToString());
                //序号包
                int sernumber = message[idx] & 0xFF;
                oneM.Add("sernumber", sernumber.ToString());
                idx += 1;
                //保留
                int hold = message[idx] & 0xFF;
                idx += 1;
                //beacon数量
                int beaconNumber = message[idx] & 0xFF;
                idx += 1;
                var beaconlist = new List<Dictionary<string, int>>();
                int beacon_start = 0;
                while (beacon_start < beaconNumber)
                {
                    //处理beacon
                    var beacon = new Dictionary<string, int>();
                    int major = ((message[idx] & 0xFF) * 256) + (message[idx + 1] & 0xFF);
                    idx += 2;
                    int minor = ((message[idx] & 0xFF) * 256) + (message[idx + 1] & 0xFF);
                    idx += 2;
                    int rssi = message[idx];
                    idx += 1;
                    beacon_start += 1;
                    beacon.Add("beacon_major", major);
                    beacon.Add("beacon_minor", minor);
                    beacon.Add("beaconstatus", rssi);
                    beaconlist.Add(beacon);
                }
                oneM.Add("beaconList", beaconlist.ToJson());
                //卡片状态
                //int status = message[idx]&0xFF;
                //oneM.AddAsync("cardStatus", ConvertByte.byteToBit(message[idx]));

                oneM.Add("cardStatus1", message[idx].ToString());
                idx += 1;

                //电池电量
                int voltage = message[idx] & 0xFF;
                idx += 1;
                oneM.Add("voltage", voltage.ToString());
                //oneM.AddAsync("voltage", ((voltage - 3.4) / 0.8).ToString());
                //后面是可选状态，读取前都先判断一下包长度
                while ((idx - start) < packsize)
                {
                    int attr = message[idx] & 0xFF;
                    idx += 1;
                    //if (attr == 1)
                    //{
                    //    //gps;
                    //    int x = ConvertByte.getInt(message, idx);
                    //    idx += 4;
                    //    int y = ConvertByte.getInt(message, idx);
                    //    idx += 4;
                    //    string x1 = string.Format("%08d", x);
                    //    string y1 = string.Format("%08d", y);

                    //    float longitude = float.Parse(x1.Substring(0, 2)) + float.Parse(x1.Substring(2, 4) + "." + x1.Substring(4)) / 60.0F;
                    //    float latitude = float.Parse(y1.Substring(0, 3)) + float.Parse(y1.Substring(3, 5) + "." + y1.Substring(5)) / 60.0F;
                    //    oneM.AddAsync("GPS" + idx, (new float[] { longitude, latitude }).ToJson());
                    //    continue;
                    //}
                    if (attr == 2)
                    {
                        //心率
                        int heart = message[idx] & 0xFF;
                        idx += 1;
                        oneM.Add("heart" + idx, heart.ToString());
                        continue;
                    }
                    if (attr == 3)
                    {
                        //计步
                        int step = message[idx] & 0xff + (message[idx + 1] & 0xff * 256);
                        idx += 2;
                        oneM.Add("step" + idx, step.ToString());
                        continue;
                    }
                    //if (attr == 4)
                    //{
                    //    //气压
                    //    float pressure = ConvertByte.getFloat(message, idx);
                    //    idx += 4;
                    //    oneM.AddAsync("pressure", pressure.ToString());
                    //    continue;
                    //}
                    if (attr == 5)
                    {
                        //温度
                        int temperature = message[idx] & 0xff + (message[idx + 1] & 0xff * 256);
                        idx += 2;
                        oneM.Add("temperature" + idx, temperature.ToString());
                        continue;
                    }
                    if (attr == 6)
                    {
                        //湿度
                        int humidity = message[idx] & 0xff + (message[idx + 1] & 0xff * 256);
                        idx += 2;
                        oneM.Add("humidity" + idx, humidity.ToString());
                        continue;
                    }
                }
                //读取一个校验位
                int verification = message[idx] & 0xFF;
                idx += 1;
                allrevlist.Add(oneM);
                if (idx >= maxlength) break;
            }
            var a = allrevlist.ToJson();
            //System.out.println(JsonUtils.listToJson(allrevlist));
        }
    }
}