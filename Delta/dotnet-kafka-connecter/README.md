# A simple test tool for connecting to kafka using confluents api

Create messages on a topic (default: testtopic). To produce messages run the dkc.producer project. By default it will create and send a list of text messages with a delay if 3000 miliseconds each. This is mostly for testing traffic on a kafka setup.

Resources
https://docs.confluent.io/kafka-clients/dotnet/current/overview.html