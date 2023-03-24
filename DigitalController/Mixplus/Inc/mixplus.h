
#ifndef __H_MIXPLUS
#define __H_MIXPLUS


#include "main.h"

#ifdef __cplusplus
extern "C" {
#endif /* __cplusplus */

#define RGB(R,G,B)	(((R >> 3) << 11) | ((G >> 2) << 5) | (B >> 3))	/* 将8位R,G,B转化为 16位RGB565格式 */
#define RGB565_R(x)  ((x >> 8) & 0xF8)
#define RGB565_G(x)  ((x >> 3) & 0xFC)
#define RGB565_B(x)  ((x << 3) & 0xF8)

	void setup();
	void loop();
#ifdef __cplusplus
}
#endif /* __cplusplus */

#endif
