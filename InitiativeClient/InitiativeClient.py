
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
P15 = Pin(15, Pin.OUT);

WaitForConnection();
print(WLAN.ifconfig()[0]);
    
while True:
    try:
        soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM);
        soc.connect(('59.110.225.239', 34421));
        
        s_data = json.dumps({"Command" : 0, "Name" : "Lock_1", "Payload" : "NC" });
        s_data += "\n";
        print(s_data)
        soc.send(s_data.encode('utf-8'));
        dt = soc.recv(512).decode('utf-8')
    
        if not dt:
            soc.close();
            time.sleep(1);
            continue;0
    
        r_data = json.loads(dt)
        print(r_data)
        
        if r_data['Payload'][0] == '1':
            P2.on();
            P15.on();
        else:
            P2.off();
            P15.off();
    
        soc.close();
        time.sleep(1);
    except Exception as e:
        print(e);
        continue;
    
    if not WLAN.isconnected():
        WaitForConnection();
        print(WLAN.ifconfig()[0]);



