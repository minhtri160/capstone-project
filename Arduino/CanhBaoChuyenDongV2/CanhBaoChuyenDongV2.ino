#include <VirtualWire.h>

const int MovementSensor = 4;
const int HumanDetect = 5;

String deviceID = "abc123";
String status1 = "1";

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
  pinMode(MovementSensor, INPUT);
  pinMode(HumanDetect, INPUT);
}

void loop() {
  String str1 = "Movement " + String(digitalRead(MovementSensor));
  //Serial.println(str1);
  String str2 = "Human Detect: " + String(digitalRead(HumanDetect));
  //Serial.println(str2);
  String finalValue = deviceID + ";" +status1 + ";" + str1 + ";" + str2;
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  String b; boolean flag = false;
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
      String a = String(c);
      b += a;
    }
    Serial.println(b);
    if (b == deviceID)
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
    Serial.println(msg);
    Serial.print(" da nhan");
    Serial.println();
  }
  //Serial.println();
  flag = false;
}
