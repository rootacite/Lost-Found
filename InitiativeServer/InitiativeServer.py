
import socket
import threading
import time
 

soc = socket.socket(socket.AF_INET, socket.SOCK_STREAM)


soc.bind(('0.0.0.0', 34420))
soc.listen(5)

status = False;
 
def Tcplink(sock, addr):
    print('Accept new connection form %s:%s...' % addr)
    
    global status
    while True:
        try:
            dt = sock.recv(8).decode('utf-8');
            if not dt:
                break;
            if dt[0] == 'g':
                sock.send(str(status).encode('utf-8'));
                continue;

            if dt[0] == 's':
                if dt[1] == '0':
                    status = False;
                else:
                    status = True;
                sock.send(str(status).encode('utf-8'));
                continue;
        except:
            break;
        
 
 
while True:
    sock, addr = soc.accept()
 
    t = threading.Thread(target=Tcplink(sock, addr), args=(sock, addr))
    t.start()