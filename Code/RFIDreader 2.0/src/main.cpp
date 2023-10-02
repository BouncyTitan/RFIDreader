#include <SPI.h>
#include <MFRC522.h>

/**
 * MFRC522 MOSI pin to Arduino Mega MOSI pin (51 on Mega)
 * MFRC522 MISO pin to Arduino Mega MISO pin (50 on Mega)
 * MFRC522 SCK pin to Arduino Mega SCK pin (52 on Mega)
*/

#define SS_PIN 53   // Define the chip select pin (SS) for the RFID reader (SDA)
#define RST_PIN 49  // Define the reset pin for the RFID reader

MFRC522 mfrc522(SS_PIN, RST_PIN); // Create an MFRC522 instance

void setup() {
  Serial.begin(9600); // Initialize serial communication
  SPI.begin();        // Initialize SPI communication
  mfrc522.PCD_Init(); // Initialize MFRC522 RFID reader
}

void loop() {
  // Look for new RFID cards/tags
  if (mfrc522.PICC_IsNewCardPresent() && mfrc522.PICC_ReadCardSerial()) {
    // Get the UID of the card/tag
    String uid = "";
    for (byte i = 0; i < mfrc522.uid.size; i++) {
      uid += String(mfrc522.uid.uidByte[i] < 0x10 ? "0" : "");
      uid += String(mfrc522.uid.uidByte[i], HEX);
      if (i < mfrc522.uid.size - 1) {
        uid += ":";
      }
    }
    
    uid.toUpperCase();

    // Send the UID to the serial monitor in the specified format
    Serial.print("#");
    Serial.println(uid);
    //Serial.println("\r");
    
    delay(1000); // Delay for a second to avoid reading the same card multiple times
  }
}
