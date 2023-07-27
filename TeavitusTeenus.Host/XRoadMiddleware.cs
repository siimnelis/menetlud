using System.Text;

namespace TeavitusTeenus.Host;

public class XRoadMiddleware
{
	public XRoadMiddleware(RequestDelegate _)
	{
		
	}

    public async Task InvokeAsync(HttpContext httpContext, ILogger<XRoadMiddleware> logger)
    {
        if (httpContext.Request.Method == HttpMethod.Get.Method)
        {
            httpContext.Response.StatusCode = 204;
            return;
        }
        using var sr = new StreamReader(httpContext.Request.Body);

        var request = await sr.ReadToEndAsync();
        
        logger.LogInformation("Received message: " + request);
        
        httpContext.Response.StatusCode = 200;
        httpContext.Response.Headers.ContentType = "text/xml; charset=utf-8";

        var xml =
	        @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
	                <soapenv:Header xmlns:xrd=""http://x-road.eu/xsd/xroad.xsd"" xmlns:id=""http://x-road.eu/xsd/identifiers"">
		                <h:service a:objectType=""MEMBER"" xmlns:h=""http://x-road.eu/xsd/xroad.xsd"" xmlns:a=""http://x-road.eu/xsd/identifiers"" xmlns=""http://x-road.eu/xsd/xroad.xsd"">
			                <a:serviceCode>TeavitusTeenus</a:serviceCode>
		                </h:service>
	                </soapenv:Header>
	                <soapenv:Body>
		                <TeavitaResponse xmlns=""http://teavitus.x-road.ee""/>
	                </soapenv:Body>
				</soapenv:Envelope>";


        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        httpContext.Response.ContentLength = ms.Length;
        await ms.CopyToAsync(httpContext.Response.Body);
    }
}