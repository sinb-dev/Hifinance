# Gradual sample setups using several technologies like kafka, prometheus and grafana.
It is advised to run the experiments in the following order
1. LinuxSystemMetrics2Prometheus-simple: Shows how to extract metrics from a linux environment using node_exporter.
2. Prometheus2grafana-simple: Setting up a Prometheus DB with a Grafana frontend, using only Prometheus own metrics as case
3. Kafka2Prometheus-simple: Showing how to scrape Kafka data in Prometheus by using a Kafka Exporter. This will measure the amount of messages produced in a given topic


Other folders
* dotnet-kafka-connecter is a folder for testing connections. It creates a producer that dispatches messages on a Kafka broker.