#include <SoftwareSerial.h> // Arduino IDE <1.6.6
#include <PZEM004T.h>
#include <VirtualWire.h>

PZEM004T pzem(&Serial); // RX,TX
IPAddress ip(192, 168, 1, 1);

String deviceID = "pmez5";
String voltageSensorID = "apmt3";
String currentSensorID = "velk5";
String powerSensorID = "rwep9";
int  deviceStatus = 0;

//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;

void setup() {
  Serial.begin(9600);
  pzem.setAddress(ip);

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  vw_rx_start();
}
String votageSensor() {
  float v = pzem.voltage(ip);
  if (v < 0.0){
    v = 0.0;
    deviceStatus=1;
  }
  String voltageValue = String(v);
  return voltageValue;
}
String currentSensor() {
  float i = pzem.current(ip);
  if (i < 0.0) i = 0.0;
  String currentValue = String(i);
  return currentValue;
}
String powerSensor() {
  float p = pzem.power(ip);
  String powerValue = String(p);
}

void loop() {
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String getRFValue;
  boolean flag = false;

  if (vw_get_message(buf, &buflen)) // Non-blocking
  {
    int i;
    Serial.print("Got: ");
    for (i = 0; i < buflen; i++)
    {
      char c = buf[i];
      getRFValue += String(c);
    }
    Serial.print(getRFValue);
    if (getRFValue == deviceID)
    {
      flag = true;
    }
  }

  //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
  String voltageValue = votageSensor();
  String currentValue = currentSensor();
  String powerValue = powerSensor();
  String finalValue = deviceID +  ";" + deviceStatus + ";" + voltageSensorID + ":" + voltageValue
                      + ";" + currentSensorID + ":" + currentValue + ";" + powerSensorID 
                      + ":" + powerValue;

  Serial.println(finalValue);
  char sendValue[50];
  //chuyen doi String sang char*
  finalValue.toCharArray(sendValue, 50);
  const char *msg = sendValue ;
  if (flag == true) {
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx();
    delay(200);
    Serial.print(" da nhan");
    Serial.println();
  }
  flag = false;
}
