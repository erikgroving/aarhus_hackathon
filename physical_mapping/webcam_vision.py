# CannyWebcam.py
import argparse

import cv2
import numpy as np
import os
import cv2.cv as cv
from OSC import OSCClient, OSCMessage, OSCServer
import _winreg as winreg
import serial
import time


class ServoControl(object):
    def __init__(self):
        self.InitSerial('COM4', 9200)

    def VerifySerial(self):
        self.comports = []
        self.compath = 'HARDWARE\\DEVICEMAP\\SERIALCOMM'
        key = winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, self.compath)
        for i in range(10):
            try:
                port = winreg.EnumValue(key, i)[1]

                try:
                    serial.Serial(port)  # test open
                except serial.serialutil.SerialException:
                    print port, " can't be openend"
                else:
                    print port, " Ready"
                    self.comports.append(port)

            except EnvironmentError:
                break
        return

    def InitSerial(self, comport, baudrate):
        self.arduino = serial.Serial(port=comport,
                                     baudrate=baudrate,
                                     bytesize=serial.EIGHTBITS,
                                     parity=serial.PARITY_NONE,
                                     stopbits=serial.STOPBITS_ONE,
                                     timeout=0.1,
                                     xonxoff=0,
                                     rtscts=0,
                                     interCharTimeout=None)
        try:
            self.arduino.open()
        except Exception:
            if not self.arduino.isOpen(): print("Not Open")

    def WriteToSerial(self, position):
        if not self.arduino.isOpen():
            self.arduino.open()
            time.sleep(0.1)
        print("Writing")

        position = 90 + position  # Servo range 0-180;

        self.arduino.write(str(position).rjust(3, '0'))
        time.sleep(0.05)
        print('Status: Open')

    def CloseSerial(self):
        self.arduino.close()

