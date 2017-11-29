#include <SoftwareSerial.h> // Arduino IDE <1.6.6
#include <PZEM004T.h>
#include <VirtualWire.h>

PZEM004T pzem(&Serial); // RX,TX
IPAddress ip(192, 168, 1, 1);

//Initialise Device ID
String deviceID = "pmez5";
String voltageSensorID = "apmt3";
String currentSensorID = "velk5";
String powerSensorID = "rwep9";

//Define RF transmit and receive pin on arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;

//Initialise Global variable
String  deviceStatus = "0";

//Startup setup for program
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

//Reading voltage from load
String votageSensor() {
  float v = pzem.voltage(ip);
  if (v < 0.0) {
    v = 0.0;
    deviceStatus = "1";
  } else {
    deviceStatus = "0";
  }
  String voltageValue = String(v);
  return voltageValue;
}

//Reading current from load
String currentSensor() {
  float i = pzem.current(ip);
  if (i < 0.0) i = 0.0;
  String currentValue = String(i);
  return currentValue;
}

//Reading power comsumption from load
String powerSensor() {
  float p = pzem.power(ip);
  String powerValue = String(p);
}

//Main program
void loop() {
  //Initialise variable
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String getRFValue;
  boolean flag = false;
  char sendValue[100];
  const char *msg = sendValue;

  //Checking if there is any RF message sent
  if (vw_get_message(buf, &buflen)) // Non-blocking
  {
    int i;
    Serial.print("Got: ");
    for (i = 0; i < buflen; i++)
    {
      char c = buf[i];
      getRFValue += String(c);
    }
  }
  //Checking the deviceID
  //Serial.print(getRFValue);
  if (getRFValue == deviceID)
  {
    flag = true;
  }

  //Read value from sensor
  String voltageValue = votageSensor();
  String currentValue = currentSensor();
  String powerValue = powerSensor();
  String finalValue = deviceID +  ";" + deviceStatus + ";" + voltageSensorID + ":" + voltageValue
                      + ";" + currentSensorID + ":" + currentValue + ";" + powerSensorID
                      + ":" + powerValue;

  //Serial.println(finalValue);

  //Change String to char*
  finalValue.toCharArray(sendValue, 100);
  //If the deviceID is True sending the parameter value of device through RF
  if (flag == true) {
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx();
    delay(200);
    //Serial.print(" da nhan");
    //Serial.println();
  }
  flag = false;
}
