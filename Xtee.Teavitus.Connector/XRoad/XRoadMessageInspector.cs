using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Xtee.Teavitus.Connector.XRoad;

public class XRoadMessageInspector : IClientMessageInspector
{
    public void AfterReceiveReply(ref Message reply, object correlationState)
    {
        
    }

    public object BeforeSendRequest(ref Message request, IClientChannel channel)
    {
        request = new XRoadMessage(request);
        return null!;
    }
}