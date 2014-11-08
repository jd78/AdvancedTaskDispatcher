AdvancedTaskDispatcher
======================

Multi-Thread task dispatcher. The messages are dispatched sequentially per message id.

The application will create a thread for each football match which will start consuming the actions sequentially. 
At the end of the matches, the threads are disposed.
