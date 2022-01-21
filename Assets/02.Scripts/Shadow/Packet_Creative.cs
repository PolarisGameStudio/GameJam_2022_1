using Newtonsoft.Json;
using UnityEngine;

public class RequestCreativePacket : IRequestPacket
{
    public int score { get; set; }
    public int round { get; set; }
    public string package { get; set; }


    public RequestCreativePacket(int score) : base("/api/vote.php")
    {
        round = 11;
        package = Application.identifier;
        this.score = score;
    }
}

public class ResponseCreativePacket : ResponsePacket
{
}