//Arduino code
#include <SoftwareSerial.h>
SoftwareSerial s(5, 6); //Object for Serial Communication 
int data; 
int lastData = 0;
int LedPin = 10;
int LedPin1 = 11;
int LedPin2 = 9;
int LedPin3 = 3;
void setup() {
  s.begin(9600);
  Serial.begin(9600);
  //Setting pinmode 
  pinMode(LedPin, OUTPUT);
  pinMode(LedPin1, OUTPUT);
  pinMode(LedPin2, OUTPUT);
  pinMode(LedPin3, OUTPUT);
  //Turning led's to off state
  digitalWrite(LedPin, LOW);
  digitalWrite(LedPin1, LOW);
  digitalWrite(LedPin2, LOW);
  digitalWrite(LedPin3, LOW);
}

void loop() {
  s.write("s");
  //Reading Data from serial via Bluetooth
  if (s.available() > 0)
  {
    data = s.read();
    Serial.println(data);
  }
  Serial.print("Last Data");
  Serial.println(lastData);
  //comparison for last  data with the newly arrived data
  if (data != lastData) {
    if (data > lastData) {
      if (data > 0 && data < 101) {
        int val = data * 10.24;
        for (int i = 0; i < val; i += 10) {
          analogWrite(LedPin, i);
          analogWrite(LedPin1, i);
          analogWrite(LedPin2, i);
          analogWrite(LedPin3, i);
          delay(100);
          lastData = data;
        }

      }
    }
    else if (data < lastData) {
      if (data > 0 && data < 101) {
        int val = data * 10.24;
        for (int i = lastData; i > data; i -= 10) {
          analogWrite(LedPin, i);
          analogWrite(LedPin1, i);
          analogWrite(LedPin2, i);
          analogWrite(LedPin3, i);
          delay(100);
          lastData = data;

        }

      }
    }
  }
  delay(2000);

}
