import socket

soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM);
soc.connect(('59.110.225.239', 34420));

soc.send(b's1');
dt = soc.recv(8).decode('utf-8');
print(dt)
