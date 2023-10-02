/**
 * 
 * 
 * ------------------------------------------------------------------------------------------
 * RST 5
 * SDA 53
 * MOSI 51
 * MISO 50
 * SCK 52
*/

#include <SPI.h>
#include <MFRC522.h>

#define RST_PIN         5          // Configurable, see typical pin layout above
#define SS_1_PIN        53         // Configurable, take a unused pin, only HIGH/LOW required, must be different to SS 2
#define SS_2_PIN        8          // Configurable, take a unused pin, only HIGH/LOW required, must be different to SS 1

#define NR_OF_READERS   1

void dump_byte_array(byte *buffer, byte bufferSize);
byte ssPins[] = {SS_1_PIN, SS_2_PIN};

MFRC522 mfrc522[NR_OF_READERS];   // Create MFRC522 instance.


void setup() {

  Serial.begin(9600); // Initialize serial communications with the PC
  while (!Serial);    // Do nothing if no serial port is opened (added for Arduinos based on ATMEGA32U4)

  SPI.begin();        // Init SPI bus

  for (uint8_t reader = 0; reader < NR_OF_READERS; reader++) {
    mfrc522[reader].PCD_Init(ssPins[reader], RST_PIN); // Init each MFRC522 card
  }
  
}

void loop() {

  for (uint8_t reader = 0; reader < NR_OF_READERS; reader++) {
    // Look for new cards

    if (mfrc522[reader].PICC_IsNewCardPresent() && mfrc522[reader].PICC_ReadCardSerial()) {
      dump_byte_array(mfrc522[reader].uid.uidByte, mfrc522[reader].uid.size);
      Serial.println();
      // Halt PICC
      mfrc522[reader].PICC_HaltA();
      // Stop encryption on PCD
      mfrc522[reader].PCD_StopCrypto1();
    } 
  } 
}

void dump_byte_array(byte *buffer, byte bufferSize) {
  if (bufferSize > 0){
    /* code */
    Serial.print(buffer[0] < 0x10 ? "0" : "");
    Serial.print(buffer[0], HEX);
    for (byte i = 1; i < bufferSize; i++) {
      Serial.print(buffer[i] < 0x10 ? ":0" : ":");
      Serial.print(buffer[i], HEX);
    }
  }
  
  
}
