#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"


/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_CONFIG,
	&app_read_REG_DEVICES_CONNECTED,
	&app_read_REG_REPEATER_STATUS,
	&app_read_REG_BATTERY_RATE,
	&app_read_REG_BATTERY,
	&app_read_REG_BATTERY_THRESHOLD_LOW,
	&app_read_REG_BATTERY_THRESHOLD_HIGH,
	&app_read_REG_BATTERY_CALIBRATION0,
	&app_read_REG_BATTERY_CALIBRATION1,
	&app_read_REG_TIMER,
	&app_read_REG_TIMER_FREQUENCY
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_CONFIG,
	&app_write_REG_DEVICES_CONNECTED,
	&app_write_REG_REPEATER_STATUS,
	&app_write_REG_BATTERY_RATE,
	&app_write_REG_BATTERY,
	&app_write_REG_BATTERY_THRESHOLD_LOW,
	&app_write_REG_BATTERY_THRESHOLD_HIGH,
	&app_write_REG_BATTERY_CALIBRATION0,
	&app_write_REG_BATTERY_CALIBRATION1,
	&app_write_REG_TIMER,
	&app_write_REG_TIMER_FREQUENCY
};


/************************************************************************/
/* REG_CONFIG                                                           */
/************************************************************************/
uint8_t battery_mode = 0;
uint8_t battery_cycle_state = 0;

void app_read_REG_CONFIG(void)
{
	app_regs.REG_CONFIG = battery_mode;
}

bool app_write_REG_CONFIG(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_START_BATTERY_CYCLE)
	{
		battery_mode = B_START_BATTERY_CYCLE; battery_cycle_state = 0;
		
		clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2;
		clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5;
	}
	
	if (reg & B_START_DISCHARGE) {battery_mode = B_START_DISCHARGE;}
	if (reg & B_START_CHARGE) { battery_mode = B_START_CHARGE; }
		
	if (reg & B_STOP)
	{
		clr_EN_CHARGE;
		clr_EN_DISCHARGE;
		
		battery_mode = 0;
	}

	app_regs.REG_CONFIG = reg;
	return true;
}


/************************************************************************/
/* REG_DEVICES_CONNECTED                                                */
/************************************************************************/
void app_read_REG_DEVICES_CONNECTED(void)
{
	app_regs.REG_DEVICES_CONNECTED = 0;
	
	if (read_DEVICE_AT_0) app_regs.REG_DEVICES_CONNECTED |= B_OUT0;
	if (read_DEVICE_AT_1) app_regs.REG_DEVICES_CONNECTED |= B_OUT1;
	if (read_DEVICE_AT_2) app_regs.REG_DEVICES_CONNECTED |= B_OUT2;
	if (read_DEVICE_AT_3) app_regs.REG_DEVICES_CONNECTED |= B_OUT3;
	if (read_DEVICE_AT_4) app_regs.REG_DEVICES_CONNECTED |= B_OUT4;
	if (read_DEVICE_AT_5) app_regs.REG_DEVICES_CONNECTED |= B_OUT5;
}

bool app_write_REG_DEVICES_CONNECTED(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DEVICES_CONNECTED = reg;
	return true;
}


/************************************************************************/
/* REG_REPEATER_STATUS                                                  */
/************************************************************************/
void app_read_REG_REPEATER_STATUS(void)
{
	//app_regs.REG_REPEATER_STATUS = 0;

}

bool app_write_REG_REPEATER_STATUS(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_REPEATER_STATUS = reg;
	return true;
}


/************************************************************************/
/* REG_BATTERY_RATE                                                     */
/************************************************************************/
void app_read_REG_BATTERY_RATE(void) {}
bool app_write_REG_BATTERY_RATE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & ~MSK_BATTERY_RATE_CONFIGURATION) return false;

	app_regs.REG_BATTERY_RATE = reg;
	return true;
}


/************************************************************************/
/* REG_BATTERY                                                          */
/************************************************************************/
void app_read_REG_BATTERY(void)
{
	//app_regs.REG_BATTERY = 0;

}

bool app_write_REG_BATTERY(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_BATTERY = reg;
	return true;
}


/************************************************************************/
/* REG_BATTERY_THRESHOLD_LOW                                                   */
/************************************************************************/
void app_read_REG_BATTERY_THRESHOLD_LOW(void)
{
	//app_regs.REG_BATTERY_THRESHOLD_LOW = 0;

}

bool app_write_REG_BATTERY_THRESHOLD_LOW(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_BATTERY_THRESHOLD_LOW = reg;
	return true;
}


/************************************************************************/
/* REG_BATTERY_THRESHOLD_HIGH                                                  */
/************************************************************************/
void app_read_REG_BATTERY_THRESHOLD_HIGH(void)
{
	//app_regs.REG_BATTERY_THRESHOLD_HIGH = 0;

}

bool app_write_REG_BATTERY_THRESHOLD_HIGH(void *a)
{
	float reg = *((float*)a);

	app_regs.REG_BATTERY_THRESHOLD_HIGH = reg;
	return true;
}


/************************************************************************/
/* REG_BATTERY_CALIBRATION0                                             */
/************************************************************************/
void app_read_REG_BATTERY_CALIBRATION0(void)
{
	//app_regs.REG_BATTERY_CALIBRATION0 = 0;

}

bool app_write_REG_BATTERY_CALIBRATION0(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_BATTERY_CALIBRATION0 = reg;
	return true;
}


/************************************************************************/
/* REG_BATTERY_CALIBRATION1                                             */
/************************************************************************/
void app_read_REG_BATTERY_CALIBRATION1(void)
{
	//app_regs.REG_BATTERY_CALIBRATION1 = 0;

}

bool app_write_REG_BATTERY_CALIBRATION1(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_BATTERY_CALIBRATION1 = reg;
	return true;
}


/************************************************************************/
/* ADD_REG_TIMER                                                        */
/************************************************************************/
void app_read_REG_TIMER(void) {}
bool app_write_REG_TIMER(void *a)
{
	uint32_t reg = *((uint32_t*)a);

	app_regs.REG_TIMER = reg;
	return true;
}


/************************************************************************/
/* ADD_REG_TIMER_FREQUENCY                                              */
/************************************************************************/
void app_read_REG_TIMER_FREQUENCY(void) {}
bool app_write_REG_TIMER_FREQUENCY(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_TIMER_RATE))
		return false;
	
	app_regs.REG_TIMER_FREQUENCY = reg;
	return true;
}