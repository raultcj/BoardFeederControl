#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgcodecs\imgcodecs.hpp"
#include "opencv2\imgproc\imgproc.hpp"
#include "opencv2\videoio\videoio.hpp"

#include "serialConnection.h"

#include <iostream>
#include <stdio.h>
#include <stdlib.h>

using namespace cv;
using namespace std;

String src_window = "Source";
String vis_window = "Visualization";

int minSize; //Variables to be changed by the recipe, depending on the board.
int maxSize;

Mat src, vis;
vector<Point> pointList;

void cameraCapture(int, void*) {
	VideoCapture cap(0); //Capturing default camera

	if (!cap.isOpened()) {
		cerr << "Unable to open camera." << endl;
		return;
	}

	cap >> src;
}

vector<Vec3f> getCircles(Mat source) {
	vector<Vec3f> circles;
	Mat gray;

	cvtColor(src, gray, COLOR_BGR2GRAY);
	GaussianBlur(gray, gray, Size(9, 9), 2, 2);
	HoughCircles(gray, circles, HOUGH_GRADIENT, 1, gray.rows / 16, 100, 30, minSize, maxSize);

	return circles;
}

double getAngle(vector<Point> points) {
	double angle;
	int deltaX, deltaY;

	if (points.size() <= 1) {
		cout << "More points required to calculate angle." << endl;
		return 0;
	}

	deltaX = points[1].x - points[0].x;
	deltaY = points[1].y - points[0].x;

	if (deltaX != 0) {
		angle = atan(deltaY / deltaX);
	}
	else {
		cout << "Board aligned." << endl;
	}

	return angle;
}

int main(int argc, char **argv) {
	//At least two arguments must be passed, the min size of the holes and the max size.
	if (argc != 3) {
		cerr << "Insufficient arguments passed." << endl;
		return (0);
	}
	else {
		minSize = atoi(argv[1]);
		maxSize = atoi(argv[2]);

		//The min size must be smaller than the max size.
		if (minSize > maxSize) {
			cerr << "Invalid arguments passed (min > max)." << endl;
			return(0);
		}
	}

	while (true) {
		//Initiating camera capture.
		cameraCapture(-1, 0);
		src.copyTo(vis);

		//Declare both the source window and the program's visualization of the circles.
		namedWindow(src_window, CV_WINDOW_AUTOSIZE);
		namedWindow(vis_window, CV_WINDOW_AUTOSIZE);

		cout << "Press ESC key to terminate" << endl;

		//Find the circles that will determine the angle.
		vector<Vec3f> circles = getCircles(src);

		cout << "Detected " << circles.size() << " circles that comply with the parameters." << endl;

		//Resetting points vector.
		pointList.clear();

		for (size_t i = 0; i < circles.size(); i++) {
			Point center(cvRound(circles[i][0]), cvRound(circles[i][1]));
			int radius = cvRound(circles[i][2]);

			//Add point to pointList vector.
			pointList.push_back(center);
			//Draw circle on Vis window.
			circle(vis, center, 15, Scalar(0, 0, 255), -1, 8, 0);

			cout << "Circle " << i << endl
				<< "x: " << center.x << endl
				<< "y: " << center.y << endl;
		}

		//Get angle of error based on the points.
		cout << getAngle(pointList) << endl;

		//Send angle of error to the Serial Port.
		//connect(getAngle(pointList));

		//Display source and vis window.
		imshow(src_window, src);
		imshow(vis_window, vis);

		if (waitKey(0) == 27) {
			break;
		}
	}
	return (0);
}