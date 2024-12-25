Code for Gateway Device. The gateway device is responsible for communication between node devices and swuich.
For now three node devices are connected to it; Automation, Temprature node and Dimming Node.
For Relay and Temprature MQTT has been used as communication protocol, whereas, for dimming BLE has been used.
The gateway device recieves messages from the server with a flag, the flag determines to which device the data will be sent. Also it recieves the messages from 
temperature node and publish back it to the server.
There are 4 main topics in the gateway;
GatewayNode: Publishing data to node device
GatewayServer: Publishing data to server device
ServerGateway: Listening to data sent by server
NodeGateway: Listening to data sent by node

Future Changes:
The edge computing concepts allows the systems to work in more systematic way. For this the node device must be more powerful. We are using kafka with mqtt due to
device constraints here. In future, if the gateway device is changed, the mqtt part can be dominated as every single device on the nodes will share data without WiFi 
network and the kafka will be used for server communication.