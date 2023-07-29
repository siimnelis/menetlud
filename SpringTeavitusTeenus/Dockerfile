FROM maven:3.8.4-openjdk-17 AS BUILD
COPY src ./src
COPY pom.xml .
RUN mvn clean package

FROM openjdk:17-jdk-alpine
COPY --from=build ./target/spring-teavitus-teenus-0.0.1.jar app.jar
EXPOSE 5273
ENTRYPOINT ["java","-jar","/app.jar"]