#include <ESP8266WiFi.h>
#include <PubSubClient.h>

#include <ESP8266HTTPClient.h>
#include <ESP8266httpUpdate.h>
//=======================================================//
#define SLEEP_DELAY_IN_SECONDS 60
#define WAIT_BEFORE_SLEEP_IN_SECONDS 5
#define REFERENCE_VOLTAGE_ADC 3.3

//======================================================//

// initializes or defines the output pin of the LM35 temperature sensor
int relayInput = 2; // the input to the relay pin

String newv = "";
const char* ssid = "TI_DEV";
const char* password = "TechDeve";
const char* mqtt_server = "broker.hivemq.com";
const char* mqtt_topic = "GatewayNode";
const char* mqtt_topicNG = "NodeGateway";
byte mac[6];                     // the MAC address of your Wifi shield
String strMacAddress = "";  // String type of Mac address
String d[7];
String a = "";
int count = 1;
String macma;


#define ORG "lh5htt" // "quickstart" or use your organisation
#define DEVICE_ID "Device01"      
#define DEVICE_TYPE "Arduino" // your device type or not used for "quickstart"
#define TOKEN "123123123" // your device token or not used for "quickstart"
//-------- Customise the above values --------

char server[] = ORG ".messaging.internetofthings.ibmcloud.com";
char topic[] = "iot-2/evt/status/fmt/json";
char authMethod[] = "use-token-auth";
char token[] = TOKEN;
char clientId[] = "d:" ORG ":" DEVICE_TYPE ":" DEVICE_ID;



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
  macma = WiFi.macAddress();
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
  Serial.println();
}

void setup() {
  // setup serial port
  Serial.begin(115200);

  pinMode(relayInput, OUTPUT); // initialize pin as OUTPUT

  // setup WiFi
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  client.subscribe(mqtt_topic);
  client.connect("ESP8266Clients");

}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect("ESP8266Client")) {
      Serial.println("connected");
      client.subscribe(mqtt_topic);
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

  if (newv.startsWith("A")) {     //For all devoces
    int ind1 = newv.indexOf('-');
    String State = newv.substring(ind1 + 1);
    if (State == "True") {
      digitalWrite(relayInput, HIGH); // turn relay off
    }
    if (State == "False") {
      digitalWrite(relayInput, LOW); // turn relay on
    }
    Serial.println("Arrived Message: " + newv);
    newv = "";
  }
  else if (newv.startsWith("M")) {      //For specific Device
    int ind1 = newv.indexOf('-');
    int ind2 = newv.indexOf('-', ind1 + 1);
    String Mac = newv.substring(ind1 + 1, ind2);
    String State = newv.substring(ind2);
    if (Mac == macma) {
      if (State == "True") {
        digitalWrite(relayInput, HIGH); // turn relay off
      }
      if (State == "False") {
        digitalWrite(relayInput, LOW); // turn relay on
      }
      newv = "";
    }
  }
  //Sending connection status to the Gateway
  String msg = "C-" + macma;
  char charBuf[100];
  msg.toCharArray(charBuf, 100);

  char result[100];
  strcpy(result, charBuf);
  client.publish(mqtt_topicNG, result);
}


void setup() {
  Serial.begin(115200); Serial.println();

  Serial.print("Connecting to "); Serial.print(ssid);
  if (strcmp (WiFi.SSID().c_str(), ssid) != 0) {
     WiFi.begin(ssid, password);
  }
  while (WiFi.status() != WL_CONNECTED) {
     delay(500);
     Serial.print(".");
  }  
  Serial.println("");
  Serial.print("WiFi connected, IP address: "); Serial.println(WiFi.localIP());

  Serial.println("View the published data on Watson at: "); 
  if (ORG == "quickstart") {
    Serial.println("https://quickstart.internetofthings.ibmcloud.com/#/device/" DEVICE_ID "/sensor/");
  } else {
    Serial.println("https://" ORG ".internetofthings.ibmcloud.com/dashboard/#/devices/browse/drilldown/" DEVICE_TYPE "/" DEVICE_ID);
  }
}


void loop() {
   if (!!!client.connected()) {
      Serial.print("Reconnecting client to "); Serial.println(server);
      while ( ! (ORG == "quickstart" ? client.connect(clientId) : client.connect(clientId, authMethod, token))) {
        Serial.print(".");
        delay(500);
     }
     Serial.println();
   }

  String payload = "{\"d\":{\"myName\":\"ESP8266.Test5\",\"counter\":";
  payload += millis() / 1000;
  payload += "}}";
  
  Serial.print("Sending payload: "); Serial.println(payload);
    
  if (client.publish(topic, (char*) payload.c_str())) {
    Serial.println("Publish ok");
  } else {
    Serial.println("Publish failed");
  }

  delay(5000);
}
