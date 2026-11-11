#include "uart0_ctrl.h"

static uint8_t saved_ctrla;
static uint8_t saved_ctrlb;
static bool uart0_suspended = false;

void uart0_disable_and_tx_to_low(void)
{
    if (!uart0_suspended)
    {
        saved_ctrla    = USARTE0.CTRLA;
        saved_ctrlb    = USARTE0.CTRLB;

        USARTE0.CTRLA = 0; /* Disable USART interrupts. */
        USARTE0.CTRLB = saved_ctrlb &
            (uint8_t)~(USART_RXEN_bm | USART_TXEN_bm);
      
        // Configure TX PE3 to logic 0
		io_pin2out(&PORTE, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);
		clear_io(PORTE, 3);
		
		// Configure RTS PE1 to input and logic 0 (logic 0 is not necessary but I will keep it because it was here before)
		io_pin2in(&PORTE, 1, PULL_IO_DOWN, SENSE_IO_NO_INT_USED);
		clear_io(PORTE, 1);
		
        uart0_suspended = true;
    }
}

void uart0_restore(void)
{
    if (uart0_suspended)
    {   
		// Configure RTS PE1 to output again
		io_pin2out(&PORTE, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);
		
        USARTE0.CTRLB = saved_ctrlb; /* Restore RX/TX and CLK2X. */
        USARTE0.CTRLA = saved_ctrla; /* Restore interrupts last. */

        uart0_suspended = false;
    }
}