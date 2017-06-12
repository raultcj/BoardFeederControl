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
	VideoCapture cap(1); //Capturing default camera
	cap.set(CV_CAP_PROP_FRAME_HEIGHT, 1080);
	cap.set(CV_CAP_PROP_FRAME_WIDTH, 1920);

	if (!cap.isOpened()) {
		cerr << "Unable to open camera." << endl;
		return;
	}

	cap.read(src);
}

bool compareCapture(Mat source, Mat templ) {
	bool pass = false;
	int cols = source.cols - templ.cols + 1;
	int rows = source.rows - templ.rows + 1;

	Mat result(cols, rows, CV_32F);

	//blur(templ, templ, Size(15, 15), Point(-1, -1));

	//imshow("sayy", templ);
	//waitKey(0);

	matchTemplate(source, templ, result, CV_TM_CCOEFF_NORMED);
	//normalize(result, resultMinMax, 0, 1, NORM_MINMAX, -1, Mat());

	double minVal, maxVal;
	Point minLoc, maxLoc, matchLoc;

	minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc);

	matchLoc = maxLoc;

	//rectangle(src, matchLoc, Point(matchLoc.x + templ.cols, matchLoc.y + templ.rows), Scalar(255, 0, 0), 2, 8, 0);
	//rectangle(src, minLoc, Point(minLoc.x + templ.cols, minLoc.y + templ.rows), Scalar(0, 255, 0), 2, 8, 0);
	//rectangle(result, matchLoc, Point(matchLoc.x + templ.cols, matchLoc.y + templ.rows), Scalar(255, 0, 0), 2, 8, 0);

	/*cout << maxVal << endl;
	cout << minVal << endl;
	cout << "***" << endl;

	cout << maxLoc.x << endl;
	cout << maxLoc.y << endl;*/

	if (maxVal > 0.45) {
		pass = true;
	}
	else {
		pass = false;
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

	//Initiating camera capture.
	cameraCapture(0, 0);
	Mat templ = imread(pathTempl);
	cvtColor(templ, templ, CV_BGR2GRAY);
	cvtColor(src, src, CV_BGR2GRAY);

	if (templ.empty()) {
		cout << "No template found." << endl;
		return -1;
	}

	//Declare both the source window and the program's visualization of the circles.
	//namedWindow(src_window, CV_WINDOW_NORMAL);
	//namedWindow(vis_window, CV_WINDOW_NORMAL);

	if (compareCapture(src, templ)) {
		cout << "Board Passed." << endl;
		//return 0;
	}
	else {
		cout << "Board NOT Passed." << endl;
		//return -1;
	}

	//Display source and vis window.
	//imshow(src_window, src);
	//imshow(vis_window, templ);

	return (0);
}