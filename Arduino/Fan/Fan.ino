#include <VirtualWire.h>

//Initialise Device ID
String deviceID = "quat1";
String temperatureSensorID = "t2v2c";
String rotationSensorID = "e1b3n";
String isElectricSensorID = "e1b4m";

//Define sensor input pin on arduino
#define relayControl 6
#define rotationSensor 3 //Rotation encoder sensor
#define checkElectricSensor 7
#define temperatureSensor A1 //Temperature LM35 sensor

//Define RF transmit and receive pin on arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 4;

//Initialise global variable
#define HOLES_DISC 2
double Voltage = 0;
double Current = 0;
float rpm;
unsigned long timeOld;
volatile unsigned int pulses;
String deviceStatus = "0";

//Startup setup for program
void setup()
{
  Serial.begin(9600);    // Debugging only
  Serial.println("Setup");
  pulses = 0;
  timeOld = 0;
  attachInterrupt(digitalPinToInterrupt(rotationSensor), counter, FALLING);

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);       // Bits per sec
  vw_rx_start();        // Start the receiver PLL running

  //Set Input and Output for sensor
  pinMode(rotationSensor, INPUT);
  pinMode(relayControl, OUTPUT);
  pinMode(checkElectricSensor, INPUT);
  pinMode(temperatureSensor, INPUT);
}

void counter()
{
  pulses++;
}
//Reading temperature from fan
float teperatureReading() {
  float temp = analogRead(temperatureSensor);
  float voltageRef = temp * 5.0 / 1024.0;

  // ở trên mình đã giới thiệu, cứ mỗi 10mV = 1 độ C.
  // Vì vậy nếu biến voltage là biến lưu hiệu điện thế (đơn vị Volt)
  // thì ta chỉ việc nhân voltage cho 100 là ra được nhiệt độ!

  float tempValue = voltageRef * 100.0;
  return tempValue;
}
//Reading rotation from Fan
float rotationReading() {
  if (millis() - timeOld >= 1000)
  {
    detachInterrupt(digitalPinToInterrupt(rotationSensor));
    rpm = (pulses * 60) / (HOLES_DISC);
    timeOld = millis();
    pulses = 0;
    attachInterrupt(digitalPinToInterrupt(rotationSensor), counter, FALLING);
  }
  return rpm;
}
int checkElectStatus() {
  int value = digitalRead(checkElectricSensor);
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
  
  if  (getRFValue == (deviceID + "1")) {
    turnOn();
    deviceStatus = "1";
    //Serial.print("Fan is ON!");
  }
  if (getRFValue == (deviceID + "0")) {
    turnOff();
    deviceStatus = "0";
    //Serial.print("Fan is OFF!");
  }
  //Read value from sensor
  String rotationValue = String(rotationReading());
  String temperatureValue = String(teperatureReading());
  String electValue = String(checkElectStatus());
  String finalValue = deviceID +  ";" + deviceStatus + ";" + rotationSensorID + ":" + rotationValue
                      + ";" + temperatureSensorID + ":" + temperatureValue + ";" + isElectricSensorID + ":" + electValue;
  //Serial.println(finalValue);//Debugging only
  //chuyen doi String sang char*
  finalValue.toCharArray(sendValue, 50);
  //If the deviceID is True sending the parameter value of device through RF
  if (flag == true) {
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx();
    delay(200);
    //Serial.print(" Sent!");//Debugging only
    //Serial.println();
  }
  flag = false;
}
