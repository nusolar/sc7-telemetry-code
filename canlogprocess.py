import pandas as pd
import sys

#Used to parse a filtered CSV containing only 403 and 402 ID packets
#and display the vehicle velocity side by side next to the bus current

#sys.argv[0] is the name of this python script
#sys.argv[1] is the name of the csv to be processed, this assumes
#that the csv is in C:/User/Name/Documents

#change your directory to C:/User/Name/Documents and make sure 
#the csv to be processed is in this directory.

#Coyp this into your command prompt:  python canlogprocess.py name.csv

name = sys.argv[1]
data = pd.read_csv(name)

eyedees = data["ID"]
values = data["float[1]"]
valuesindex = 0
final = [] #list of lists that will be put back into a csv
temp = [0,0] #2 item list containing the velocity, current pair
start = False #indicator of whether we've found the first 403-ID packet
partner = False #indicator of the whole every other row is a 403 ID

for address in eyedees:
    stradd = str(address)
    #print(address)
    if (stradd == " 0x403" and start == False): #found the first 403!
        start = True
        temp[0] = values[valuesindex] * 2.23694 #convert from metres/second to mph 
        partner = True
        valuesindex += 1
    elif (stradd == " 0x403" and start == True): #every other 403
        temp[0] = values[valuesindex] * 2.23694
        partner = True
        valuesindex += 1
    elif (stradd == " 0x402" and start == True and partner == True):  #so we've started logging the data
        temp[1] = values[valuesindex] #record the current (in amps)
        partner = False   #reset the partner bool (since next packet will be 403)
        valuesindex += 1  
        final.append(temp)  #append the data pair to final list
        temp = [0,0]   #reset
    else:
        temp = [0,0]
        #print("Messerschmitt")

df = pd.DataFrame(final, columns=["velocity", "bus current"])
df.to_csv('filtered.csv', index=False)