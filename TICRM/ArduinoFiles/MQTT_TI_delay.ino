
#include <ESP8266WiFi.h>
#include <PubSubClient.h>

//=======================================================//
#define SLEEP_DELAY_IN_SECONDS 60
#define WAIT_BEFORE_SLEEP_IN_SECONDS 5
#define REFERENCE_VOLTAGE_ADC 3.3

//======================================================//

// initializes or defines the output pin of the LM35 temperature sensor
int outputpin = A0;

float readingIn=0;
const char* ssid = "Mansoor";
//const char* ssid = "Guest";
const char* password = "password";
const char* mqtt_server = "broker.hivemq.com";
const char* mqtt_topic = "TiTempSensor";
const char* mqtt_listen_topic="UbloxInputTopic";
const char* mqtt_listen_topic_delay="delay";
const char *APssid = "NodeMCU-17a";
const char *DeviceID="IoTPoCMQTT-17a";
const char *APpassword = "test12345";
//char temperatureString[6];
char mqttMessage[200];
int count;

String Latitude = "31.533353"; // containing Latitude of the device
String Longitude = "74.3382643"; //containing Longitude of the device

String strloaction = "," + Latitude + "," + Longitude;

//float readingIn = 0;
//const char* ssid = "";
//const char* password = "";
//const char* mqtt_server = "broker.hivemq.com";
//const char* mqtt_topic = "";
char temperatureString[6];
byte mac[6];                     // the MAC address of your Wifi shield
String strMacAddress = "";  // String type of Mac address



WiFiClient espClient;
PubSubClient client(espClient);
int delay_in_ms = 500;



void setup_wifi() {
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

  strMacAddress = "";

  Serial.print("MAC: ");
  Serial.print(mac[5], HEX);
  Serial.print(":");
  Serial.print(mac[4], HEX);
  Serial.print(":");
  Serial.print(mac[3], HEX);
  Serial.print(":");
  Serial.print(mac[2], HEX);
  Serial.print(":");
  Serial.print(mac[1], HEX);
  Serial.print(":");
  Serial.println(mac[0], HEX);
  
}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
  }
  Serial.println();
  if (strcmp(topic, mqtt_listen_topic_delay)==0){
    int t = atoi((char*)payload);
    delay_in_ms = t;
    Serial.print("Delay set to value: ");
    Serial.println(t);
  }
}

void setup() {
  // setup serial port
  Serial.begin(115200);

  // setup WiFi
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  client.connect("TIESPClient");
  client.subscribe(mqtt_listen_topic_delay);

}
void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect("TIESPClient")) {
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

float getTemperature() {
  int analogValue = analogRead(outputpin);
  float millivolts = (analogValue / 1024.0) * 3300; //3300 is the voltage provided by NodeMCU
  float celsius = millivolts / 10;
  Serial.print("Temperature in DegreeC= ");
  Serial.println(celsius);

   //return 25.81;
  return celsius;
}


String mac2String(byte ar[]){
  String s;
  for (byte i = 0; i < 6; ++i)
  {
    char buf[3];
    sprintf(buf, "%2X", ar[i]);
    s += buf;
    if (i < 5) s += ':';
  }
  return s;
}


void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();

  float temperature = getTemperature();

  // convert temperature to a string with two digits before the comma and 2 digits for precision
  dtostrf(temperature, 2, 2, temperatureString);

  // send temperature to the serial console
  Serial.print( "Sending temperature: ");
  Serial.println(temperatureString );

  // get mac address in a string format 
  strMacAddress = ',' + mac2String(mac);
  char charBuf[25];
  strMacAddress.toCharArray(charBuf, 25);

  // get the location 
  char charLocation[25];
  strloaction.toCharArray(charLocation, 25);


  
  char result[100];   // array to hold the result. temrature and amac address of the device
  
  strcpy(result,temperatureString); // copy string one into the result.
  strcat(result,charBuf); // append string two to the result.
  strcat(result,charLocation); // append Location to the result.
  // send temperature and MAC Address to the MQTT topic
  client.publish(mqtt_topic, result );

  delay(delay_in_ms);

  // Serial.println( "Closing MQTT connection...");
  //  client.disconnect();

  // Serial.println( "Closing WiFi connection...");
  //WiFi.disconnect();

//  delay(100);
//  Serial.println("Waiting 30 seconds before deep sleep");
//  delay(1000 * WAIT_BEFORE_SLEEP_IN_SECONDS);
//  Serial.print("Entering deep sleep mode for ");
//  Serial.print(SLEEP_DELAY_IN_SECONDS);
//  Serial.println(" seconds");
//
//  //ESP.deepSleep(SLEEP_DELAY_IN_SECONDS * 1000000, WAKE_RF_DEFAULT);

}