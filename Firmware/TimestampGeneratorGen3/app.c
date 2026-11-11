#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#define F_CPU 32000000
#include <util/delay.h>

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);

/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "TimestampGeneratorGen3";

void hwbp_app_initialize(void)
{
    /* Define versions */
    uint8_t hwH = 1;
    uint8_t hwL = 2;
    uint8_t fwH = 1;
    uint8_t fwL = 3;
    uint8_t ass = 0;
    
   	/* Start core */
    core_func_start_core(
        1158,
        hwH, hwL,
        fwH, fwL,
        ass,
        (uint8_t*)(&app_regs),
        APP_NBYTES_OF_REG_BANK,
        APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
        default_device_name,
		true,	// The device is able to repeat the harp timestamp clock
		true,	// The device is able to generate the harp timestamp clock
		3		// Default timestamp offset
    );
} 

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2; clr_LED_LOCK;     clr_LED_POWER;
	clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5; clr_LED_REPEATER; clr_LED_STATE;
	clr_EN_CHARGE;
	clr_EN_DISCHARGE;
}

/************************************************************************/
/* User functions                                                       */
/************************************************************************/
/* Add your functions here or load external functions if needed */

/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
#define T_STARTUP_ON  50
#define T_STARTUP_OFF 0

void core_callback_define_clock_default(void)
{	
	/* By default, the device will be used as a clock generator */
	core_device_to_clock_generator();
}

void core_callback_initialize_hardware(void)
{
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
	
	_delay_ms(1000);
	
	/* Initialize hardware */
	for (uint8_t i = 0; i < 2; i++)
	{
		set_LED_CLK_0;  _delay_ms(T_STARTUP_ON); clr_LED_CLK_0;  _delay_ms(T_STARTUP_OFF);
		set_LED_CLK_1;  _delay_ms(T_STARTUP_ON); clr_LED_CLK_1;  _delay_ms(T_STARTUP_OFF);
		set_LED_CLK_2;  _delay_ms(T_STARTUP_ON); clr_LED_CLK_2;  _delay_ms(T_STARTUP_OFF);
		set_LED_LOCK;   _delay_ms(T_STARTUP_ON); clr_LED_LOCK;  _delay_ms(T_STARTUP_OFF);
		set_LED_POWER;  _delay_ms(T_STARTUP_ON); clr_LED_POWER;  _delay_ms(T_STARTUP_OFF);
		set_LED_CLK_3;  _delay_ms(T_STARTUP_ON); clr_LED_CLK_3;  _delay_ms(T_STARTUP_OFF);
		set_LED_CLK_4;  _delay_ms(T_STARTUP_ON); clr_LED_CLK_4;  _delay_ms(T_STARTUP_OFF);
		set_LED_CLK_5;  _delay_ms(T_STARTUP_ON); clr_LED_CLK_5;  _delay_ms(T_STARTUP_OFF);
		set_LED_REPEATER; _delay_ms(T_STARTUP_ON); clr_LED_REPEATER;  _delay_ms(T_STARTUP_OFF);
		set_LED_STATE;  _delay_ms(T_STARTUP_ON); clr_LED_STATE;  _delay_ms(T_STARTUP_OFF);
	}
	_delay_ms(T_STARTUP_ON*2);

	for (uint8_t i = 0; i < 2; i++)
	{
		set_LED_CLK_0; set_LED_CLK_1; set_LED_CLK_2; set_LED_LOCK; set_LED_POWER;
		set_LED_CLK_3; set_LED_CLK_4; set_LED_CLK_5; set_LED_REPEATER; set_LED_STATE;
		_delay_ms(T_STARTUP_ON*2);
		
		clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2; clr_LED_LOCK; clr_LED_POWER;
		clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5; clr_LED_REPEATER; clr_LED_STATE;
		_delay_ms(T_STARTUP_ON*2);
	}
	
	_delay_ms(500);
	
	/* Initialize ADC */
	adc_A_initialize_single_ended(REF_ADC_1V);
	
	/* Check battery charge */
	set_BAT_READ;
	_delay_us(1000);
	app_regs.REG_BATTERY = adc_A_read_channel(0) / 4096.0 / (14.7/(14.7+51.0));
	clr_BAT_READ;
	
	if (app_regs.REG_BATTERY > 0.5 && app_regs.REG_BATTERY < 3.3)
	{
		set_LED_STATE; _delay_ms(100);
		for (uint8_t i = 0; i < 20; i++)
		{
			tgl_LED_STATE; _delay_ms(100);
		}
	}
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	app_regs.REG_BATTERY_RATE = MSK_EACH_60SECONDS;
	
	app_regs.REG_BATTERY_TH_HIGH = 3.75;
	app_regs.REG_BATTERY_TH_LOW = 3.65;
}

