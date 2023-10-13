# This library connects to Kafka and is used by the dkc.producer and dkc.consumer projects
The combined dkc projects were made to create some kafka trafic visible inside Prometheus. Prometheus scrapes Kafka data exposed by Kafka Exporter.

This project defines a Producer class that can produce a list of messages either instantly or delayed. Given a list of delayed messages (containing a delay and message property) this library can simulate message created over a period of time.