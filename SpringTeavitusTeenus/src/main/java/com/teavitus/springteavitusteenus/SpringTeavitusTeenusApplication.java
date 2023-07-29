package com.teavitus.springteavitusteenus;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.annotation.EnableCaching;

@SpringBootApplication
@EnableCaching
public class SpringTeavitusTeenusApplication {

	public static void main(String[] args) {
		SpringApplication.run(SpringTeavitusTeenusApplication.class, args);
	}

}
