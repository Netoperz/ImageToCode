#include <M5Stack.h>

#define imgName dukeprofile
#define PicArray extern unsigned char 
PicArray imgName[]; 

void setup() {
  // put your setup code here, to run once:
  M5.begin();
  M5.Lcd.drawBitmap(0, 0, 320, 240, (uint16_t *)imgName);
}

void loop() {
  // put your main code here, to run repeatedly:

}
