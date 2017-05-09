#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgcodecs\imgcodecs.hpp"
#include "opencv2\imgproc\imgproc.hpp"
#include "opencv2\videoio\videoio.hpp"

#include <iostream>
#include <stdio.h>
#include <stdlib.h>

using namespace cv;
using namespace std;

int minSize = 39; //Variables to be changed by the recipe, depending on the board.
int maxSize = 45;

Mat src, vis;

String src_window = "Source";
String vis_window = "Visualization";

void cameraCapture(int, void*) {
	VideoCapture cap(0); //Capturing default camera

	if (!cap.isOpened()) {
		cerr << "Unable to open camera." << endl;
		return;
	}

	cap >> src;

	try {
		imshow(src_window, src);
	}
	catch (Exception e) {
		cout << "Window not yet created." << endl;
	}
}

vector<Vec3f> getCircles(Mat source) {
	vector<Vec3f> circles;
	Mat gray;

	cvtColor(src, gray, COLOR_BGR2GRAY);
	GaussianBlur(gray, gray, Size(9, 9), 2, 2);
	HoughCircles(gray, circles, HOUGH_GRADIENT, 1, gray.rows / 16, 100, 30, minSize, maxSize);

	return circles;
}

int main(int argc, char **argv) {
	cout << "Select capture method:" << endl
		<< "1) Image file" << endl
		<< "2) Camera capture" << endl
		<< "3) Exit" << endl;

	int ans;
	cin >> ans;
	string filename;

	switch (ans) {
	case 1:
		cout << "Insert filename" << endl;
		cin >> filename;
		src = imread(filename, 1);
		src.copyTo(vis);
		break;
	case 2:
		cameraCapture(-1, 0);
		break;
	default:
		return (0);
		break;
	}

	if (src.empty()) {
		cerr << "Blank frame grabbed\n";
		return (0);
	}

	namedWindow(src_window, CV_WINDOW_NORMAL);
	namedWindow(vis_window, CV_WINDOW_NORMAL);

	cout << "Press ESC key to terminate" << endl;

	vector<Vec3f> circles = getCircles(src);

	cout << "Detected " << circles.size()
		<< " circles that comply with the parameters." << endl;

	for (size_t i = 0; i < circles.size(); i++) {
		Point center(cvRound(circles[i][0]), cvRound(circles[i][1]));
		int radius = cvRound(circles[i][2]);

		circle(vis, center, 15, Scalar(0, 0, 255), -1, 8, 0);

		cout << "Circle " << i << endl
			<< "x: " << center.x << endl
			<< "y: " << center.y << endl;
	}

	imshow(src_window, src);
	imshow(vis_window, vis);

	waitKey(0);
	return (0);
}