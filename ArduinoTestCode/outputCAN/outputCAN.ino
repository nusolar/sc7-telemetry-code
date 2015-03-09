/* 
 * This file contains code to generate test CAN packets on the Arduino Due
 * for use by the 2014-2015 telemetry team.
 * Authors:
 *   - Alexander Martin
 *   - < Please add yourselves as you edit >
 */

//#define DEBUG

#include <SPI.h>
#include "sc7-can-libinclude.h"

#define CANINT 8
#define CANCS 5
#define CAN_NBT 1000  //Nominal Bit Time
#define CAN_FOSC 16

CAN_IO can(CANCS,CANINT,CAN_NBT,CAN_FOSC);
uint16_t        errors = 0; // for catching can errors
long            lastmillis = 0; // for timing 
long            odometer = 0;

void loop()
{
  /* Write CAN (Make changes in this section) *********************************/
  //Generate Random Can Packets and Send them
  Layout packet;
  int choice = random(0,10);
  Serial.print("PKT#: ");
  Serial.println(choice);

  switch(choice)
  {

    case 0:{
      packet = BMS_FanStatus(150*sin(millis()/1000.0), 150*cos(millis()/1000.0), 5, 7);
    break;}

    case 1:{
      float current = 2.5 + random(-2.5, 2.5);
      packet = MC_BusStatus(current, 120.0);
    break;}

    case 2:{
      packet = MC_OdoAmp(odometer/40000.0, odometer);
    break;}

    case 3:
    default:{ //Move default down later as more cases are added
      float cvel = random(30);
      float mvel = cvel + random(-3,3);
      packet = MC_Velocity(cvel,mvel);
    break;}
  }

  can.Send(packet, TXB0); // Send a packet out of port 0
  odometer++;             // Increment odometer
  delay(100);             // Wait a little bit for the message to send.
    
#ifdef DEBUG
	Serial.print("TEC: ");
	Serial.println(can.controller.Read(TEC), BIN);
	Serial.print("REC: ");
	Serial.println(can.controller.Read(REC), BIN);
	Serial.print("EFLG: ");
	Serial.println(can.controller.Read(EFLG), BIN);
#endif

	/* Read CAN   ****************************************/
	while (can.Available())
	{
		Frame& f = can.Read();
		char str[50];
		switch (f.id)
		{
		  case DC_DRIVE_ID:
		  {
			DC_Drive packet(f); //Get the drive packet
			sprintf(str, "Id: %x, Vel: %.1f, Cur: %.4f,", packet.id, packet.velocity, packet.current);
			Serial.println(str); 
			break;
		  }
		  case MC_VELOCITY_ID:
		  {
			MC_Velocity packet(f);
			sprintf(str, "Id: %x, CarVel: %f, MotVel: %f", packet.id, packet.car_velocity, packet.motor_velocity);
			Serial.println(str);
			break;
		  }
      default:
      {
            Serial.print("Id: ");
            Serial.println(f.id,HEX);
            break;
      }
	  }
		
		//Print out buffer size so we can see if there is overflow (this is not accurate when DEBUG is enabled.)
		if (millis() > lastmillis + 1000)
		{
      lastmillis = millis();
		  Serial.print("Buffer Count:");
		  Serial.println(can.RXbuffer.size());
		}
	}
}

void setup()
{
    //Start Serial and wait for MCP2515 to finish 128 clock cycles
    Serial.begin(9600);
    Serial.println("BEGIN TEST CODE");
    delay(100);
	
    //Create a CAN filter structure
    CANFilterOpt filter;
    filter.setRB0(MASK_NONE,0,0);
    filter.setRB1(MASK_NONE,0,0,0,0);

    //Set up CANS
    can.Setup(filter, &errors);
    
#ifdef DEBUG
	Serial.println(errors, BIN);
#endif
}

