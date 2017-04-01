import _winreg as winreg
import serial
import time
import struct

from Tkinter import *
import threading



class ServoControl(object):
    def __init__(self,root):
        self.root = root
        
        self.InitWidgets()
        self.CenterWidget()
        self.InitSerial('COM7', 9200)

    def OnSlide(self,event):
        self.position = self.scale.get()
        self.label['text']='Motor at '+str(self.position)+' degrees'
        self.WriteToSerial(self.position) 
        
    def InitWidgets(self):
        self.frame = Frame(self.root)
        self.frame.pack(side=LEFT, expand=1, fill=BOTH, anchor=CENTER)
        self.clabel = Label(self.frame, text='Using serial port: UNKNOWN')
        self.clabel.pack(side=TOP, expand=1, fill=X, anchor=CENTER)
        self.slabel = Label(self.frame, text='Status: UNKNOWN')
        self.slabel.pack(side=TOP, expand=1, fill=X, anchor=CENTER)
        self.label = Label(self.frame, text='Motor at 0 degrees')
        self.label.pack(side=TOP, expand=1, fill=X, anchor=CENTER)
        self.scale = Scale(self.frame, from_=-90, to=90, command=self.OnSlide, orient=HORIZONTAL)
        self.scale.pack(side=TOP, expand=1, fill=X, anchor=CENTER)
        return
    
    def CenterWidget(self):
        w = self.root.winfo_screenwidth()
        h = self.root.winfo_screenheight()
        #rootsize = tuple(int(_) for _ in self.root.geometry().split('+')[0].split('x'))
        rootsize = (250,100)
        x = w/2 - rootsize[0]/2
        y = h/2 - rootsize[1]/2
        self.root.geometry("%dx%d+%d+%d" % (rootsize + (x, y)))

    
    def VerifySerial(self):
        self.comports = []
        self.compath = 'HARDWARE\\DEVICEMAP\\SERIALCOMM'
        key = winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, self.compath)
        for i in range(10):
            try:
                port = winreg.EnumValue(key,i)[1]
                
                try:
                    serial.Serial(port) # test open
                except serial.serialutil.SerialException:
                    print port," can't be openend"
                else:
                    print port," Ready"
                    self.comports.append(port)
                
            except EnvironmentError:
                break
        return
    
    def InitSerial(self, comport,baudrate):
        self.arduino = serial.Serial(port=comport,                 
                                baudrate=baudrate,
                                bytesize=serial.EIGHTBITS,
                                parity=serial.PARITY_NONE,
                                stopbits=serial.STOPBITS_ONE,
                                timeout=0.1,
                                xonxoff=0,
                                rtscts=0,
                                interCharTimeout=None)
        self.clabel['text']='Using serial port: '+ comport
        self.slabel['text']='Status: Ready'
        try:
            self.arduino.open()
        except Exception:
            if not self.arduino.isOpen(): self.slabel['text']='Status: Failed to open'
        
    
    def WriteToSerial(self,position):
        if not self.arduino.isOpen(): 
            self.arduino.open()
            time.sleep(0.1)
        self.slabel['text']='Status: Writing...'
        
        position = 90 + position # Servo range 0-180;
        
        self.arduino.write(str(position).rjust(3,'0'))
        time.sleep(0.05)
        self.slabel['text']='Status: Open'
        
    def CloseSerial(self):
        self.arduino.close()




def main():
    root = Tk()
    
    application = ServoControl(root)
    root.title('ServoControl')
    root.mainloop()
    
if __name__ == '__main__':
    main()

