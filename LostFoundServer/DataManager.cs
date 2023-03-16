using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostFoundServer;
class DataStructure
{
    public int Command;
    //可以是 0，1，2，3，4 0:读取 1:写入 2:表示Payload是一个Base64编码的文件，需要被保存 3:删除名称为Name的数据 4:删除文件名为Name的文件
    public string Name;
    //数据的名称
    public string Payload;
    //负载数据
}

class DataManager
{
    public Dictionary<string, string> Data = new Dictionary<string, string>(); //成对保存数据

    public bool Exists(string Key) => Data.ContainsKey(Key);

    public string Get(string Key) => JsonConvert.SerializeObject(new DataStructure() { Command = 0, Name = Key, Payload = Data[Key] });

    public bool Set(string Key, string value)
    {
        if (Exists(Key))
        {
            Data[Key] = value;
            return true;  //true代表此次进行的是更新操作
        }
        else
        {
            Data.Add(Key, value);
            return false; //false代表此次进行的是添加操作
        }
    }

    public void Delete(string Key) => Data.Remove(Key);

    public string Serialize
    {
        get
        {
            return JsonConvert.SerializeObject(Data); //将Dictionary序列化为Json以便保存
        }
    }

    public void Save()
    {
        File.WriteAllText("./Data.json", Serialize); //将Json保存到硬盘                                 
    }

    public void Load()
    {
        if (!File.Exists("./Data.json"))   //如果数据文件不存在，则直接返回
            return;

        string Dt = File.ReadAllText("./Data.json");
        Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(Dt); //读取数据后反序列化
    }

    public void Print() //打印数据
    {
        foreach (var i in Data)
        {
            Console.WriteLine(i);
        }
    }
}

