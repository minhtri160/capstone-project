#include <VirtualWire.h>
#include <SoftwareSerial.h> // Arduino IDE <1.6.6
#include "DHT.h" //Thu Vien Nhiet Do

//Initialise Device ID
String device_ID = "mtia8";
String gasSensor_ID = "gesp1";
String fireSensor_ID = "eifw6";
String smokeSensor_ID = "ekma5";
String leakSensor_ID = "keet7";
String temperatureSensor_ID = "ppet9";
String humiditySensor_ID = "msyu4";

//Thong bao cac cong cho sensor tren arduino
#define DHTPIN 5 //sensor nhietdo doam.
#define DHTTYPE DHT22
#define gasSensor_pin A1//mq2
#define fireSensor_pin 7
#define smokeSensor_pin 8
#define leakWaterSenser_pin 6

//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int transmit_pin = 10;
const int receive_pin = 2;

//Initialise Global variable
String deviceStatus = "1";

DHT dht(DHTPIN, DHTTYPE);

//Startup setup for program
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

  //Set Input and Output for sensor
  pinMode(fireSensor_pin, INPUT);
  pinMode(gasSensor_pin, INPUT);
  pinMode(smokeSensor_pin, INPUT);
  pinMode(leakWaterSenser_pin, INPUT);
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
  long gas = analogRead(gasSensor_pin);
  String value = "";
  if (gas > 100) {
    value = "1";
  } else if (gas < 100) {
    value = "0";
  }
  return value;
}

String fireSensorCalculation() {
  int fire = digitalRead(fireSensor_pin);
  String value = String(fire);
  return value;
}
String smokeSensorCalculation() {
  int smoke = digitalRead(smokeSensor_pin);
  String value = String(smoke);
  return value;
}
String waterLeakSensorCalculation() {
  int leak = digitalRead(leakWaterSenser_pin);
  String value = "";
  if (leak == 1) {
    value = "0";
  } else if (leak == 0) {
    value = "1";
  }
  return value;
}

//Main program
void loop()
{
  //Initialise variable
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String getRFValue;
  boolean flag = false;
  char sendValue[100];
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
  }
  //Serial.print(getRFValue);
  if (getRFValue == device_ID)
  {
    flag = true;
  }

  if  (getRFValue == (device_ID + "1")) {
    deviceStatus = "1";
  }
  if (getRFValue == (device_ID + "0")) {
    deviceStatus = "0";
  }

  //Read value from sensor
  String temperatureValue = String(temperatureCalculation());
  String humidityValue = String(humidityCalculation());
  String gasValue = gasSensorCalculation();
  String smokeValue = smokeSensorCalculation();
  String fireValue = fireSensorCalculation();
  String leakWaterValue = waterLeakSensorCalculation();
  String finalValue = device_ID +  ";" + deviceStatus + ";" + temperatureSensor_ID + ":" + temperatureValue + ";"
                      + humiditySensor_ID + ":" + humidityValue + ";" + gasSensor_ID + ":" + gasValue + ";"
                      + smokeSensor_ID + ":" + smokeValue + ";" + fireSensor_ID + ":" + fireValue + ";"
                      + leakSensor_ID + ":" + leakWaterValue;
  //Serial.println(finalValue);

  //chuyen doi String sang char*
  finalValue.toCharArray(sendValue, 100);

  //If the deviceID is True sending the parameter value of device through RF
  if (flag == true) {
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx();
    delay(200);
    Serial.print(" da nhan");
    Serial.println();
  }
  flag = false;
}
