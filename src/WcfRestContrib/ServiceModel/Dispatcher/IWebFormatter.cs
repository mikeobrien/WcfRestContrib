using System;

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
