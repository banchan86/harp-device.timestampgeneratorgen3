#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
    #define bool uint8_t
#endif
#ifndef true
    #define true 1
#endif
#ifndef false
    #define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_CONFIG(void);
void app_read_REG_DEVICES_CONNECTED(void);
void app_read_REG_REPEATER_STATUS(void);
void app_read_REG_BATTERY_RATE(void);
void app_read_REG_BATTERY(void);
void app_read_REG_BATTERY_THRESHOLD_LOW(void);
void app_read_REG_BATTERY_THRESHOLD_HIGH(void);
void app_read_REG_BATTERY_CALIBRATION0(void);
void app_read_REG_BATTERY_CALIBRATION1(void);
void app_read_REG_TIMER(void);
void app_read_REG_TIMER_FREQUENCY(void);

bool app_write_REG_CONFIG(void *a);
bool app_write_REG_DEVICES_CONNECTED(void *a);
bool app_write_REG_REPEATER_STATUS(void *a);
bool app_write_REG_BATTERY_RATE(void *a);
bool app_write_REG_BATTERY(void *a);
bool app_write_REG_BATTERY_THRESHOLD_LOW(void *a);
bool app_write_REG_BATTERY_THRESHOLD_HIGH(void *a);
bool app_write_REG_BATTERY_CALIBRATION0(void *a);
bool app_write_REG_BATTERY_CALIBRATION1(void *a);
bool app_write_REG_TIMER(void *a);
bool app_write_REG_TIMER_FREQUENCY(void *a);


#endif /* _APP_FUNCTIONS_H_ */
