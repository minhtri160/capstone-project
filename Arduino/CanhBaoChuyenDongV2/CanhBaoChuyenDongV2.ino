#include <VirtualWire.h>

String deviceID = "hmna7";
String humanSensorID = "zxpu2";

//Define sensor input pin on arduino
#define humanSensor 5

//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;


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
  //Thiet lap xuat nhap cho arduino
  pinMode(humanSensor, INPUT);
}

String checkHumanMovement() {
  String value = String(digitalRead(humanSensor));
  return value;
}

void loop() {
  String deviceStatus ="1";
  String value = checkHumanMovement();
  String finalValue = deviceID + ";" + deviceStatus + ";" + humanSensorID + ":" + value;
  //Serial.println(finalValue);
  
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String getRFValue;
  boolean flag = false;
  char sendValue[50];
  finalValue.toCharArray(sendValue, 50);
  const char *msg = sendValue ;
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
  }
  if (flag == true) {
    for (int i = 0; i < 1; i++) {
      delay(100);
      vw_send((uint8_t *)msg, strlen(msg));
      vw_wait_tx();
      delay(200);
    }
    //Serial.println(msg);
    //Serial.print(" da nhan");
    //Serial.println();
  }
  //Serial.println();
  flag = false;
  //delay(100);
}
