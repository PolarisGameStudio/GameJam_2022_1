using Newtonsoft.Json;
using UnityEngine;

public class RequestCreativePacket : IRequestPacket
{
    public int round { get; set; }
    public string package { get; set; }
    public string device_id { get; set; }
    public int score { get; set; }


    public RequestCreativePacket(int score) : base("/api/vote.php")
    {
    }
}

public class ResponseCreativePacket : ResponsePacket
{
}