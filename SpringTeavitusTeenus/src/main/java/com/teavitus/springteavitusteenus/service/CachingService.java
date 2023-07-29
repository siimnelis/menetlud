package com.teavitus.springteavitusteenus.service;

import ee.x_road.teavitus.Teavitus;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.CacheManager;
import org.springframework.stereotype.Component;

@Component
public class CachingService {

    private final String cacheName = "messages";

    @Autowired
    CacheManager cacheManager;

    public void putToCache(String key, Teavitus value) {
        cacheManager.getCache(cacheName).put(key, value);
    }

    public Teavitus getFromCache(String key) {
        Teavitus value = null;
        if (cacheManager.getCache(cacheName).get(key) != null) {
            value = (Teavitus) cacheManager.getCache(cacheName).get(key).get();
        }
        return value;
    }

}
