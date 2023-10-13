using dkc.lib;
Producer producer = new Producer( new HostConfig {
    Brokers = "kafka:9092",
    Topic = "testtopic"
});

var messages = new DelayedMessage[] {
    new DelayedMessage(3000, "A: This is the first line"),
    new DelayedMessage(3000, "B: Oh hi there"),
    new DelayedMessage(3000, "A: Well hello!"),
    new DelayedMessage(3000, "B: Welcome to kafka"),
    new DelayedMessage(3000, "A: Thank you very much!"),
    new DelayedMessage(3000, "B: I hope you will enjoy your stay"),
};

producer.StartDelayedMessages(messages);