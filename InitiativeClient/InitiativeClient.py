
import socket
import time

soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

soc.connect(('127.0.0.1', 34420))

while True:
    soc.send(b'g');
    print(soc.recv(1024).decode('utf-8'))
    time.sleep(0.5)

soc.close()