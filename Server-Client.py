# NUSolar Telemetry
# Server

import pyodbc
import socket

DRIVER = '{SQL Server Native Client 11.0}'
SERVER = 'localhost'
DATABASE = 'NUSolarTelemetry'
UID = 'sa'
PWD = 'nusolartelemetry'
HOST = ''                 # Symbolic name meaning all available interfaces
PORT = 50007              # Arbitrary non-privileged port

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
db = pyodbc.connect('DRIVER={};SERVER={};DATABASE={};UID={};PWD={}'.format(DRIVER, SERVER, DATABASE, UID, PWD))

s.bind((HOST, PORT))
s.listen(1)
conn, addr = s.accept()
print('Connected by', addr)
while True:
    data = conn.recv(1024)
    if not data:
        break
    conn.sendall(data)