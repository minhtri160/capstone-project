#include <VirtualWire.h>
#include <SoftwareSerial.h> // Arduino IDE <1.6.6

//Initialise Device ID
String deviceID = "httc8";
String lightSensorID = "lmgh1";
String isElectricSensorID = "efga6";

//Define sensor input pin on arduino
#define lightSensor 8
#define relayControl 6
#define checkElectricSensor 5

//Define RF transmit and receive pin on arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;

//Initialise Global variable
String deviceStatus = "0";

//Startup setup for program
void setup()
{
  Serial.begin(9600);    // Debugging only
  Serial.println("Setup");

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  vw_rx_start();

  //Set Input and Output for sensor
  pinMode(lightSensor, INPUT);
  pinMode(relayControl, OUTPUT);
  pinMode(checkElectricSensor, INPUT);

}

//Check if Light Bulb is emitted or not
String checkLightStatus() {
  int light = digitalRead(lightSensor);
  int value = 0;
  if ( light == 0 ) {
    value = 1;
  } else {
    value = 0;
  }
  String returnValue = String(value);
  return returnValue;
}

//Check if there is electric
String checkElectStatus() {
  int value = digitalRead(checkElectricSensor);
  String returnvalue = String(value);
  return returnvalue;
}

//Turn on Device
void turnOn() {
  digitalWrite(relayControl, HIGH);
}

//Turn off Device
void turnOff() {
  digitalWrite(relayControl, LOW);
}

//Main program
void loop()
{
  //Initialise variable
  char sendValue[50];
  const char *msg = sendValue ;
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String getRFValue;
  boolean flag = false;

  //Checking if there is any RF message sent
  if (vw_get_message(buf, &buflen)) // Non-blocking
  {
    int i;
    //Serial.print("Got: ");
    for (i = 0; i < buflen; i++)
    {
      char c = buf[i];
      getRFValue += String(c);
    }
  }
  //Checking the deviceID
  if (getRFValue == deviceID)
  {
    flag = true;
  }
  //Checking
  if  (getRFValue == (deviceID + "1")) {
    turnOn();
    deviceStatus = "1";
    //Serial.print("Light is ON!");
  }
  if (getRFValue == (deviceID + "0")) {
    turnOff();
    deviceStatus = "0";
    //Serial.print("Light is OFF!");
  }
  //Read value from sensor
  String electValue = checkElectStatus();
  String light = checkLightStatus();
  
  String finalValue = deviceID +  ";" + deviceStatus + ";" + isElectricSensorID + ":" + electValue
                      + ";" + lightSensorID + ":" + light;
  //Serial.println(finalValue); //Debugging only
  //Change String to char*
  finalValue.toCharArray(sendValue, 50);
  //If the deviceID is True sending the parameter value of device through RF
  if (flag == true) {
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx();
    delay(200);
    //Serial.print(" Sent!"); //Debugging only
    //Serial.println();
  }
  flag = false;
}
