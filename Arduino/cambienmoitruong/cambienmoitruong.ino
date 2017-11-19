#include <VirtualWire.h>
#include <SoftwareSerial.h> // Arduino IDE <1.6.6
#include "DHT.h" //Thu Vien Nhiet Do

// Dat ten hoac so hieu cho arduino.Vidu:Canh bao den, canh bao quat,...
//hoac dat theo so
String deviceID = "httc8";

//Dat ten hoac so cho cac cam bien gan vao arduino
String sensorId = "1";

//Thong bao cac cong cho sensor tren arduino
#define DHTPIN 9 //sensor nhietdo doam.
#define DHTTYPE DHT22
#define sensorKhiGas A3//mq2
#define sensorLua 7
#define sensorKhoi 8

//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int transmit_pin = 10;
const int receive_pin = 2;


DHT dht(DHTPIN, DHTTYPE);
void setup()
{
  Serial.begin(9600);    // Debugging only
  Serial.println("Setup");

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(3);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  vw_rx_start();
  //Thiet lap xuat nhap cho arduino
  // pinMode(sensorSang, INPUT);
  pinMode(sensorLua, INPUT);
  //pinMode(sensorKhiGas, INPUT);
  pinMode(sensorKhoi, INPUT);
  dht.begin();
}

float doDoAm() {
  //delay(1000);
  float h = dht.readHumidity();
  return h;
}
float doNhiet() {
  float t = dht.readTemperature();
  return t;
}
String doKhiGas() {
  float gas = analogRead(sensorKhiGas);
  String value = String(gas);
  return value;
}

String doLua() {
  int dataRead = digitalRead(sensorLua);
  String result = String(dataRead);
  return result;
}
String doKhoi() {
  int smoke = digitalRead(sensorKhoi);
  String result = String(smoke);
  return result;
}

void loop()
{
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

  String status1 = "0";
  //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
  String giatrinhietdo = String(doNhiet());
  String giatridoam = String(doDoAm());
  String giatrikhigas = doKhiGas();
  String giatrikhoi = String(doKhoi());
  String giatriLua = doLua();
  String finalValue = deviceID +  ";" + status1 + ";" + "Khoi:" + giatrikhoi + ";" + "Gas:" + giatrikhigas + ";"
                      + "Nhietdo:" + giatrinhietdo + ";" + "Do am:" + giatridoam + ";" + "Lửa:" + giatriLua;

  //Serial.println(finalValue);
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
}