#import myo as libmyo; libmyo.init()
#import time
#import sys
'''
class Listener(libmyo.DeviceListener):
    """
    Listener implementation. Return False from any function to
    stop the Hub.
    """

    interval = 0.05  # Output only 0.05 seconds

    def __init__(self, servocontrol):
        super(Listener, self).__init__()
        self.orientation = None
        self.pose = libmyo.Pose.rest
        self.emg_enabled = False
        self.locked = False
        self.rssi = None
        self.emg = None
        self.last_time = 0
        self.current_orientation = None
        self.orientation_change = False
        self.servo_control = servocontrol

    def on_connect(self, myo, timestamp, firmware_version):
        myo.vibrate('short')
        myo.vibrate('short')
        myo.request_rssi()
        myo.request_battery_level()

    def on_rssi(self, myo, timestamp, rssi):
        self.rssi = rssi

    def on_pose(self, myo, timestamp, pose):
        if pose == libmyo.Pose.fist:
            self.emg_enabled = True
        elif pose == libmyo.Pose.fingers_spread:
            myo.set_stream_emg(libmyo.StreamEmg.disabled)
            self.emg_enabled = False
            self.emg = None
        self.pose = pose

    def on_orientation_data(self, myo, timestamp, orientation):
        self.current_orientation = orientation
        if self.orientation != self.current_orientation:
            self.orientation_change = True
        if self.orientation_change == True:
            change_x = self.orientation.x - self.current_orientation.x
            change_y = self.orientation.y - self.current_orientation.y
            if change_x < 0:
                self.servo_control.WriteToSerial(89)
            elif change_x > 0:
                self.servo_control.WriteToSerial(-89)
        self.current_orientation = self.orientation


    def on_accelerometor_data(self, myo, timestamp, acceleration):
        pass

    def on_gyroscope_data(self, myo, timestamp, gyroscope):
        pass

    def on_emg_data(self, myo, timestamp, emg):
        self.emg = emg

    def on_unlock(self, myo, timestamp):
        self.locked = False

    def on_lock(self, myo, timestamp):
        self.locked = True

    def on_event(self, kind, event):
        """
        Called before any of the event callbacks.
        """

    def on_event_finished(self, kind, event):
        """
        Called after the respective event callbacks have been
        invoked. This method is *always* triggered, even if one of
        the callbacks requested the stop of the Hub.
        """

    def on_pair(self, myo, timestamp, firmware_version):
        """
        Called when a Myo armband is paired.
        """

    def on_unpair(self, myo, timestamp):
        """
        Called when a Myo armband is unpaired.
        """

    def on_disconnect(self, myo, timestamp):
        """
        Called when a Myo is disconnected.
        """

    def on_arm_sync(self, myo, timestamp, arm, x_direction, rotation,
                    warmup_state):
        """
        Called when a Myo armband and an arm is synced.
        """

    def on_arm_unsync(self, myo, timestamp):
        """
        Called when a Myo armband and an arm is unsynced.
        """

    def on_battery_level_received(self, myo, timestamp, level):
        """
        Called when the requested battery level received.
        """

    def on_warmup_completed(self, myo, timestamp, warmup_result):
        """
        Called when the warmup completed.
        """
'''
###################################################################################################
def main():

    client = OSCClient()
    client.connect(("192.168.137.192", 9001))
    client.connect(("192.168.137.1", 9001))
    print(str(client))

    server = OSCServer(("localhost", 7110))
    server.timeout = 0
    run = True

    # Setup Arduino
    use_arduino = True
    if use_arduino:
        servo_controller = ServoControl()
       # try:
        #    hub = libmyo.Hub()
        #except MemoryError:
        #    print("Myo Hub could not be created. Make sure Myo Connect is running.")
        #    return

        #hub.set_locking_policy(libmyo.LockingPolicy.none)
        #myo_listener = Listener(servo_controller)
        #hub.run(1000, myo_listener)

    #Setup Myo


    # this method of reporting timeouts only works by convention
    # that before calling handle_request() field .timed_out is
    # set to False
    def handle_timeout(self):
        self.timed_out = True

    # funny python's way to add a method to an instance of a class
    import types
    server.handle_timeout = types.MethodType(handle_timeout, server)

    def user_callback(path, tags, args, source):
        # which user will be determined by path:
        # we just throw away all slashes and join together what's left
        user = ''.join(path.split("/"))
        # tags will contain 'fff'
        # args is a OSCMessage with data
        # source is where the message came from (in case you need to reply)
        print ("Now do something with", user, args[2], args[0], 1 - args[1])

    server.addMsgHandler("/unity", user_callback)

    # user script that's called by the game engine every frame
    def each_frame():
        # clear timed_out flag
        server.timed_out = False
        # handle all pending requests then return
        while not server.timed_out:
            server.handle_request()


    capWebcam = cv2.VideoCapture(0)         # declare a VideoCapture object and associate to webcam, 0 => use 1st webcam

    if capWebcam.isOpened() == False:               # check if VideoCapture object was associated to webcam successfully
        print "error: capWebcam not accessed successfully\n\n"      # if not, print error message to std out
        os.system("pause")                                          # pause until user presses a key so user can see error message
        return                                                      # and exit function (which exits program)

    while cv2.waitKey(1) != 27 and capWebcam.isOpened():            # until the Esc key is pressed or webcam connection is lost
        blnFrameReadSuccessfully, imgOriginal = capWebcam.read()            # read next frame

        if not blnFrameReadSuccessfully or imgOriginal is None:     # if frame was not read successfully
            print "error: frame not read from webcam\n"             # print error message to std out
            os.system("pause")                                      # pause until user presses a key so user can see error message
            break                                                   # exit while loop (which exits program)

        imgGrayscale = cv2.cvtColor(imgOriginal, cv2.COLOR_BGR2GRAY)    # convert to grayscale

        imgBlurred = cv2.GaussianBlur(imgGrayscale, (5, 5), 0)          # blur

        imgCanny = cv2.Canny(imgBlurred, 100, 200)                      # get Canny edges

        cv2.namedWindow("imgOriginal", cv2.WINDOW_NORMAL)        # create windows, use WINDOW_AUTOSIZE for a fixed window size
        cv2.namedWindow("imgCanny", cv2.WINDOW_NORMAL)           # or use WINDOW_NORMAL to allow window resizing

        # detect circles in the image
        circles = cv2.HoughCircles(imgCanny, cv.CV_HOUGH_GRADIENT, 1, 260, param1=30, param2=10, minRadius=0, maxRadius=0)
        # print circles

        # ensure at least some circles were found
        osc_circles = []
        if circles is not None:
            # convert the (x, y) coordinates and radius of the circles to integers
            circles = np.round(circles[0, :]).astype("int")

            # loop over the (x, y) coordinates and radius of the circles
            for (x, y, r) in circles:
                # time.sleep(0.5)
                #print "Column Number: "
                #print x
                #print "Row Number: "
                #print y
                #print "Radius is: "
                #print r
                osc_circles.append((x,y))
        cv2.imshow("imgOriginal", imgOriginal)  # show windows
        cv2.imshow("imgCanny", imgCanny)
        message = OSCMessage("/cam")
        message.append(osc_circles)
        client.send(message)
        if use_arduino:
            if osc_circles is not None and osc_circles != []:
                if osc_circles[0][0] > 180:
                    servo_controller.WriteToSerial(-8)
                else:
                    servo_controller.WriteToSerial(9)
        print(message)
    # end while

    cv2.destroyAllWindows()     # remove windows from memory
    if use_arduino:
        print("Shutting down hub...")
        #hub.shutdown()

    return

###################################################################################################
if __name__ == "__main__":
    main()