void core_callback_registers_were_reinitialized(void)
{
	/* Update registers if needed */
	app_regs.REG_CONFIG = B_STOPS_ANY;
	app_write_REG_CONFIG(&app_regs.REG_CONFIG);
}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	set_EN_SCREEN_BACKLIGHT;
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	clr_EN_SCREEN_BACKLIGHT;
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void) {}
void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
bool read_battery = false;

uint16_t counter = 2000;
uint8_t battery_counter = 0;
uint16_t timer_counter = 0;

#define BATTERY_BUFFER 16
#define BATTERY_DIV 4
uint16_t battery[BATTERY_BUFFER];
uint8_t battery_index = 0;
bool battery_ready = false;

#define BUTTON_DEBOUNCE_MS 20

#define BUTTON_USER_STANDBY 0
#define BUTTON_USER_BAT_LEVEL 1
#define BUTTON_USER_LOCK_UNLOCK 2
#define BUTTON_USER_GEN_REPEATER 3
#define BUTTON_USER_TURN_OFF 4
#define BUTTON_USER_START_BATTERY_CYCLE 5

#define BUTTON_USER_BAT_LEVEL_MS 500
#define BUTTON_USER_LOCK_UNLOCK_MS 2500
#define BUTTON_USER_GEN_REPEATER_MS 5500
#define BUTTON_USER_START_BATTERY_CYCLE_MS 15000

uint16_t button_pressed_ms = 0;
uint8_t button_blink_counter = 0;
#define BUTTON_USER_INDICATION_BLINK_MS 100

uint16_t battery_indication_ms = 0;
#define BATTERY_INDICATION_PERIOD 5000
#define BATTERY_INDICATION_BLINK_MS 200

#define BATTERY_CHARGED_TH 4.1
#define BATTERY_DISCHARGED_TH 3.35
#define BATTERY_DISCHARGED_DEPLETED 3.25

extern uint8_t battery_mode;
extern uint8_t battery_cycle_state;
void batery_state_machine(void);

void core_callback_t_before_exec(void)
{
	timer_counter++;
	
	if ((app_regs.REG_TIMER_FREQUENCY == MSK_TIMER_1000HZ && ((timer_counter & 1) == 0)) ||
		(app_regs.REG_TIMER_FREQUENCY == MSK_TIMER_500HZ && ((timer_counter & 3) == 0)) ||
		(app_regs.REG_TIMER_FREQUENCY == MSK_TIMER_200HZ && ((timer_counter % 10) == 0)) ||
		(app_regs.REG_TIMER_FREQUENCY == MSK_TIMER_100HZ && ((timer_counter % 20) == 0)) ||
		(app_regs.REG_TIMER_FREQUENCY == MSK_TIMER_50HZ && ((timer_counter % 40) == 0)))
	{
		core_func_send_event(ADD_REG_TIMER, true);
		app_regs.REG_TIMER++;
	}
}
void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void)
{	
	if (counter >= 4000)
		counter = 0;
	
	set_BAT_READ;
	read_battery = true;
	
	timer_counter = 0;
}
void core_callback_t_500us(void) {}

void update_LED_LOCK(void)
{
	if (core_bool_clock_is_locked())
		set_LED_LOCK;
	else
		clr_LED_LOCK;
}

void update_LED_REPEATER(void)
{
	if (core_bool_device_is_repeater())
		set_LED_REPEATER;
	else
		clr_LED_REPEATER;
}

void update_LED_POWER(void)
{
	if (!read_POWER_GOOD)
		set_LED_POWER;
	else
		clr_LED_POWER;
}

