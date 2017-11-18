#include <VirtualWire.h>
#include <SoftwareSerial.h> // Arduino IDE <1.6.6
// Dat ten hoac so hieu cho arduino.Vidu:Canh bao den, canh bao quat,...
//hoac dat theo so
String deviceID = "asg";
//Thong bao cac cong cho sensor tren arduino
#define sensorSang 8
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;
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
  pinMode(sensorSang, INPUT);
}


String Dosang() {
  String value = "Dosang:" + String(digitalRead(8));
  return value;
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
  
  String giatriDoSang = Dosang();
  String finalValue = deviceID +  ";" + status1 + ";" + giatriDoSang;

  Serial.println(finalValue);
  char sendValue[100];
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
