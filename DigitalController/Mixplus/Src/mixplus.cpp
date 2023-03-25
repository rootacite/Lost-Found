

#include "mixplus.h"
#include "GPIO.hpp"
#include "Timer.hpp"
#include "PWM.hpp"
#include "Serial.hpp"

extern TIM_HandleTypeDef htim2;
extern UART_HandleTypeDef huart1;

GPIO PB12(GPIOB, GPIO_PIN_12);
GPIO PC13(GPIOC, GPIO_PIN_13);

Serial Serial1(&huart1);

void setup()
{
	htim2.Instance->ARR = 1000;
	int p = (72 * 1000000) / 50000;
	htim2.Instance->PSC = p - 1;
	__HAL_TIM_SET_COUNTER(&htim2, htim2.Instance->ARR);
	HAL_TIM_PWM_Start(&htim2, TIM_CHANNEL_1);

	Serial1.begin();
}


void loop()
{
	if(PB12.get())
	{
		__HAL_TIM_SET_COMPARE(&htim2, TIM_CHANNEL_1, 34);
		PC13.set(0);
	}
	else
	{
		__HAL_TIM_SET_COMPARE(&htim2, TIM_CHANNEL_1, 85);
		PC13.set(1);
	}

	Timer::delay(10);
}
