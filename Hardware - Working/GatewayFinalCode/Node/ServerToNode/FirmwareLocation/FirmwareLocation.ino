#include <ESP8266WiFi.h>
#include <PubSubClient.h>

#include <ESP8266HTTPClient.h>
#include <ESP8266httpUpdate.h>

//=======================================================//
#define SLEEP_DELAY_IN_SECONDS 60
#define WAIT_BEFORE_SLEEP_IN_SECONDS 5
#define REFERENCE_VOLTAGE_ADC 3.3

//======================================================//
String newv = "";
String versions = "001";
const char* ssid = "Akhtar";
const char* password = "akhtar1234";
const char* mqtt_server = "broker.hivemq.com";
//const char* mqtt_topics = "Version/Mac";
const char* mqtt_topic = "GatewayNode";
char temperatureString[6];
byte mac[6];                     // the MAC address of your Wifi shield
String strMacAddress = "";  // String type of Mac address
String MacAddress = ""; //Mac address recieving from the server
String d[30];
String a = "";
int count = 1;
String macma = ""; //device mac
String action = "";

String Latitude = "31.533353"; // containing Latitude of the device
String Longitude = "74.3382643"; //containing Longitude of the device

String strloaction = "," + Latitude + "," + Longitude;

WiFiClient espClient;
PubSubClient client(espClient);

void setup_wifi() {
  Serial.println(espClient);
  delay(10);
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
  macma = WiFi.macAddress(); //device mac
  macma = "," + macma;

}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");

  for (int i = 0; i < length; i++) {
    d[i] = (char)payload[i];
    Serial.print(d[i]);
    newv = newv + d[i];
  }
  // getting version number for all devices
  Serial.println();

}

void setup() {
  // setup serial port
  Serial.begin(115200);
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
      //client.subscribe(mqtt_topic);
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

String mac2String(byte ar[]) {
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
  client.subscribe(mqtt_topic);
  client.setCallback(callback);

  if (newv.startsWith("A")) {
    int ind1 = newv.indexOf('-');  //finds location of first ,
    String value = newv.substring(ind1 + 1); //captures first data String
    if (value > versions) {     //For all devices
      WiFiClient clients; //client for http update
      Serial.println("Dowlnloading new version " + value);
      t_httpUpdate_return ret = ESPhttpUpdate.update(clients, "http://192.168.43.101/Blink.bin");

      switch (ret) {
        case HTTP_UPDATE_FAILED:
          Serial.printf("HTTP_UPDATE_FAILD Error (%d): %s\n", ESPhttpUpdate.getLastError(), ESPhttpUpdate.getLastErrorString().c_str());
          break;

        case HTTP_UPDATE_NO_UPDATES:
          Serial.println("HTTP_UPDATE_NO_UPDATES");
          break;

        case HTTP_UPDATE_OK:
          Serial.println("HTTP_UPDATE_OK");
          break;
      }

    }
  }
  else if (newv.startsWith("M")) {      //For a specific Device
    int ind1 = newv.indexOf('-');
    int ind2 = newv.indexOf('-', ind1 + 1);
    String Mac = newv.substring(ind1, ind2);
    String Value = newv.substring(ind2 + 1);
    if (Mac == macma) {
      if (Value > versions) {
        WiFiClient clients; //client for http update
        Serial.println("Dowlnloading new version " + Value);
        t_httpUpdate_return ret = ESPhttpUpdate.update(clients, "http://192.168.43.101/Blink.bin");

        switch (ret) {
          case HTTP_UPDATE_FAILED:
            Serial.printf("HTTP_UPDATE_FAILD Error (%d): %s\n", ESPhttpUpdate.getLastError(), ESPhttpUpdate.getLastErrorString().c_str());
            break;

          case HTTP_UPDATE_NO_UPDATES:
            Serial.println("HTTP_UPDATE_NO_UPDATES");
            break;

          case HTTP_UPDATE_OK:
            Serial.println("HTTP_UPDATE_OK");
            break;
        }
      }
    }
  }
  newv = "";
  delay(2000);
}
