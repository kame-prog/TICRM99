Node device code to update the new firmware. It uses MQTT for communication with Gateway. There are two usecases here; Firmware Update for all devices and Location based update.
The message recieved contains a flag which defines each usecase. For Firmware Update HTTPUPDATE library has been used.
In future, the code need to be modified in order to download from a remote HTTPS server. Rightnow, the device download the file 
kept on local host of a machine in the same network.