void turn_device_off(void)
{
	clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2; clr_LED_LOCK; clr_LED_POWER;
	clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5; clr_LED_REPEATER; clear_io(PORTD, 5);
	
	clr_EN_CHARGE;
	clr_EN_DISCHARGE;
	clr_BAT_READ;
	
	clr_EN_SCREEN_BACKLIGHT;
	
	for (uint8_t j = 0; j < 20; j++)
	{
		_delay_ms (100);
		//toggle_io(PORTD, 5);
		tgl_LED_POWER;
	}
	
	SLEEP_CTRL = SLEEP_SMODE_PDOWN_gc | SLEEP_SEN_bm;
	__asm volatile("sleep");
	
	wdt_reset_device();
}

bool is_first_time_at_battery_cycle_user_indication;

void core_callback_t_1ms(void)
{	
	/* When button is being pressed */
	if (!read_BUTTON)
	{
		button_pressed_ms++;
		
		if (++button_blink_counter == BUTTON_USER_INDICATION_BLINK_MS)
		{			
			button_blink_counter = 0;
			
			if (button_pressed_ms < BUTTON_USER_LOCK_UNLOCK_MS)
			{
			}
			
			if (button_pressed_ms >= BUTTON_USER_LOCK_UNLOCK_MS && button_pressed_ms < BUTTON_USER_GEN_REPEATER_MS)
			{
				tgl_LED_LOCK;
			}
			
			if (button_pressed_ms >= BUTTON_USER_GEN_REPEATER_MS && button_pressed_ms < BUTTON_USER_START_BATTERY_CYCLE_MS)
			{
				
				if (core_bool_is_visual_enabled())					
					update_LED_LOCK();
				else
					clr_LED_LOCK;
				
				tgl_LED_REPEATER;
				is_first_time_at_battery_cycle_user_indication = true;
			}
						
			if (button_pressed_ms >= BUTTON_USER_START_BATTERY_CYCLE_MS)
			{
				if (core_bool_is_visual_enabled())
					update_LED_REPEATER();
				else
					clr_LED_POWER;
				
				if(!read_POWER_GOOD)
				{
					if(is_first_time_at_battery_cycle_user_indication)
					{
						is_first_time_at_battery_cycle_user_indication = false;
						clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2;
						clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5;
					}
					
					tgl_LED_CLK_0; tgl_LED_CLK_1; tgl_LED_CLK_2;
					tgl_LED_CLK_3; tgl_LED_CLK_4; tgl_LED_CLK_5;
				}
			}			
		}
	}
	
	/* While button is not pressed */
	else
	{	
		if (button_pressed_ms > BUTTON_DEBOUNCE_MS)
		{
			/* If the button is released, check what command to execute */
			
			if (button_pressed_ms < BUTTON_USER_LOCK_UNLOCK_MS)
			{
				clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2;
				clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5;				
				
				battery_indication_ms = BATTERY_INDICATION_PERIOD;
			}
			
			if (button_pressed_ms >= BUTTON_USER_LOCK_UNLOCK_MS && button_pressed_ms < BUTTON_USER_GEN_REPEATER_MS)
			{
				if (core_bool_clock_is_locked())
					core_clock_to_unlock();
				else
					core_clock_to_lock();
			}
			
			if (button_pressed_ms >= BUTTON_USER_GEN_REPEATER_MS && button_pressed_ms < BUTTON_USER_START_BATTERY_CYCLE_MS)
			{
				if (core_bool_device_is_repeater())
					core_device_to_clock_generator();
				else
					core_device_to_clock_repeater();
				
				core_save_all_registers_to_eeprom();
			}
			
			if (button_pressed_ms >= BUTTON_USER_START_BATTERY_CYCLE_MS)
			{
				// Toggle battery cycle
				if (battery_mode & B_START_BATTERY_CYCLE)
					battery_mode = 0;
				else
					battery_mode = B_START_BATTERY_CYCLE;
			}
		}
	
		button_pressed_ms = 0;
		
		/* Indicate the battery current level */
		if (battery_indication_ms)
		{			
			battery_indication_ms--;
			
			if ((battery_indication_ms % BATTERY_INDICATION_BLINK_MS) == 0)
			{
				if ( app_regs.REG_BATTERY < 3.55)
				{
					set_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2;
				}
				else if ( app_regs.REG_BATTERY < 3.60)
				{
					set_LED_CLK_0; set_LED_CLK_1; clr_LED_CLK_2;
				}
				else
				{
					set_LED_CLK_0; set_LED_CLK_1; set_LED_CLK_2;
				}
			}
			if ((battery_indication_ms % (BATTERY_INDICATION_BLINK_MS * 2)) == 0)
			{
				clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2;
			}
		}
		
		/* If not showing battery level, show all the others */
		else 
		{
			if (!core_bool_is_visual_enabled())
			{
				clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2; clr_LED_LOCK; clr_LED_POWER;
				clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5; clr_LED_REPEATER;
			}
			else
			{			
				if (!read_POWER_GOOD && ((counter%50) == 0) && ((battery_mode&B_START_BATTERY_CYCLE)==0))
				{
					if (read_DEVICE_AT_0) set_LED_CLK_0; else clr_LED_CLK_0;
					if (read_DEVICE_AT_1) set_LED_CLK_1; else clr_LED_CLK_1;
					if (read_DEVICE_AT_2) set_LED_CLK_2; else clr_LED_CLK_2;
					if (read_DEVICE_AT_3) set_LED_CLK_3; else clr_LED_CLK_3;
					if (read_DEVICE_AT_4) set_LED_CLK_4; else clr_LED_CLK_4;
					if (read_DEVICE_AT_5) set_LED_CLK_5; else clr_LED_CLK_5;
					update_LED_LOCK();
					update_LED_REPEATER();
					set_LED_POWER;
				}
				
				if (!read_POWER_GOOD && ((counter%500) == 0) && (battery_mode&B_START_BATTERY_CYCLE))
				{
					set_LED_CLK_0; set_LED_CLK_1; set_LED_CLK_2;
					clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5;
					update_LED_LOCK();
					update_LED_REPEATER();
					set_LED_POWER;
				}
				
				if (!read_POWER_GOOD && ((counter%1000) == 0) && (battery_mode&B_START_BATTERY_CYCLE))
				{
					clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2;
					set_LED_CLK_3; set_LED_CLK_4; set_LED_CLK_5;
					update_LED_LOCK();
					update_LED_REPEATER();
					set_LED_POWER;
				}
			
				if (read_POWER_GOOD && (counter == 3000))
				{
					if (read_DEVICE_AT_0) set_LED_CLK_0; else clr_LED_CLK_0;
					if (read_DEVICE_AT_1) set_LED_CLK_1; else clr_LED_CLK_1;
					if (read_DEVICE_AT_2) set_LED_CLK_2; else clr_LED_CLK_2;
					if (read_DEVICE_AT_3) set_LED_CLK_3; else clr_LED_CLK_3;
					if (read_DEVICE_AT_4) set_LED_CLK_4; else clr_LED_CLK_4;
					if (read_DEVICE_AT_5) set_LED_CLK_5; else clr_LED_CLK_5;
					update_LED_LOCK();
					update_LED_REPEATER();
					clr_LED_POWER;
				}
				
				if (read_POWER_GOOD && ((counter%305) == 0))
				{
					clr_LED_CLK_0; clr_LED_CLK_1; clr_LED_CLK_2; clr_LED_LOCK; clr_LED_POWER;
					clr_LED_CLK_3; clr_LED_CLK_4; clr_LED_CLK_5; clr_LED_REPEATER;
				}
			}
		}
	}
	
	if (read_battery)
	{
		read_battery = false;
		
		int16_t adc_read = adc_A_read_channel(0);		
		clr_BAT_READ;
		
		if (adc_read <= 100) // If battery is not available
		{
			for (uint8_t i = 0; i < BATTERY_BUFFER; i++)
			{
				battery[i] = 0;
			}
			battery_index = 0;
			battery_ready = false;
			clr_EN_CHARGE;
			clr_EN_DISCHARGE;
			battery_mode = 0;
			app_regs.REG_BATTERY=0;
		}
		else
		{
			battery[battery_index++] = adc_read;
		}
		
		if (battery_index == BATTERY_BUFFER)
		{
			battery_index = 0;
			battery_ready = true;
		}
		
		if (battery_ready)
		{
			uint32_t battery_sum = 0;
			for (uint8_t i = 0; i < BATTERY_BUFFER; i++)
			{
				battery_sum += battery[i];
			}
		
			app_regs.REG_BATTERY = ((uint16_t)(battery_sum >> BATTERY_DIV)) / 4096.0 / (14.7/(14.7+51.0));
			
			batery_state_machine();
		}
		else
		{	
			battery_mode = 0;
					
			clr_EN_CHARGE;
			clr_EN_DISCHARGE;
		}

		switch (app_regs.REG_BATTERY_RATE)
		{
			case MSK_EACH_SECOND:
				battery_counter = 0;
				if (battery_ready)
					core_func_send_event(ADD_REG_BATTERY, true);
				break;
			case MSK_EACH_10SECONDS:
				if (++battery_counter == 10)
				{
					battery_counter = 0;
					if (battery_ready)
						core_func_send_event(ADD_REG_BATTERY, true);
				}
				break;
			case MSK_EACH_60SECONDS:
				if (++battery_counter == 60)
				{
					battery_counter = 0;
					if (battery_ready)
						core_func_send_event(ADD_REG_BATTERY, true);
				}
				break;
			default:
				battery_counter = 0;
		}
	}

	counter++;
}

