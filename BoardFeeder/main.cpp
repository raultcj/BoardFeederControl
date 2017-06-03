#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgcodecs\imgcodecs.hpp"
#include "opencv2\imgproc\imgproc.hpp"
#include "opencv2\videoio\videoio.hpp"

#include <iostream>
#include <stdio.h>
#include <stdlib.h>

using namespace cv;
using namespace std;

String pathTempl;
String src_window = "Source";
String vis_window = "Visualization";

Mat src, vis;

void cameraCapture(int, void*) {
	VideoCapture cap(0); //Capturing default camera
	cap.set(CV_CAP_PROP_FRAME_HEIGHT, 1080);
	cap.set(CV_CAP_PROP_FRAME_WIDTH, 1920);

	if (!cap.isOpened()) {
		cerr << "Unable to open camera." << endl;
		return;
	}

	cap >> src;
}

bool compareCapture(Mat source, Mat templ) {
	bool pass = false;
	int cols = source.cols - templ.cols + 1;
	int rows = source.rows - templ.rows + 1;

	Mat result(cols, rows, CV_32FC1);
	matchTemplate(source, templ, result, CV_TM_CCOEFF);
	normalize(result, result, 0, 255.0, NORM_MINMAX, CV_8UC1, Mat());

	Mat resultMask;
	threshold(result, resultMask, 180, 255.0, THRESH_BINARY);

	vector <vector<Point> > draw;
	findContours(resultMask, draw, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE, Point(templ.cols / 2, templ.rows / 2));

	vector <vector<Point> >::iterator i;

	for (i = draw.begin(); i != draw.end(); i++) {
		Moments m = moments(*i, false);
		Point centroid(m.m10 / m.m00, m.m01 / m.m00);
		circle(src, centroid, 40, Scalar(0, 0, 255), -1, 8, 0);
	}

	if (!result.empty()) {
		pass = true;
	}

	return pass;
}

int main(int argc, char **argv) {
	//At least two arguments must be passed, the min size of the holes and the max size.
	if (argc != 2) {
		cerr << "Insufficient arguments passed. Must include ID of the board." << endl;
		//pathTempl = "C:\\Users\\Raul Juarez\\Pictures\\Camera Roll\\rpi.jpg";
		return -1;
	}
	else {
		//Verify integrity of the argument pased.
		try {
			pathTempl = argv[1];
		}
		catch (Exception e) {
			cout << "Error parsing ID string." << endl;
			return -1;
		}
	}

	while (true) {
		//Initiating camera capture.
		cameraCapture(-1, 0);
		Mat templ = imread(pathTempl, 1);

		if (templ.empty()) {
			cout << "No template found." << endl;
			return -1;
		}

		//Declare both the source window and the program's visualization of the circles.
		namedWindow(src_window, CV_WINDOW_NORMAL);

		if (compareCapture(src, templ)) {
			cout << "Board Passed." << endl;
			return 0;
		}
		else {
			cout << "Board NOT Passed." << endl;
			return -1;
		}

		//Display source and vis window.
		imshow(src_window, src);

		if (waitKey(0) == 27) {
			break;
		}
	}
	return (0);
}