#include <VirtualWire.h>
#include <SoftwareSerial.h> // Arduino IDE <1.6.6

//Initialise Device ID
String deviceID = "httc8";

//Define sensor input pin on arduino
#define lightSensor 8
#define relayControl 6

//Define RF transmit and receive pin on arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;

//Another Define pin on arduino

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

}

//Check if Light Bulb is emit or not
String checkLight() {
  String value = "Light:" + String(digitalRead(lightSensor));
  return value;
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
  char sendValue[50];
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String getRFValue;
  boolean flag = false;
  String deviceStatus = "0";

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
    Serial.println();
  }
  if (getRFValue == deviceID)
  {
    flag = true;
  }
  if  (getRFValue == (deviceID + "1")) {
    turnOn();
    deviceStatus = "1";
    Serial.print("Light is ON!");
  }
  if (getRFValue == (deviceID + "0")) {
    turnOff();
    deviceStatus = "0";
    Serial.print("Light is OFF!");
  }
  //Serial.print(getRFValue[5]);
  //Serial.println();

  String finalValue = deviceID +  "; " + deviceStatus + "; " + checkLight();

  Serial.println(finalValue);
  //chuyen doi String sang char*
  finalValue.toCharArray(sendValue, 50);
  const char *msg = sendValue ;
  if (flag == true) {
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx();
    delay(200);
    Serial.print(" Sent!");
    Serial.println();
  }
}
