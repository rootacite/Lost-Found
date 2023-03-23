
from machine import *
import time
import esp

import network
import socket

import json

def WaitForConnection():
    while not WLAN.isconnected():
        P2.on();
        time.sleep(0.15);
        P2.off();
        time.sleep(0.15);

freq(160000000)
print(freq());

WLAN = network.WLAN(network.STA_IF); # create station interface
WLAN.active(True);
WLAN.connect('lishiyuan', '761834925');

P2 = Pin(2,Pin.OUT);

WaitForConnection();
print(WLAN.ifconfig()[0]);
    
while True:
    soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM);
    soc.connect(('59.110.225.239', 34420));
    counter = 0;
    while True:
        soc.send(b'g');
        dt = soc.recv(8).decode('utf-8')
        if not dt:
            break;
        print(dt + str(counter))
        if dt == 'False':
            P2.on();
        else:
            P2.off();
        counter+=1;
        time.sleep(0.5)
    
    if not WLAN.isconnected():
        WaitForConnection();
        print(WLAN.ifconfig()[0]);