void batery_state_machine(void)
{	
	/* If power is not plugged, cancel any battery mode */
	if (read_POWER_GOOD)
	{
		battery_mode = 0;
		
		clr_EN_CHARGE;
		clr_EN_DISCHARGE;
		
		if (app_regs.REG_BATTERY <= BATTERY_DISCHARGED_DEPLETED)
			turn_device_off();
		
		return;
	}
	
	/* Typical behavior */
	if (battery_mode == 0)
	{	
		if (app_regs.REG_BATTERY <= app_regs.REG_BATTERY_TH_LOW || app_regs.REG_BATTERY <= BATTERY_DISCHARGED_TH)
		{
			set_EN_CHARGE;
			clr_EN_DISCHARGE;			
		}
		if (app_regs.REG_BATTERY >= app_regs.REG_BATTERY_TH_HIGH || app_regs.REG_BATTERY >= BATTERY_CHARGED_TH)
		{
			clr_EN_CHARGE;
			clr_EN_DISCHARGE;
		}
	}
	
	if (battery_mode == B_START_BATTERY_CYCLE)
	{	
		if (battery_cycle_state == 0)
		{
			clr_EN_CHARGE;
			set_EN_DISCHARGE;
			
			if (app_regs.REG_BATTERY <= BATTERY_DISCHARGED_TH)
			{
				battery_cycle_state = 1;				
				set_EN_CHARGE;
				clr_EN_DISCHARGE;
			}
		}
		
		if (battery_cycle_state == 1)
		{
			if (app_regs.REG_BATTERY >= BATTERY_CHARGED_TH)
			{
				battery_cycle_state = 2;				
				clr_EN_CHARGE;
				set_EN_DISCHARGE;
			}
		}
		
		if (battery_cycle_state == 2)
		{
			if (app_regs.REG_BATTERY <= app_regs.REG_BATTERY_TH_HIGH)
			{
				battery_mode = 0;				
				clr_EN_CHARGE;
				clr_EN_DISCHARGE;
			}
		}
	}
	
	if (battery_mode == B_START_DISCHARGE)
	{		
		clr_EN_CHARGE;
		set_EN_DISCHARGE;
		
		if (app_regs.REG_BATTERY <= BATTERY_DISCHARGED_TH)
		{
			battery_mode = 0;
			clr_EN_CHARGE;
			clr_EN_DISCHARGE;
		}
	}
	
	if (battery_mode == B_START_CHARGE)
	{
		set_EN_CHARGE;
		clr_EN_DISCHARGE;
		
		if (app_regs.REG_BATTERY >= BATTERY_CHARGED_TH)
		{
			battery_mode = 0;
			clr_EN_CHARGE;
			clr_EN_DISCHARGE;
		}
	}	
}

/************************************************************************/
/* Callbacks: cloc control                                              */
/************************************************************************/
void core_callback_clock_to_repeater(void)
{
	clr_EN_CLOCK_OUT;
	_delay_us(10);
	set_EN_CLOCK_IN;
}
void core_callback_clock_to_generator(void)
{
	clr_EN_CLOCK_IN;
	_delay_us(10);
	set_EN_CLOCK_OUT;
}
void core_callback_clock_to_unlock(void) {}
void core_callback_clock_to_lock(void) {}

/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}