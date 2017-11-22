#include <SoftwareSerial.h> // Arduino IDE <1.6.6
#include <PZEM004T.h>
#include <VirtualWire.h>

PZEM004T pzem(&Serial1); // RX,TX
IPAddress ip(192, 168, 1, 1);

String deviceID = "aadf9";

//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int transmit_pin = 4;
const int receive_pin = 2;

void setup() {
  Serial.begin(9600);
  pzem.setAddress(ip);

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  vw_rx_start();
}
String dodong() {
  float v = pzem.voltage(ip);
  if (v < 0.0) v = 0.0;
  String voltageValue = String(v);

  float i = pzem.current(ip);
  if (i < 0.0) i = 0.0;
  String currentValue = String(i);

  String value = "Ap:" + voltageValue + "V;Dong:" + currentValue + "A";
  return value;
}

void loop() {
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
  String giatrido = dodong();
  String finalValue = deviceID +  ";" + status1 + ";" + giatrido;
  Serial.println(finalValue);
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
