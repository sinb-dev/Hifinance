# Simpel example how to setup Prometheus to scrape Kafka metrics
This setup requires docker. The docker compose file will start a kafka environment with a single zookeeper and kafka environmen by using confluents images. In addition a Prometheus will be started and to "glue" the Kafka environment together with Prometheus an Kafka exporter will be started as well. The kafka exporter will expose metrics related to kafkas performance eg. the offset (amount of messages sent) in a given topic and stuff like that. Kafka exporter cannot show any metrics on the specific data produced in a topic.

The kafka exporter from bitname is documented here
https://hub.docker.com/r/bitnami/kafka-exporter
and here 
https://github.com/danielqsj/kafka_exporter#flags

## Connecting Prometheus to the Kafka exporter
In order to scrape from the kafka exporter a job must be created within the Prometheus config. There is no way to setting this job up through Prometheus web frontend. There is a prometheus configuration file inside the prometheus folder relative to this readme.

# Run it twice
You're required to run this docker compose file twice. The exporter does not run the first time because there is no broker to connect to. Run docker compose again to simple start it when the broker is ready.