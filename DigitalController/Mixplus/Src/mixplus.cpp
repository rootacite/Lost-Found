

#include "mixplus.h"
#include "GPIO.hpp"
#include "Timer.hpp"
#include "PWM.hpp"

extern TIM_HandleTypeDef htim2;

GPIO PB12(GPIOB,GPIO_PIN_12);
PWM TIM2CH1(&htim2,100);

void setup()
{
	TIM2CH1.freq(72, 50);
	TIM2CH1.begin(0);
}


void loop()
{
	if(PB12.get())
	{
		TIM2CH1.pulse(0, 0.025);
	}
	else
	{
		TIM2CH1.pulse(0, 0.125);
	}
}
