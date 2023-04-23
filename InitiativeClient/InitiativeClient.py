import time
from machine import *
from simple import MQTTClient
import esp
import network
import socket
import json

import ubinascii

freq(80000000)
print(freq())

P2 = Pin(2, Pin.OUT)

WLAN = network.WLAN(network.STA_IF)  # create station interface
WLAN.active(True)
WLAN.connect('lishiyuan', '761834925')


def WaitForConnection():
    while not WLAN.isconnected():
        P2.on()
        time.sleep(0.15)
        P2.off()
        time.sleep(0.15)


WaitForConnection()
print(WLAN.ifconfig()[0])


def MqttCallBack(t: bytearray, m: bytearray):
    print("Gotten")
    if t.decode('utf-8') == "Sys/ServiceMsg":
        print(m.decode('utf-8'))


while True:
    try:
        client = MQTTClient("my_client_id", "59.110.225.239", port=34420, user="root", password="07211145141919")
        client.connect()
        client.set_callback(MqttCallBack)
        client.publish(b"Sys/ServiceMsg", b"Hre")
        client.subscribe(b"Sys/ServiceMsg")

        while True:
            client.check_msg()
            if not WLAN.isconnected():
                break
    except Exception as e:
        print(e)
        continue

    if not WLAN.isconnected():
        WaitForConnection()
        print(WLAN.ifconfig()[0])
