#include <VirtualWire.h>
#include <SoftwareSerial.h> // Arduino IDE <1.6.6
#include "DHT.h" //Thu Vien Nhiet Do

// Dat ten hoac so hieu cho arduino.Vidu:Canh bao den, canh bao quat,...
//hoac dat theo so
String deviceID = "mtia8";
String gasSensorID = "gesp1";
String fireSensorID = "eifw6";
String smokeSensorID = "ekma5";
String leakSensorID = "keet7";
String temperatureSensorID = "ppet9";
String humiditySensorID = "msyu4";

//Dat ten hoac so cho cac cam bien gan vao arduino
String deviceStatus = "1";

//Thong bao cac cong cho sensor tren arduino
#define DHTPIN 5 //sensor nhietdo doam.
#define DHTTYPE DHT22
#define gasSensor A1//mq2
#define fireSensor 7
#define smokeSensor 8
#define leakWaterSenser 6

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
  pinMode(fireSensor, INPUT);
  pinMode(gasSensor, INPUT);
  pinMode(smokeSensor, INPUT);
  pinMode(leakWaterSenser, INPUT);
  dht.begin();
}

float humidityCalculation() {
  float humidity = dht.readHumidity();
  return humidity;
}
float temperatureCalculation() {
  float temperature = dht.readTemperature();
  return temperature;
}
String gasSensorCalculation() {
  long gas = analogRead(gasSensor);
  String value = "";
  if (gas > 100) {
    value = "1";
  } else if (gas < 100) {
    value = "0";
  }
  //String a = String(gas);
  //return a;
  return value;
}

String fireSensorCalculation() {
  int fire = digitalRead(fireSensor);
  String value = String(fire);
  return value;
}
String smokeSensorCalculation() {
  int smoke = digitalRead(smokeSensor);
  String value = String(smoke);
  return value;
}
String waterLeakSensorCalculation() {
  int leak = digitalRead(leakWaterSenser);
  String value = "";
  if (leak == 1) {
    value = "0";
  } else if (leak == 0) {
    value = "1";
  }
  return value;
}
void loop()
{
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
    if  (getRFValue == (deviceID + "1")) {
    deviceStatus = "1";
  }
  if (getRFValue == (deviceID + "0")) {
    deviceStatus = "0";
  }


  //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
  String temperatureValue = String(temperatureCalculation());
  String humidityValue = String(humidityCalculation());
  String gasValue = gasSensorCalculation();
  String smokeValue = smokeSensorCalculation();
  String fireValue = fireSensorCalculation();
  String leakWaterValue = waterLeakSensorCalculation();
  String finalValue = deviceID +  ";" + deviceStatus + ";" + temperatureSensorID + ":" + temperatureValue + ";"
                      + humiditySensorID + ":" + humidityValue + ";" + gasSensorID + ":" + gasValue + ";"
                      + smokeSensorID + ":" + smokeValue + ";" + fireSensorID + ":" + fireValue + ";"
                      + leakSensorID + ":" + leakWaterValue;
  Serial.println(finalValue);
  char sendValue[100];
  //chuyen doi String sang char*
  finalValue.toCharArray(sendValue, 100);
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
