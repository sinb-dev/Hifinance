This example shows how to extract linux metrics using node_exporter. A node_exporter container is booted up pointing to the host systems proc and sys folders using docker mounts. The node exporter is ready to be scraped on port 9100.

Prometheus is configured to scrape node_exporter:9100 address and metrics prefixed with node_ are ready to be explored.