# Gradual sample setups using several technologies like kafka, prometheus and grafana.
It is advised to run the experiments in the following order
1. Prometheus2grafana-simple: Setting up a Prometheus DB with a Grafana frontend, using only Prometheus own metrics as case
2. Kafka2Prometheus-simple: Showing how to scrape Kafka data in Prometheus by using a Kafka Exporter. This will measure the amount of messages produced in a given topic