#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <SoftwareSerial.h>

#include <ESP8266HTTPClient.h>
#include <ESP8266httpUpdate.h>
//=======================================================//
#define SLEEP_DELAY_IN_SECONDS 60
#define WAIT_BEFORE_SLEEP_IN_SECONDS 5
#define REFERENCE_VOLTAGE_ADC 3.3

//======================================================//
// Software Serial
SoftwareSerial s(12, 14);


// initializes or defines the output pin of the LM35 temperature sensor
String newv = "";
String versions = "001";
const char* ssid = "TI_DEV";
const char* password = "TechDeve";

const char* mqtt_server = "192.168.22.79";
const char* mqtt_topicNG = "NodeGateway";   //Node to Gateway       (Subscribe)
const char* mqtt_topicSG = "ServerGateway";   //Server to Gateway   (Subscribe)
const char* mqtt_topicGS = "GatewayServer";   //Gateway to Server   (Publish)
const char* mqtt_topicGN = "GatewayNode";   //Gateway to Node       (Publish)

char temperatureString[6];
byte mac[6];                     // the MAC address of your Wifi shield
String strMacAddress = "";  // String type of Mac address
String d[25];
String a = "";
int count = 1;
String macadd = "";
String action;
String Mac;
String value;
int ind1;
int ind2;
int ind3;


String Latitude = "31.4787"; // containing Latitude of the device
String Longitude = "74.4160"; //containing Longitude of the device

String strloaction = "," + Latitude + "," + Longitude;

WiFiClient espClient;
PubSubClient client(espClient);

void setup_wifi() {
  Serial.println(espClient);
  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  WiFi.macAddress(mac);
  macadd = WiFi.macAddress();
}

void callback(char* topic, byte* payload, unsigned int length) {
  // Node Topic Payload
  if (strcmp(topic, "NodeGateway") == 0) {
    Serial.print("Message arrived [");
    Serial.print(topic);
    Serial.print("] ");
    for (int i = 0; i < length; i++) {
      d[i] = (char)payload[i];
      Serial.print(d[i]);
      newv = newv + d[i];
    }
    if (newv.startsWith("T"))
    {
      storeTemp();
    }
    else if(newv.startsWith("C")){
      sendStatus();
    }
  }
  // Server Topic Payload
  else if (strcmp(topic, "ServerGateway") == 0) {
    Serial.print("Message arrived [");
    Serial.print(topic);
    Serial.print("] ");
    for (int i = 0; i < length; i++) {
      d[i] = (char)payload[i];
      Serial.print(d[i]);
      newv = newv + d[i];
    }
    if (newv.startsWith("F"))
    {
      firmwareUpdate();
    }
    else if (newv.startsWith("R"))
    {
      relay();
    }
    else if (newv.startsWith("D"))
    {
      dimming();
    }
  }


  Serial.println();
}


void setup() {
  // setup serial port
  Serial.begin(9600);
  s.begin(9600);
  // setup WiFi
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  client.connect("ESP8266Client");

}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect("ESP8266Client")) {
      Serial.println("connected");
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}


void loop() {
  if (!client.connected()) {
    reconnect();
  }
  if(client.connected()){
    Serial.println("Connected");
  }
  client.loop();
  client.subscribe(mqtt_topicNG);
  client.subscribe(mqtt_topicSG);
  client.setCallback(callback);

  value = "";
  Mac = "";
  action = "";
  newv = "";
  delay(100);
}

//===== Sending Firmware update request to Node Device =====//
void firmwareUpdate() {
  ind1 = newv.indexOf('-');
  action = newv.substring(0, ind1);
  ind2 = newv.indexOf('-', ind1 + 1 );
  Mac = newv.substring(ind1 + 1, ind2);
  value = newv.substring(ind2 + 1);
  int index = value.indexOf('W');
  String val = value.substring(index + 1);
  if (Mac == "A") {
    String msg = Mac + "-" + val;
    char charBuf[25];
    msg.toCharArray(charBuf, 25);

    char result[25];
    strcpy(result, charBuf);

    client.publish(mqtt_topicGN, result);
  }
  else if (Mac != "A") {
    value = "-" + value;
    Mac = "-" + Mac;
    String msg = "M" + Mac + value;
    char charBuf[50];
    msg.toCharArray(charBuf, 50);

    char result[50];
    strcpy(result, charBuf);
    client.publish(mqtt_topicGN, result);
  }
}

//===== Sending Relay State to Node Device =====//
void relay() {
  ind1 = newv.indexOf('-');
  action = newv.substring(0, ind1);
  ind2 = newv.indexOf('-', ind1 + 1 );
  Mac = newv.substring(ind1 + 1, ind2);
  value = newv.substring(ind2 + 1);

  if (Mac == "A") {
    String msg = Mac + "-" + value;
    char charBuf[25];
    msg.toCharArray(charBuf, 25);

    char result[25];
    strcpy(result, charBuf);

    client.publish(mqtt_topicGN, result);

  }

  else if (Mac != "A") {
    String msg = "M-" + Mac + "-" + value;
    char charBuf[50];
    msg.toCharArray(charBuf, 50);

    char result[50];
    strcpy(result, charBuf);
    client.publish(mqtt_topicGN, result);
  }
}

//===== Sending Dimming value to Node Device =====//
void dimming() {
  ind1 = newv.indexOf('-');
  action = newv.substring(0, ind1);
  ind2 = newv.indexOf('-', ind1 + 1 );
  Mac = newv.substring(ind1 + 1, ind2);
  value = newv.substring(ind2 + 1);


  if (s.available()) {
    int data = value.toInt();
    s.write(data);
    delay(1000);
  }
}



//===== Sending Node Data to Server =====//
void storeTemp() {
  int index = newv.indexOf('-');
  String init = newv.substring(0, index);
  int index1 = newv.indexOf('-', index + 1);
  String Macadd = newv.substring(index + 1, index1);
  String val = newv.substring(index1 + 1);
  val = val.toInt();

  String msg = val + "," + Macadd + strloaction;

  char charBuf[100];
  msg.toCharArray(charBuf, 100);

  char result[100];
  strcpy(result, charBuf);
  client.publish(mqtt_topicGS, result);
}

//Sending the device status to the Server
void sendStatus(){
  int index = newv.indexOf('-');
  String init=newv.substring(0, index);
  String mac = newv.substring(index+1);

  String msgs = "0," + mac + strloaction;
  Serial.print("Msg: ");
  Serial.println(msgs);
  char charBuf[100];
  msgs.toCharArray(charBuf,100);

  char result[100];
  strcpy(result, charBuf);
  client.publish(mqtt_topicGS, result);
}
