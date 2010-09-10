using System.ServiceModel.Description;

namespace WcfRestContrib.ServiceModel.Description
{
    public class RequestMessagePartDescription : MessagePartDescription
    {
        // ────────────────────────── Enumerations ──────────────────────────

        public enum MessagePartType
        {
            EntityBody,
            PathSegment,
            Querystring
        }

        // ────────────────────────── Constructors ──────────────────────────

        public RequestMessagePartDescription(MessagePartDescription messagePart, MessagePartType partType, string alias) : 
            base(messagePart.Name, messagePart.Namespace)
        {
            Index = messagePart.Index;
            MemberInfo = messagePart.MemberInfo;
            Multiple = messagePart.Multiple;
            ProtectionLevel = messagePart.ProtectionLevel;
            Type = messagePart.Type;
            Alias = alias;
            PartType = partType;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public MessagePartType PartType { get; set; }
        public string Alias { get; set; }
    }
}
