using System;
using System.ServiceModel.Channels;
using System.Xml;
using WcfRestContrib.ServiceModel.Channels;
using System.Runtime.Serialization;

namespace WcfRestContrib.ServiceModel.Dispatcher
{
    /// <summary>
    /// Interface for request/response serializers
    /// </summary>
    public interface IWebFormatter
    {
        object Deserialize(WebFormatterDeserializationContext context, Type type);
        WebFormatterSerializationContext Serialize(object data, Type type);
    }
}
