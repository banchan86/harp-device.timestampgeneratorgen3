#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

extern AppRegs app_regs;

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{
    /* Configure input pins */
    io_pin2in(&PORTA, 2, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);        // DEVICE_AT_4
    io_pin2in(&PORTA, 3, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);        // DEVICE_AT_5
    io_pin2in(&PORTA, 4, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);        // DEVICE_AT_2
    io_pin2in(&PORTB, 0, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // POWER_GOOD
    io_pin2in(&PORTB, 3, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // BUTTON
    io_pin2in(&PORTC, 6, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);        // DEVICE_AT_0
    io_pin2in(&PORTC, 7, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);        // DEVICE_AT_1
    io_pin2in(&PORTR, 0, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);        // DEVICE_AT_3

    /* Configure input interrupts */
    io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);                 // POWER_GOOD

    /* Configure output pins */
    io_pin2out(&PORTA, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // BAT_READ
    io_pin2out(&PORTA, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_SCREEN_BACKLIGHT
    io_pin2out(&PORTA, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_CLK_4
    io_pin2out(&PORTA, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_CLK_1
    io_pin2out(&PORTB, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_CHARGE
    io_pin2out(&PORTB, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_DISCHARGE
    io_pin2out(&PORTC, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_CLK_0
    io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_CLK_3
    io_pin2out(&PORTC, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_CLK_2
    io_pin2out(&PORTC, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_CLK_5
    io_pin2out(&PORTD, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_POWER
    io_pin2out(&PORTD, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_CLOCK_OUT
    io_pin2out(&PORTD, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN_CLOCK_IN
    io_pin2out(&PORTD, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_REPEATER
    io_pin2out(&PORTD, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_LOCK
    io_pin2out(&PORTD, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // LED_STATE

    /* Initialize output pins */
    clr_BAT_READ;
    clr_EN_SCREEN_BACKLIGHT;
    clr_LED_CLK_4;
    clr_LED_CLK_1;
    clr_EN_CHARGE;
    clr_EN_DISCHARGE;
    clr_LED_CLK_0;
    clr_LED_CLK_3;
    clr_LED_CLK_2;
    clr_LED_CLK_5;
    clr_LED_POWER;
    clr_EN_CLOCK_OUT;
    set_EN_CLOCK_IN;
    clr_LED_REPEATER;
    clr_LED_LOCK;
    clr_LED_STATE;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
    TYPE_U8,
    TYPE_U8,
    TYPE_U8,
    TYPE_U8,
    TYPE_FLOAT,
    TYPE_FLOAT,
    TYPE_FLOAT,
    TYPE_U16,
    TYPE_U16,
    TYPE_U32,
    TYPE_U8
};

uint16_t app_regs_n_elements[] = {
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1
};

uint8_t *app_regs_pointer[] = {
    (uint8_t*)(&app_regs.REG_CONFIG),
    (uint8_t*)(&app_regs.REG_DEVICES_CONNECTED),
    (uint8_t*)(&app_regs.REG_REPEATER_STATUS),
    (uint8_t*)(&app_regs.REG_BATTERY_RATE),
    (uint8_t*)(&app_regs.REG_BATTERY),
    (uint8_t*)(&app_regs.REG_BATTERY_THRESHOLD_LOW),
    (uint8_t*)(&app_regs.REG_BATTERY_THRESHOLD_HIGH),
    (uint8_t*)(&app_regs.REG_BATTERY_CALIBRATION0),
    (uint8_t*)(&app_regs.REG_BATTERY_CALIBRATION1),
    (uint8_t*)(&app_regs.REG_TIMER),
    (uint8_t*)(&app_regs.REG_TIMER_FREQUENCY)
};
