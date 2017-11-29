#include <VirtualWire.h>

//Initialise Device ID
String deviceID = "hmna7";
String humanSensorID = "zxpu2";
String deviceStatus = "0";

//Define sensor input pin on arduino
#define humanSensor 5

//Define RF transmit and receive pin on arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;

//Initialise Global variable
String deviceStatus = "0";

//Startup setup for program
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);    // Debugging only
  Serial.println("Setup");
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  vw_rx_start();       // Start the receiver PLL running

  //Set Input and Output for sensor
  pinMode(humanSensor, INPUT);
}
//Deteck Human in the area
String checkHumanMovement() {
  String value = String(digitalRead(humanSensor));
  return value;
}

//Main program
void loop() {
  //Initialise variable
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String getRFValue;
  boolean flag = false;
  char sendValue[50];
  const char *msg = sendValue ;

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
    //Serial.println(getRFValue);
    if (getRFValue == deviceID)
    {
      flag = true;
    }
    if  (getRFValue == (deviceID + "1")) {
      deviceStatus = "1";
    }
    if (getRFValue == (deviceID + "0")) {
      deviceStatus = "0";
    }
  }
  //Read value from sensor
  String value = checkHumanMovement();
  String finalValue = deviceID + ";" + deviceStatus + ";" + humanSensorID + ":" + value;
  //Serial.println(finalValue);
  //If the deviceID is True sending the parameter value of device through RF
  finalValue.toCharArray(sendValue, 50);
  if (flag == true) {
    for (int i = 0; i < 1; i++) {
      delay(100);
      vw_send((uint8_t *)msg, strlen(msg));
      vw_wait_tx();
      delay(200);
    }
  }
  flag = false;
}
