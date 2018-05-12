Using Python 3

 - pySerial: serial access for python
 - pyodbc: SQL server interfacing for python
 - sockets (default): for TCP communication

asynchronous shenanigans for python:

 - threading
 - asyncore
 - asyncserver
 ...
 ..
 .


Thinking 1-3 second interval for sending between client and server. Data structure-wise on a high level maybe the server stores a table-row and updates the appropriate one. Also we probably need to replicate the CAN library's layouts for each possible CAN packet in some fashion, so that once the packet is identified in the serial data stream, we know what the data actually means.

 - superclass: CAN packet
 - subclasses for each BMS/DC/MC packet
 - decode function that parses the data depending on specification for that packet.
 

So we will definitely need to look at all the packets that we currently receive and see which of them we actually want to see in telemetry. The rest we can just ignore.


See CAN-library layouts for 'documentation' on the driver controls packets.