using System.ServiceModel.Channels;
using System.Xml;

namespace Xtee.Teavitus.Connector.XRoad;

public class XRoadMessage : Message
{
    private Message Message { get; }

    public XRoadMessage(Message message)
    {
        Message = message;
    }
    
    public override MessageHeaders Headers => Message.Headers;
    public override MessageProperties Properties => Message.Properties;
    public override MessageVersion Version => Message.Version;
    
    protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
    {
        Message.WriteBodyContents(writer);
    }

    protected override void OnWriteStartBody(XmlDictionaryWriter writer)
    {
        writer.WriteStartElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");
    }

    protected override void OnWriteStartHeaders(XmlDictionaryWriter writer)
    {
        writer.WriteStartElement("Header", "http://schemas.xmlsoap.org/soap/envelope/");
    }

    protected override void OnWriteStartEnvelope(XmlDictionaryWriter writer)
    {
        writer.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
        writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");            
    }
    
}