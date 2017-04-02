# CannyWebcam.py
import argparse

import cv2
import numpy as np
import os
import cv2.cv as cv
from OSC import OSCClient, OSCMessage, OSCServer


###################################################################################################
def main():

    client = OSCClient()
    client.connect(("192.168.137.1", 9001))
    print(str(client))

    server = OSCServer(("localhost", 7110))
    server.timeout = 0
    run = True

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
        circles = cv2.HoughCircles(imgCanny, cv.CV_HOUGH_GRADIENT, 1, 260, param1=30, param2=20, minRadius=0, maxRadius=0)
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
        #print(message)
    # end while

    cv2.destroyAllWindows()                 # remove windows from memory

    return

###################################################################################################
if __name__ == "__main__":
    main()