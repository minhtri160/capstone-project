#include <VirtualWire.h>

//Initialise Device ID
String deviceID = "quat1";
String temperatureSensorID = "t2v2c";
String rotationSensorID = "e1b3n";
String electricSensorID = "e1b4m";

//Define sensor input pin on arduino
#define relayControl 6
#define rotationSensor 3// vong quay
#define checkElectricSensor 7
#define temperatureSensor A1 //nhiet do

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
  vw_setup(2000);  // Bits per sec
  vw_rx_start();       // Start the receiver PLL running

  //Thiet lap xuat nhap cho arduino
  pinMode(rotationSensor, INPUT);
  pinMode(relayControl, OUTPUT);
  pinMode(checkElectricSensor, INPUT);
  pinMode(temperatureSensor, INPUT);
}

void counter()
{
  pulses++;
}


float donhiet() {
  int temp = analogRead(temperatureSensor);
  float voltage = temp * 5.0 / 1024.0;

  // ở trên mình đã giới thiệu, cứ mỗi 10mV = 1 độ C.
  // Vì vậy nếu biến voltage là biến lưu hiệu điện thế (đơn vị Volt)
  // thì ta chỉ việc nhân voltage cho 100 là ra được nhiệt độ!

  float tempValue = voltage * 100.0;
  //Serial.println("Nhiệt độ =\t");
  //Serial.println(temp);
  return tempValue;
}

float dovong() {
  if (millis() - timeOld >= 1000)
  {
    detachInterrupt(digitalPinToInterrupt(rotationSensor));
    rpm = (pulses * 60) / (HOLES_DISC);
    timeOld = millis();
    pulses = 0;
    attachInterrupt(digitalPinToInterrupt(rotationSensor), counter, FALLING);
    
//Thong bao cac cong nhan cua cac thiet bi tren arduinoint reading = analogRead(rotationSensor);
  }
  return rpm;
}
int checkElectric() {
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

void loop()
{ char sendValue[50];
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
    Serial.println();
  }
  if (getRFValue == deviceID)
  {
    flag = true;
  }
  if  (getRFValue == (deviceID + "1")) {
    turnOn();
    deviceStatus = "1";
    Serial.print("Fan is ON!");
  }
  if (getRFValue == (deviceID + "0")) {
    turnOff();
    deviceStatus = "0";
    Serial.print("Fan is OFF!");
  }
  //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
  String giatridovong = String(dovong());
  String giatridonhiet = String(donhiet());
  String giatridien = String(checkElectric());
  String finalValue = deviceID +  ";" + deviceStatus + ";" + rotationSensorID + ":" + giatridovong
                      + ";" + temperatureSensorID + ":" + giatridonhiet + ";" + electricSensorID + ":" + giatridien;
  Serial.println(finalValue);
  //chuyen doi String sang char*
  finalValue.toCharArray(sendValue, 50);
  const char *msg = sendValue ;
  if (flag == true) {
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx();
    delay(200);
    Serial.print(" da gui");
    Serial.println();
  }
  //Serial.println();
  flag = false;
}
