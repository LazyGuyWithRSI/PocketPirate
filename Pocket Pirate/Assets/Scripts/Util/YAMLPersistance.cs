using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public static class YAMLPersistance
{
    public static void ReadYaml(string path)
    {
        var input = new StreamReader(path);
        var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        var thing = deserializer.Deserialize<PatrolAction>(input);
        Debug.Log("Thing type: " + thing.DelayLower);
    }
}
