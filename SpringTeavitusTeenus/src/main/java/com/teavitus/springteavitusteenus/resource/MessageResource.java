package com.teavitus.springteavitusteenus.resource;

import com.teavitus.springteavitusteenus.service.CachingService;
import ee.x_road.teavitus.ObjectFactory;
import ee.x_road.teavitus.Teavita;
import ee.x_road.teavitus.TeavitaResponse;
import ee.x_road.teavitus.Teavitus;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.ws.context.MessageContext;
import org.springframework.ws.server.endpoint.annotation.Endpoint;
import org.springframework.ws.server.endpoint.annotation.PayloadRoot;
import org.springframework.ws.server.endpoint.annotation.RequestPayload;
import org.springframework.ws.server.endpoint.annotation.ResponsePayload;
import org.springframework.ws.soap.SoapHeader;
import org.springframework.ws.soap.SoapHeaderElement;
import org.springframework.ws.soap.saaj.SaajSoapMessage;

import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import java.io.ByteArrayOutputStream;
import java.nio.charset.StandardCharsets;
import java.util.Iterator;

@Endpoint
public class MessageResource {

    private static final Logger LOGGER = LoggerFactory.getLogger(MessageResource.class);

    @Autowired
    private CachingService cachingService;

    @PayloadRoot(namespace = "http://teavitus.x-road.ee", localPart = "Teavita")
    @ResponsePayload
    public TeavitaResponse getTeavitaResponse(@RequestPayload Teavita request, MessageContext messageContext) throws TransformerException {
        if (request.getTeavitus() == null || request.getTeavitus().getTeavitusId() == null) {
            throw new RuntimeException("Something is wrong with the request");
        }

        addResponseHeaders(messageContext);

        Teavitus cachedTeavitus = cachingService.getFromCache(request.getTeavitus().getTeavitusId());
        if (cachedTeavitus != null) {
            LOGGER.info("Notification ID={} already received", cachedTeavitus.getTeavitusId());
            ObjectFactory factory = new ObjectFactory();
            return factory.createTeavitaResponse();
        }

        logRequest((SaajSoapMessage) messageContext.getRequest());

        cachingService.putToCache(request.getTeavitus().getTeavitusId(), request.getTeavitus());

        LOGGER.info("Sending OK response");
        ObjectFactory factory = new ObjectFactory();
        return factory.createTeavitaResponse();
    }

    private void addResponseHeaders(MessageContext messageContext) throws TransformerException {
        SoapHeader requestHeader = ((SaajSoapMessage) messageContext.getRequest()).getSoapHeader();
        SoapHeader responseHeader = ((SaajSoapMessage) messageContext.getResponse()).getSoapHeader();

        TransformerFactory transformerFactory = TransformerFactory.newInstance();
        Transformer transformer = transformerFactory.newTransformer();
        Iterator<SoapHeaderElement> iterator = requestHeader.examineAllHeaderElements();
        while (iterator.hasNext()) {
            SoapHeaderElement soapHeaderElement = iterator.next();
            transformer.transform(soapHeaderElement.getSource(), responseHeader.getResult());
        }
    }

    private void logRequest(SaajSoapMessage soapRequest) {
        try (ByteArrayOutputStream out = new ByteArrayOutputStream()) {
            soapRequest.writeTo(out);
            String requestString = out.toString(StandardCharsets.UTF_8);
            LOGGER.info("Received REQUEST message: \n {}", requestString);
        } catch (Exception e) {
            LOGGER.warn("Could not log WS message", e);
        }
    }

}
