using System;
using System.Reflection;
using System.Runtime.Serialization;

[Serializable]
public class Message
{
    public string Content { get; set; }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Content", Content);
    }

    protected Message(SerializationInfo info, StreamingContext context)
    {
        Content = info.GetString("Content");
    }

    public Message()
    {
    }
}

sealed class PreMergeToMergedDeserializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type typeToDeserialize = null;
        String exeAssembly = Assembly.GetExecutingAssembly().FullName;
        typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, exeAssembly));

        return typeToDeserialize;
    }
}