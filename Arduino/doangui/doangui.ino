#include <VirtualWire.h>
#define PIN_DO 18// vong quay
volatile unsigned int pulses;
float rpm;
unsigned long timeOld;
#define HOLES_DISC 2
int sensorPin = A1;//nhiet do
double Voltage = 0;
double Current = 0;
// Dat ten hoac so hieu cho arduino.Vidu:Canh bao den, canh bao quat,...
//hoac dat theo so
String deviceID = "def789";

//Dat ten hoac so cho cac cam bien gan vao arduino
String sensorId = "1";

//Thong bao cac cong cho sensor tren arduino
#define sensorDongDien 7
#define sensorDienAp 8

//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;



long randomNumber;
void counter()
{
  pulses++;
}
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
  pinMode(sensorDongDien, INPUT);
  pinMode(sensorDienAp, INPUT);
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
float dodong() {
  for (int i = 0; i < 1000; i++) {
    Voltage = (Voltage + (.0049 * analogRead(A5)));   // (5 V / 1024 = 0.0049) which converter Measured analog input voltage to 5 V Range
    return Voltage = 0;
  }
  Voltage = Voltage / 1000;
  Current = (Voltage - 2.5) / 0.185; // Sensed voltage is converter to current

  Serial.print("\n Voltage Sensed (V) = "); // shows the measured voltage
  Serial.print(Voltage, 2); // the '2' after voltage allows you to display 2  digits after decimal point
  Serial.print("\t Current (A) = ");   // shows the voltage measured
  Serial.print(Current, 2);
}


void loop()
{ /*
    String status1 = "1";
    //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
    String giatridovong = String(dovong());
    String giatridonhiet = String(donhiet());
    String giatridodong = String(dodong());
    String finalValue = deviceID +  ";" + status1 + ";" + "dovong:" + giatridovong
                      + ";" + "donhiet:" + giatridonhiet + ";" + "dodong:" + giatridodong;
    //String finalValue ="2dd";
    //Serial.println(finalValue);
    //chuyen doi String sang char*
    char sendValue[50];
    finalValue.toCharArray(sendValue, 50);
  */
  char sendValue[50];
  /*
    //Gui chuoi gia tri di
    const char *msg = sendValue ;
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx(); // Wait until the whole message is gone
  */

  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String b;
  boolean flag = false;

  if (vw_get_message(buf, &buflen)) // Non-blocking
  {
    int i;
    Serial.print("Got: ");
    for (i = 0; i < buflen; i++)
    {
      char c = buf[i];
      String a = String(c);
      b += a;
    }
    Serial.print(b);
    if (b == deviceID)
    {
      flag = true;
    }
  }
  String status1 = "1";
  //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
  String giatridovong = String(dovong());
  String giatridonhiet = String(donhiet());
  String giatridodong = String(dodong());
  String finalValue = deviceID +  ";" + status1 + ";" + "dovong:" + giatridovong
                      + ";" + "donhiet:" + giatridonhiet + ";" + "dodong:" + giatridodong;
  //String finalValue ="2dd";
  //delay(1000);
  //Serial.println(finalValue);
  //chuyen doi String sang char*
  finalValue.toCharArray(sendValue, 50);
  const char *msg = sendValue ;
  
  if (flag == true) {
    for (int i = 0; i < 1; i++) {
      delay(100);
      vw_send((uint8_t *)msg, strlen(msg));
      vw_wait_tx();
      delay(200);
    }
    Serial.println(msg);
    Serial.print(" da nhan");
    Serial.println();
  }
  //Serial.println();
  flag = false;
}
/*
  //Gui chuoi gia tri di
  const char *msg = sendValue ;
  vw_send((uint8_t *)msg, strlen(msg));
  vw_wait_tx(); // Wait until the whole message is gone
*/
/*  float temp = donhiet();
  float rpm = dovong();
  Serial.print("Nhiệt độ =\t");
  Serial.println(temp);
  Serial.print("RPM =\t");
  Serial.println(rpm);
  Serial.print("\n Voltage Sensed (V) = "); // shows the measured voltage
  Serial.print(Voltage, 2); // the '2' after voltage allows you to display 2  digits after decimal point
  Serial.print("\t Current (A) = ");   // shows the voltage measured
  Serial.print(Current, 2);*/
//delay(200);


