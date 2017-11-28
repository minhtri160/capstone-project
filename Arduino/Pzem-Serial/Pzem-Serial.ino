#include <SoftwareSerial.h> // Arduino IDE <1.6.6
#include <PZEM004T.h>

PZEM004T pzem(11, 12); // RX,TX
IPAddress ip(192, 168, 1, 1);

String deviceID = "pmez5";
String voltageSensorID = "apmt3";
String currentSensorID = "velk5";
String powerSensorID = "rwep9";
int  deviceStatus = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pzem.setAddress(ip);
}

void loop() {
  // put your main code here, to run repeatedly:
  float v = pzem.voltage(ip);
  if (v < 0.0) {
    v = 0.0;
    deviceStatus = 0;
  } else if (v > 0.0) {
    deviceStatus = 1;
  }
  String voltageValue = String(v);

  float i = pzem.current(ip);
  if (i < 0.0) i = 0.0;
  String currentValue = String(i);

  float p = pzem.power(ip);
  if(p <0.0) p =0.0;
  String powerValue = String(p);

  String finalValue = deviceID +  ";" + deviceStatus + ";" + voltageSensorID + ":" + voltageValue
                      + ";" + currentSensorID + ":" + currentValue + ";" + powerSensorID
                      + ":" + powerValue;

  Serial.println(finalValue);
}
