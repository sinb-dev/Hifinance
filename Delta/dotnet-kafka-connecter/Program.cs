Thread consumerThread = new Thread( () => {
    Consumer c = new Consumer();
    int runtimeMsec = 10000; //run for ten seconds
    DateTime start = DateTime.Now;
    while ( (DateTime.Now - start).Milliseconds < runtimeMsec ) {
        c.Start();
        Thread.Sleep(20);
    }
    Console.WriteLine("Consumer thread closed");
});
consumerThread.Start();
Thread.Sleep(100);
Thread producerThread = new Thread( () => {
    Producer p = new Producer();
    p.Start();
    Console.WriteLine("Producer thread done");
});
producerThread.Start();

