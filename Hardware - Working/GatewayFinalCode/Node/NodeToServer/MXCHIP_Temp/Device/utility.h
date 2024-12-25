// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. 

#ifndef UTILITY_H
#define UTILITY_H

bool readMessage(int, char *);

void SensorInit(void);

void blinkLED(void);
void blinkSendConfirmation(void);
int getInterval(void);

float readTemperature(void);
float readHumidity(void);

#endif /* UTILITY_H */