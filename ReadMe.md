# Proovitöö

## Rakenduse kävitamise juhend

Lähtekoodi *root* kasutas käivitada käsud:

1. docker compose build
2. docker compose up


## Sõnumeid tootev rakendus

Kasutaliides on loodus kasutades SwaggerUI-d, rakenuds avaneb peale dockeris kävitamist aadressil http://localhost:5271/swagger/index.html

## Sõnumeid vahendav rakendus 

Sõnumite edastamise õnnestumises/ebaõnnestumies saab veenduda vaadates menetlus-xtee.teavitus-connector-1 dockeri kontaineri logisi.

Õnnestunud saatmise logi näide:

Succesfully sent notification: {"Event": {"$type": "menetlusLoodud", "Markus": "string", "Kusimus": "string", "Staatus": 1, "Avaldaja": {"Eesnimi": "string", "Perenimi": "string", "Isikukood": "string"}, "MenetlusId": 1}, "Menetleja": null}

Ebaõnnestunud saatmise logi näide:

Calling TeavitusTeenus failed. Sent notification to dead letter queue.

## Sõnumeid tarbiv rakendus

Sõnumi saabumises saab veenduda vaadates menetlus-teavitus-1 dockeri kontaineri logi.

Kui sõnum saabub logitakse *Received REQUEST message: + SOAP envelope*