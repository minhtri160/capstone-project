#include <VirtualWire.h>

//Initialise Device ID
 
 
 
 
 
String deviceID = "quat1";
String SensorIDnhiet = "t2v2c:";
String SensorIDvong ="e1b3n:";
String SensorIDDong ="e1b4m:";

 
 
 
 

//Define sensor input pin on arduino
#define relayControl 6
#define PIN_DO 3// vong quay
int sensorPin = A1;//nhiet do

//Thong bao cac cong nhan cua cac thiet bi tren arduino
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

void setup()
{
  Serial.begin(9600);    // Debugging only
  Serial.println("Setup");
  pinMode(PIN_DO, INPUT);
  pulses = 0;
  timeOld = 0;
  attachInterrupt(digitalPinToInterrupt(PIN_DO), counter, FALLING);

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  vw_rx_start();       // Start the receiver PLL running

  //Thiet lap xuat nhap cho arduino
}


void counter()
{
  pulses++;
}


float donhiet() {
  int reading = analogRead(sensorPin);
  float voltage = reading * 5.0 / 1024.0;

  // ở trên mình đã giới thiệu, cứ mỗi 10mV = 1 độ C.
  // Vì vậy nếu biến voltage là biến lưu hiệu điện thế (đơn vị Volt)
  // thì ta chỉ việc nhân voltage cho 100 là ra được nhiệt độ!

  float temp = voltage * 100.0;
  //Serial.println("Nhiệt độ =\t");
  //Serial.println(temp);
  return temp;
}

float dovong() {
  if (millis() - timeOld >= 1000)
  {
    detachInterrupt(digitalPinToInterrupt(PIN_DO));
    rpm = (pulses * 60) / (HOLES_DISC);
    timeOld = millis();
    pulses = 0;
    attachInterrupt(digitalPinToInterrupt(PIN_DO), counter, FALLING);
    int reading = analogRead(sensorPin);

  }
  return rpm;
}
/*
float dodong() {
  for (int i = 0; i < 1000; i++) {
    Voltage = (Voltage + (0.0049 * analogRead(A5)));   // (5 V / 1024 = 0.0049) which converter Measured analog input voltage to 5 V Range
    return Voltage = 0;
  }
  Voltage = Voltage / 1000;
  Current = (Voltage - 2.5) / 0.185; // Sensed voltage is converter to current

  Serial.print("\n Voltage Sensed (V) = "); // shows the measured voltage
  Serial.print(Voltage, 2); // the '2' after voltage allows you to display 2  digits after decimal point
  Serial.print("\t Current (A) = ");   // shows the voltage measured
  Serial.print(Current, 2);
}
*/

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
 
 /*
  //String giatridodong = String(dodong());
  //String finalValue = deviceID +  ";" + deviceStatus + ";" + "dovong:" + giatridovong
                      //+ ";" + "donhiet:" + giatridonhiet + ";" + "dodong:" + giatridodong;
 */
 
 
 //String giatridodong = String(dodong());
 String finalValue = deviceID +  ";" + deviceStatus + ";" +SensorIDnhiet + giatridovong
  + ";" +SensorIDvong + giatridonhiet + ";" /*+ SensorIDDong + giatridodong*/;

 
 
 
 
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
  flag=false;
}
