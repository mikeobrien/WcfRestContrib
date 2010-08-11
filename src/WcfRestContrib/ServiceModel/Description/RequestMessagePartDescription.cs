using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;

namespace WcfRestContrib.ServiceModel.Description
{
    public class RequestMessagePartDescription : System.ServiceModel.Description.MessagePartDescription
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
            this.Index = messagePart.Index;
            this.MemberInfo = messagePart.MemberInfo;
            this.Multiple = messagePart.Multiple;
            this.ProtectionLevel = messagePart.ProtectionLevel;
            this.Type = messagePart.Type;
            this.Alias = alias;
            this.PartType = partType;
        }

        // ────────────────────────── Public Members ──────────────────────────

        public MessagePartType PartType { get; set; }
        public string Alias { get; set; }
    }
}
